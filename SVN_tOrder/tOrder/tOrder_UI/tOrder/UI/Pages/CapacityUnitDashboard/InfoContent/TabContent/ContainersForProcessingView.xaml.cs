//===================================================================
// $Workfile:: ContainersForProcessingView.xaml.cs                   $
// $Author:: Alexandra_Seligova                                     $
// $Revision:: 1                                                    $
// $Date:: 2025-08-15 23:42:00 +0200 (čt, 15 srp 2025)              $
//===================================================================
// Description: SPC - tOrder
//     View for tab "Containers for Processing" in InfoContentC.
//     Displays order detail fields and flags for further processing.
//===================================================================

namespace tOrder.UI;

//-----------------------------------------------------------
#region Using directives
//-----------------------------------------------------------
using Microsoft.UI.Xaml.Controls;
using tOrder.Common;

#endregion // Using directives

//===================================================================
// class ContainersForProcessingView
//===================================================================

public sealed partial class ContainersForProcessingView : UserControl
{


    public LayoutConfigVM LayoutConfig => App.GetService<LayoutConfigVM>();

    //-----------------------------------------------------------
    #region Constructor
    //-----------------------------------------------------------

    public ContainersForProcessingView()
    {
        this.InitializeComponent();
        this.DataContext = this;
    }

    #endregion // Constructor
}
//===================================================================
