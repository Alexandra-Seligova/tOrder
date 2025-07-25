//===================================================================
// $Workfile:: CapacityUnitDashboard.xaml.cs                        $
// $Author:: Alexandra_Seligova                                     $
// $Revision:: 1                                                    $
// $Date:: 2025-06-19 18:25:00 +0200 (èt, 19 èvn 2025)              $
//===================================================================
// Description: SPC - tOrder
//     CapacityUnitDashboard – main dashboard for capacity unit.
//     Handles tab selection, loads tab content dynamically using
//     TabSelectorView and ContentPresenter.
//===================================================================

namespace tOrder.UI;

//-----------------------------------------------------------
#region Using directives
//-----------------------------------------------------------
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

#endregion //Using directives

//===================================================================
// class CapacityUnitDashboard
//===================================================================

public sealed partial class CapacityUnitDashboard : Page
{
    /// <summary>
    /// Layout configuration settings (e.g., scale, width) provided via DI.
    /// </summary>
    public LayoutConfigVM LayoutConfig => App.GetService<LayoutConfigVM>();

    //-----------------------------------------------------------
    #region Constructor
    //-----------------------------------------------------------

    public CapacityUnitDashboard()
    {
        this.InitializeComponent();
        TabSelectorView.TabChanged += OnTabChanged;
        ShowTabContent(0); // Default tab: Dashboard
    }

    #endregion //Constructor

    //-----------------------------------------------------------
    #region Tab Selection & Content
    //-----------------------------------------------------------

    /// <summary>
    /// Handles tab change events from TabSelectorView.
    /// </summary>
    /// <param name="index">Selected tab index</param>
    private void OnTabChanged(int index)
    {
        ShowTabContent(index);
    }

    /// <summary>
    /// Dynamically loads the appropriate UserControl into the DashboardContentPresenter
    /// based on the selected tab index.
    /// </summary>
    /// <param name="index">Selected tab index</param>
    private void ShowTabContent(int index)
    {
        DashboardContentPresenter.Content = null;
        switch (index)
        {
            case 0:
                // "Dashboard" tab
                DashboardContentPresenter.Content = new DashboardContentView();
                break;

            case 1:
                // "Chargen/Behälter" tab
                DashboardContentPresenter.Content = new ChargenContentC();
                break;

            case 2:
                // "Werkzeuge" tab
                DashboardContentPresenter.Content = new WerkzeugeContentC();
                break;

            case 3:
                // "Qualität" tab
                //DashboardContentPresenter.Content = new QualitatContentC();
                break;

            case 4:
                // "Arbeitsplan" tab
                //DashboardContentPresenter.Content = new ArbeitsplanContentC();
                break;

            case 5:
                // "Dokumenten" tab
                //DashboardContentPresenter.Content = new DokumentenContentC();
                break;

            case 6:
                // "Info" tab
                //DashboardContentPresenter.Content = new InfoContentC();
                break;

            case 7:
                // "OEE" tab
                //DashboardContentPresenter.Content = new OeeContentC();
                break;

            default:
                // For any other index, show nothing or a placeholder
                DashboardContentPresenter.Content = null;
                break;
        }
    }

    #endregion //Tab Selection & Content
}
//===================================================================
