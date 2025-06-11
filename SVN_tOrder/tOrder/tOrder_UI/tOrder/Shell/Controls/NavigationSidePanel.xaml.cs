//===================================================================
// $Workfile:: NavigationSidePanel.xaml.cs                         $
// $Author:: Alexandra_Seligova                                    $
// $Revision:: 4                                                   $
// $Date:: 2025-06-11 15:44:33 +0200 (st, 11 čvn 2025)            $
//===================================================================
// Description: SPC - tOrder
//     UserControl code-behind for left-side navigation panel
//===================================================================

namespace tOrder.Shell;

//-----------------------------------------------------------
#region Using directives
//-----------------------------------------------------------

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

#endregion //Using directives

/// <summary>
/// Left-side navigation panel used in MainWindow layout.
/// Provides method for toggling visibility.
/// </summary>
public sealed partial class NavigationSidePanel : UserControl
{
    //-----------------------------------------------------------
    #region Constructor
    //-----------------------------------------------------------

    public NavigationSidePanel()
    {
        InitializeComponent();
    }

    #endregion

}
