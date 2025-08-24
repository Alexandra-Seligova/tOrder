//===================================================================
// $Workfile:: ShiftView.xaml.cs                                    $
// $Author:: Alexandra_Seligova                                     $
// $Revision:: 1                                                    $
// $Date:: 2025-08-16                                               $
//===================================================================
// Description:  - tOrder
//     View for tab "Shift" in InfoContentC.
//     Displays shift-level production statistics and internal defects
//     in a two-column grid layout, with footer action buttons.
//===================================================================

#nullable enable

namespace tOrder.UI;

//-----------------------------------------------------------
#region Using directives
//-----------------------------------------------------------
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using tOrder.Common;
#endregion // Using directives

//===================================================================
// class ShiftView
//===================================================================

public sealed partial class ShiftView : UserControl
{
    //-----------------------------------------------------------
    #region Services & ViewModel Access
    //-----------------------------------------------------------

    /// <summary>
    /// Provides LayoutConfig for scaling via BindingProxy in XAML.
    /// </summary>
    public LayoutConfigVM LayoutConfig => App.GetService<LayoutConfigVM>();

    #endregion // Services & ViewModel Access

    //-----------------------------------------------------------
    #region Constructor
    //-----------------------------------------------------------

    public ShiftView()
    {
        InitializeComponent();
    }

    #endregion // Constructor

    //-----------------------------------------------------------
    #region Event Handlers
    //-----------------------------------------------------------

    /// <summary>
    /// Handles click on "Alternative Arb.Pl." footer button.
    /// </summary>
    private void OnAlternativeArbeitsplatzClicked(object sender, RoutedEventArgs e)
    {
        // TODO: Implement navigation or action for alternative workplace.
        System.Diagnostics.Debug.WriteLine("[ShiftView] Alternative Arb.Pl. clicked.");
    }

    /// <summary>
    /// Handles click on "TBK Lager" footer button.
    /// </summary>
    private void OnTbkLagerClicked(object sender, RoutedEventArgs e)
    {
        // TODO: Implement navigation or action for TBK storage.
        System.Diagnostics.Debug.WriteLine("[ShiftView] TBK Lager clicked.");
    }

    #endregion // Event Handlers
}
//===================================================================
