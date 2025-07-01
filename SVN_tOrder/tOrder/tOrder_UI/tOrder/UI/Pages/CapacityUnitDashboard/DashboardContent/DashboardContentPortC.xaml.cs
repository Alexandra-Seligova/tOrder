namespace tOrder.UI;

using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using tOrder.Common;

/// <summary>
/// DashboardContentPortC - komponenta pro zobrazení vstupu/výstupu s kontejnerem (kyblíkem).
/// 
/// Bindované vstupy a vlastnosti:
/// - string HeaderText         → Nadpis v hlavičce (např. "Eingang Behälter")
/// - string OutputText         → Text k výstupu (např. "Ausgang 1.")
/// - string ButtonText         → Text na tlačítku (např. "Behälter öffnen...")
/// - PortContainerM ContainerModel → Objekt kontejneru s daty (viz PortContainerM.cs)
///     • bool IsInput              → Určuje, zda jde o vstup nebo výstup
///     • uint PieceCount           → Aktuální počet kusů v kontejneru
///     • int? ContainerId          → ID přiřazeného kontejneru (null = žádný)
/// - bool IsActionEnabled      → Povoluje/deaktivuje tlačítko
/// 
/// Události:
/// - ContainerClosed           → Vyvoláno po úspěšném odpisu (zavření) kontejneru
/// </summary>

public sealed partial class DashboardContentPortC : UserControl
{
    public ObservableCollection<PortContainerM> PortContainers { get; } = new();
    public double TabWidth
    {
        get => (double)GetValue(TabWidthProperty);
        set => SetValue(TabWidthProperty, value);
    }
    public DashboardContentPortC()
    {
        this.InitializeComponent();

        // Připojení handleru velikosti
        this.SizeChanged += UserControl_SizeChanged;

        // 🧪 Testovací naplnění 2 kontejnerů
        PortContainers.Add(new PortContainerM
        {
            IsInput = true,
            PieceCount = 6540,
            ContainerId = 10100,
            //   Description = "Edelstahl Teile"
        });

        PortContainers.Add(new PortContainerM
        {
            IsInput = false,
            PieceCount = 3120,
            ContainerId = 2,
            //   Description = "Nachbearbeitung"
        });
        PortContainers.Add(new PortContainerM
        {
            IsInput = false,
            PieceCount = 123456789,
            ContainerId = 1234,
            //   Description = "Nachbearbeitung"
        });
        PortContainers.Add(new PortContainerM
        {
            IsInput = true,
            PieceCount = 6540,
            ContainerId = 10100,
            //   Description = "Edelstahl Teile"
        });

        PortContainers.Add(new PortContainerM
        {
            IsInput = false,
            PieceCount = 3120,
            ContainerId = 2,
            //   Description = "Nachbearbeitung"
        });
        PortContainers.Add(new PortContainerM
        {
            IsInput = false,
            PieceCount = 123456789,
            ContainerId = 1234,
            //   Description = "Nachbearbeitung"
        });

        // Výchozí výběr
        SelectedTabIndex = 0;
        ContainerModel = PortContainers[SelectedTabIndex];
    }

    #region Tab selection


    public static readonly DependencyProperty TabWidthProperty =
        DependencyProperty.Register(nameof(TabWidth), typeof(double), typeof(DashboardContentPortC), new PropertyMetadata(80.0));

    private void UserControl_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        int count = PortContainers.Count;
        if (count > 0)
        {
            double totalWidth = this.ActualWidth;
            TabWidth = totalWidth / count;
        }
    }


    public int SelectedTabIndex { get; set; } = 0;

    private void OnTabSelected(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is int index && index < PortContainers.Count)
        {
            SelectedTabIndex = index;
            ContainerModel = PortContainers[index];
        }
    }

    #endregion

    #region Dependency Properties
    public int TabColumns
    {
        get => (int)GetValue(TabColumnsProperty);
        set => SetValue(TabColumnsProperty, value);
    }

    public static readonly DependencyProperty TabColumnsProperty =
        DependencyProperty.Register(nameof(TabColumns), typeof(int), typeof(DashboardContentPortC), new PropertyMetadata(1));







    public string? OutputTypeText
    {
        get => (string?)GetValue(OutputTypeTextProperty);
        set => SetValue(OutputTypeTextProperty, value);
    }

    public static readonly DependencyProperty OutputTypeTextProperty =
        DependencyProperty.Register(nameof(OutputTypeText), typeof(string), typeof(DashboardContentPortC), new PropertyMetadata(null));

    public int? SelectedContainerId
    {
        get => (int?)GetValue(SelectedContainerIdProperty);
        set => SetValue(SelectedContainerIdProperty, value);
    }

    public static readonly DependencyProperty SelectedContainerIdProperty =
        DependencyProperty.Register(nameof(SelectedContainerId), typeof(int?), typeof(DashboardContentPortC), new PropertyMetadata(null));

    public string ButtonText
    {
        get => (string)GetValue(ButtonTextProperty);
        set => SetValue(ButtonTextProperty, value);
    }

    public static readonly DependencyProperty ButtonTextProperty =
        DependencyProperty.Register(nameof(ButtonText), typeof(string), typeof(DashboardContentPortC), new PropertyMetadata("Behälter öffnen zur\nBearbeitung"));

    public string HeaderText
    {
        get => (string)GetValue(HeaderTextProperty);
        set => SetValue(HeaderTextProperty, value);
    }

    public static readonly DependencyProperty HeaderTextProperty =
        DependencyProperty.Register(nameof(HeaderText), typeof(string), typeof(DashboardContentPortC), new PropertyMetadata("Eingang Behälter"));

    public string OutputText
    {
        get => (string)GetValue(OutputTextProperty);
        set => SetValue(OutputTextProperty, value);
    }

    public static readonly DependencyProperty OutputTextProperty =
        DependencyProperty.Register(nameof(OutputText), typeof(string), typeof(DashboardContentPortC), new PropertyMetadata("Ausgang 1."));

    #endregion

    #region ContainerModel Property

    public PortContainerM ContainerModel
    {
        get => (PortContainerM)GetValue(ContainerModelProperty);
        set => SetValue(ContainerModelProperty, value);
    }

    public static readonly DependencyProperty ContainerModelProperty =
        DependencyProperty.Register(nameof(ContainerModel), typeof(PortContainerM), typeof(DashboardContentPortC),
            new PropertyMetadata(null, OnContainerModelChanged));

    private static void OnContainerModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is DashboardContentPortC control && e.NewValue is PortContainerM model)
        {
            control.ApplyModel(model);
        }
    }

    private void ApplyModel(PortContainerM model)
    {
        // HeaderText = model.IsInput ? "Eingang Behälter" : "Ausgang Behälter";
        /* OutputText = model.IsInput
             ? $"Ausgang {model.ContainerId?.ToString() ?? "-"}."
             : $"Eingang {model.ContainerId?.ToString() ?? "-"}.";*/

        IsActionEnabled = model.ContainerId.HasValue && model.PieceCount > 0;
    }

    #endregion

    #region Button Enabled Property

    public bool IsActionEnabled
    {
        get => (bool)GetValue(IsActionEnabledProperty);
        set => SetValue(IsActionEnabledProperty, value);
    }

    public static readonly DependencyProperty IsActionEnabledProperty =
        DependencyProperty.Register(nameof(IsActionEnabled), typeof(bool), typeof(DashboardContentPortC), new PropertyMetadata(false));

    #endregion

    #region Event: ContainerClosed

    public event EventHandler ContainerClosed;

    private void OnCloseContainer()
    {
        // Vyvolání události pro parent (např. PageViewModel)
        ContainerClosed?.Invoke(this, EventArgs.Empty);
    }

    #endregion

    private void TabButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is int id)
        {
            SelectedContainerId = id;
            ContainerModel = PortContainers.FirstOrDefault(p => p.ContainerId == id);
        }
    }




    // Styl aktivního vs. neaktivního tlačítka
    public Style GetTabButtonStyle(int index)
    {
        var isSelected = index == SelectedTabIndex;
        var key = isSelected ? "TabButtonSelectedStyle" : "TabButtonUnselectedStyle";
        return (Style)Resources[key];
    }
    public Style GetTabButtonStyleFromItem(int? containerId)
    {
        int index = PortContainers.IndexOf(PortContainers.FirstOrDefault(x => x.ContainerId == containerId));
        bool isSelected = index == SelectedTabIndex;
        var key = isSelected ? "TabButtonSelectedStyle" : "TabButtonUnselectedStyle";
        return (Style)this.Resources[key];
    }
    private void TabButton_Loaded(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Content is TextBlock tb)
        {
            int index = TabRepeater.GetElementIndex(btn);
            tb.Text = (index + 1).ToString(); // 1-based
        }
    }


}
