using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;

namespace tOrder.UI
{
    public sealed partial class CapacityUnitDashboard : Page
    {
        public CapacityUnitDashboard()
        {
            this.InitializeComponent();
            TabSelectorView.TabChanged += OnTabChanged;
            ShowTabContent(0); // V�choz� z�lo�ka: Dashboard
        }

        private void OnTabChanged(int index)
        {
            ShowTabContent(index);
        }

        private void ShowTabContent(int index)
        {
            DashboardContentPresenter.Content = null;
            switch (index)
            {
                case 0:
                    // Z�lo�ka "Dashboard"
                    DashboardContentPresenter.Content = new DashboardContentView();
                    break;

                case 1:
                    // Z�lo�ka "Chargen/Beh�lter"
                    DashboardContentPresenter.Content = new ChargenContentC();
                    break;

                case 2:
                    // Z�lo�ka "Werkzeuge"
                    DashboardContentPresenter.Content = new WerkzeugeContentC();
                    break;

                case 3:
                    // Z�lo�ka "Qualit�t"
                    //DashboardContentPresenter.Content = new QualitatContentC();
                    break;

                case 4:
                    // Z�lo�ka "Arbeitsplan"
                    //DashboardContentPresenter.Content = new ArbeitsplanContentC();
                    break;

                case 5:
                    // Z�lo�ka "Dokumenten"
                    //DashboardContentPresenter.Content = new DokumentenContentC();
                    break;

                case 6:
                    // Z�lo�ka "Info"
                    //DashboardContentPresenter.Content = new InfoContentC();
                    break;

                case 7:
                    // Z�lo�ka "OEE"
                    //DashboardContentPresenter.Content = new OeeContentC();
                    break;

                default:
                    // Pro ostatn� indexy placeholder nebo pr�zdno
                    DashboardContentPresenter.Content = null;
                    break;
            }
        }
    }
}
