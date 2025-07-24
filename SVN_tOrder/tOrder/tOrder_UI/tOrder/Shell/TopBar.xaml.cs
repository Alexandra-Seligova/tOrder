//===================================================================
// $Workfile:: TopBar.xaml.cs                                       $
// $Author:: Alexandra_Seligova                                     $
// $Revision:: 1                                                    $
// $Date:: 2025-07-25 00:15:00 +0200 (pá, 25 čvc 2025)              $
//===================================================================
// Description: SPC - tOrder
//     Code-behind for the TopBar control.
//     Displays header, breadcrumbs, user info and notifications.
//===================================================================

namespace tOrder.Shell;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

/// <summary>
/// Code-behind for the TopBar UserControl.
/// </summary>
public sealed partial class TopBar : UserControl
{
    //-----------------------------------------------------------
    #region Fields & Properties
    //-----------------------------------------------------------

    /// <summary>
    /// ViewModel instance for the TopBar view.
    /// </summary>
    public TopBarVM VM { get; }

    private string _tooltipText = "TopBar";

    /// <summary>
    /// Dynamic tooltip describing current top bar dimensions.
    /// </summary>
    public string TooltipText
    {
        get => _tooltipText;
        set
        {
            if (_tooltipText != value)
            {
                _tooltipText = value;
                ToolTipService.SetToolTip(TopBarBorder, _tooltipText);
            }
        }
    }

    #endregion // Fields & Properties

    //-----------------------------------------------------------
    #region Constructor
    //-----------------------------------------------------------

    /// <summary>
    /// Initializes a new instance of the <see cref="TopBar"/> class.
    /// </summary>
    public TopBar()
    {
        this.InitializeComponent();

        VM = App.GetService<TopBarVM>();
        this.DataContext = VM;

        TopBarGrid.SizeChanged += TopBarGrid_SizeChanged;

        UpdateTooltip();
    }

    #endregion // Constructor

    //-----------------------------------------------------------
    #region Events
    //-----------------------------------------------------------

    /// <summary>
    /// Updates the tooltip text when the grid size changes.
    /// </summary>
    private void TopBarGrid_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        UpdateTooltip();
    }

    /// <summary>
    /// Sets the formatted tooltip with grid size and column count.
    /// </summary>
    private void UpdateTooltip()
    {
        int columnCount = TopBarGrid.ColumnDefinitions.Count;
        TooltipText = $"TopBar {TopBarGrid.ActualWidth:0}x{TopBarGrid.ActualHeight:0} {columnCount}xcol.";
    }

    #endregion // Events
}
