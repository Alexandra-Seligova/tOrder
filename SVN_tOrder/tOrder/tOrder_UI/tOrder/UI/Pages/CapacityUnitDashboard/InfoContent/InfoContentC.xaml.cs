//===================================================================
// $Workfile:: InfoContentC.xaml.cs                                 $
// $Author:: Alexandra_Seligova                                     $
// $Revision:: 1                                                    $
// $Date:: 2025-08-15 22:25:00 +0200 (èt, 15 srp 2025)              $
//===================================================================
// Description:  - tOrder
//     InfoContentC – tab control for Info section in CapacityUnitDashboard.
//     Hosts tab buttons (header) and content switching via SelectedTab.
//===================================================================

namespace tOrder.UI;

//-----------------------------------------------------------
#region Using directives
//-----------------------------------------------------------
using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.UI.Text;

#endregion //Using directives

//===================================================================
// class InfoContentC
//===================================================================

public sealed partial class InfoContentC : UserControl
{
    //-----------------------------------------------------------
    #region Dependency Properties
    //-----------------------------------------------------------

    /// <summary>
    /// Selected tab name identifier (e.g. "ProductionOrder", "Shift").
    /// Controls which content is visible and updates visual state.
    /// </summary>
    public static readonly DependencyProperty SelectedTabProperty =
        DependencyProperty.Register(nameof(SelectedTab), typeof(string), typeof(InfoContentC),
            new PropertyMetadata("ProductionOrder", OnSelectedTabChanged));

    public string SelectedTab
    {
        get => (string)GetValue(SelectedTabProperty);
        set => SetValue(SelectedTabProperty, value);
    }

    #endregion //Dependency Properties

    //-----------------------------------------------------------
    #region Constructor
    //-----------------------------------------------------------

    public InfoContentC()
    {
        this.InitializeComponent();
        UpdateTabVisuals(); // Apply initial state
    }

    #endregion //Constructor

    //-----------------------------------------------------------
    #region Event Handlers
    //-----------------------------------------------------------

    /// <summary>
    /// Handles click event from tab buttons.
    /// Updates SelectedTab based on button.Tag.
    /// </summary>
    private void Tab_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button button && button.Tag is string tag)
        {
            SelectedTab = tag;
        }
    }

    /// <summary>
    /// Called when SelectedTab changes; updates button/indicator visuals.
    /// </summary>
    private static void OnSelectedTabChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is InfoContentC control)
        {
            control.UpdateTabVisuals();
        }
    }

    #endregion //Event Handlers

    //-----------------------------------------------------------
    #region Visual Updates
    //-----------------------------------------------------------

    /// <summary>
    /// Updates visual state of all tab buttons and indicator bars.
    /// </summary>
    private void UpdateTabVisuals()
    {
        void SetTabState(Button btn, Border indicator, bool isActive)
        {
            btn.Foreground = (Brush)Resources[isActive ? "TabActiveBrush" : "TabInactiveTextBrush"];
            btn.FontWeight = (FontWeight)Resources[isActive ? "TabButtonFontWeightBold" : "TabButtonFontWeightNormal"];
            indicator.Background = (Brush)Resources[isActive ? "TabIndicatorActiveBrush" : "TabIndicatorInactiveBrush"];
            indicator.Margin = (Thickness)Resources[isActive ? "TabIndicatorMarginActive" : "TabIndicatorMarginInactive"];
        }

        SetTabState(TabButton_0, Indicator_0, SelectedTab == "ProductionOrder");
        SetTabState(TabButton_1, Indicator_1, SelectedTab == "Shift");
        SetTabState(TabButton_2, Indicator_2, SelectedTab == "InboundContainers");
        SetTabState(TabButton_3, Indicator_3, SelectedTab == "OutboundContainers");
        SetTabState(TabButton_4, Indicator_4, SelectedTab == "ContainersForProcessing");
    }

    #endregion //Visual Updates
}
//===================================================================
