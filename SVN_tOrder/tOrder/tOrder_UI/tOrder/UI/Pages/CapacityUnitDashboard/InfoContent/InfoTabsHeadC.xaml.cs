//===================================================================
// $Workfile:: InfoTabsHeadC.xaml.cs                                $
// $Author:: Alexandra_Seligova                                     $
// $Revision:: 1                                                    $
// $Date:: 2025-08-15                                               $
//===================================================================
// Description: SPC - tOrder
//     InfoTabsHeadC – UI control for sub-tab selection in InfoContentC.
//     Provides tab highlighting, switching, and event notification.
//===================================================================

namespace tOrder.UI;

//-----------------------------------------------------------
#region Using directives
//-----------------------------------------------------------
using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using Microsoft.UI.Text;
using Windows.UI.Text;
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
        DependencyProperty.Register(nameof(SelectedTab), typeof(string), typeof(InfoTabsHeadC),
            new PropertyMetadata("ProductionOrder", OnSelectedTabChanged));

    #endregion //Fields & Properties

    //-----------------------------------------------------------
    #region Constructor
    //-----------------------------------------------------------

    public InfoTabsHeadC()
    {
        this.InitializeComponent();
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
    /// </summary>
    /// <param name="activeKey">The tag (string) of the currently selected tab.</param>
    private void UpdateTabVisuals(string activeKey)
    {
        for (int i = 0; i < TabRoot.Children.Count; i++)
        {
            if (TabRoot.Children[i] is StackPanel stack
                && stack.Children[0] is Button button
                && stack.Children[1] is Border border)
            {
                bool isActive = (string)button.Tag == activeKey;

                button.Foreground = (SolidColorBrush)Resources[isActive ? "TabActiveBrush" : "TabInactiveTextBrush"];
                button.FontWeight = (FontWeight)Resources[isActive ? "TabButtonFontWeightBold" : "TabButtonFontWeightNormal"];
                border.Background = (SolidColorBrush)Resources[isActive ? "TabIndicatorActiveBrush" : "TabIndicatorInactiveBrush"];
                border.Margin = (Thickness)Resources[isActive ? "TabIndicatorMarginActive" : "TabIndicatorMarginInactive"];
            }
        }
    }

    #endregion //Visual Updates
}
//===================================================================