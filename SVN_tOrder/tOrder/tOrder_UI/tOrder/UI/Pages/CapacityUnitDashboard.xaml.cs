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
            ShowTabContent(0); // Defaultn� Dashboard
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
                    DashboardContentPresenter.Content = new DashboardContentView();
                    break;
                // p��padn� p�id� dal�� case 1, 2, 3 ... pro jin� taby
                default:
                    // M��e� d�t placeholder, nebo nic
                    DashboardContentPresenter.Content = null;
                    break;
            }
        }
    }
}
