//===================================================================
// $Workfile:: InfoTabsHeadC.xaml.cs                                $
// $Author:: Alexandra_Seligova                                     $
// $Revision:: 2                                                    $
// $Date:: 2025-08-15                                               $
//===================================================================
// Description:  - tOrder
//     InfoTabsHeadC – UI control for sub-tab selection in InfoContentC.
//     Provides tab highlighting, switching, and event notification.
//===================================================================

namespace tOrder.UI;

//-----------------------------------------------------------
#region Using directives
//-----------------------------------------------------------
using System;
using Microsoft.UI;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.UI.Text; // FontWeight / FontWeights
#endregion //Using directives

//===================================================================
// class InfoTabsHeadC
//===================================================================

public sealed partial class InfoTabsHeadC : UserControl
{
    //-----------------------------------------------------------
    #region Fields & Properties
    //-----------------------------------------------------------

    /// <summary>
    /// Raised when the user selects a tab.
    /// </summary>
    public event Action<string>? TabChanged;

    /// <summary>
    /// Currently selected tab key.
    /// </summary>
    public string SelectedTab
    {
        get => (string)GetValue(SelectedTabProperty);
        set => SetValue(SelectedTabProperty, value);
    }

    public static readonly DependencyProperty SelectedTabProperty =
        DependencyProperty.Register(
            nameof(SelectedTab),
            typeof(string),
            typeof(InfoTabsHeadC),
            new PropertyMetadata("ProductionOrder", OnSelectedTabChanged));

    #endregion //Fields & Properties

    //-----------------------------------------------------------
    #region Constructor
    //-----------------------------------------------------------

    public InfoTabsHeadC()
    {
        InitializeComponent();
        UpdateTabVisuals(SelectedTab);
    }

    #endregion //Constructor

    //-----------------------------------------------------------
    #region Event Handlers
    //-----------------------------------------------------------

    private void OnTabClick(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is string tag)
        {
            SelectedTab = tag; // Triggers change and visual update
        }
    }

    private static void OnSelectedTabChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is InfoTabsHeadC control && e.NewValue is string newTab)
        {
            control.UpdateTabVisuals(newTab);
            control.TabChanged?.Invoke(newTab);
        }
    }

    #endregion //Event Handlers

    //-----------------------------------------------------------
    #region Visual Updates
    //-----------------------------------------------------------

    /// <summary>
    /// Updates the visual state of all tab buttons and indicator bars.
    /// Expects unified InfoTabs.* resources; falls back safely if missing.
    /// </summary>
    private void UpdateTabVisuals(string activeKey)
    {
        if (TabRoot is null || TabRoot.Children is null) return;

        var brushActive = GetResource<Brush>("InfoTabs.TextActiveBrush", new SolidColorBrush(Colors.Black));
        var brushInactive = GetResource<Brush>("InfoTabs.TextInactiveBrush", new SolidColorBrush(Colors.Gray));
        var indActive = GetResource<Brush>("InfoTabs.IndicatorActive", new SolidColorBrush(Colors.Black));
        var indInactive = GetResource<Brush>("InfoTabs.IndicatorInactive", new SolidColorBrush(Colors.Transparent));

        // Optional margins (present only if you define them in XAML)
        var marginActive = GetResource<Thickness>("InfoTabs.IndicatorMarginActive", new Thickness(0));
        var marginInactive = GetResource<Thickness>("InfoTabs.IndicatorMarginInactive", new Thickness(0));

        var fwActive = GetResource<FontWeight>("InfoTabs.FontWeight.Active", FontWeights.Bold);
        var fwInactive = GetResource<FontWeight>("InfoTabs.FontWeight.Inactive", FontWeights.Normal);

        foreach (var child in TabRoot.Children)
        {
            if (child is StackPanel stack &&
                stack.Children.Count >= 2 &&
                stack.Children[0] is Button button &&
                stack.Children[1] is Border indicator)
            {
                bool isActive = (button.Tag as string) == activeKey;

                button.Foreground = isActive ? brushActive : brushInactive;
                button.FontWeight = isActive ? fwActive : fwInactive;

                indicator.Background = isActive ? indActive : indInactive;
                indicator.Margin = isActive ? marginActive : marginInactive;
            }
        }
    }

    #endregion //Visual Updates

    //-----------------------------------------------------------
    #region Helpers
    //-----------------------------------------------------------

    private T GetResource<T>(string key, T fallback)
    {
        if (Resources != null && Resources.TryGetValue(key, out var obj) && obj is T typed)
            return typed;
        return fallback;
    }

    #endregion //Helpers
}
//===================================================================
