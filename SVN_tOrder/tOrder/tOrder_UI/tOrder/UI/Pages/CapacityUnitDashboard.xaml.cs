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
            ShowTabContent(0); // Výchozí záložka: Dashboard
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
                    // Záložka "Dashboard"
                    DashboardContentPresenter.Content = new DashboardContentView();
                    break;

                case 1:
                    // Záložka "Chargen/Behälter"
                    DashboardContentPresenter.Content = new ChargenContentC();
                    break;

                case 2:
                    // Záložka "Werkzeuge"
                    DashboardContentPresenter.Content = new WerkzeugeContentC();
                    break;

                case 3:
                    // Záložka "Qualität"
                    //DashboardContentPresenter.Content = new QualitatContentC();
                    break;

                case 4:
                    // Záložka "Arbeitsplan"
                    //DashboardContentPresenter.Content = new ArbeitsplanContentC();
                    break;

                case 5:
                    // Záložka "Dokumenten"
                    //DashboardContentPresenter.Content = new DokumentenContentC();
                    break;

                case 6:
                    // Záložka "Info"
                    //DashboardContentPresenter.Content = new InfoContentC();
                    break;

                case 7:
                    // Záložka "OEE"
                    //DashboardContentPresenter.Content = new OeeContentC();
                    break;

                default:
                    // Pro ostatní indexy placeholder nebo prázdno
                    DashboardContentPresenter.Content = null;
                    break;
            }
        }
    }
}
