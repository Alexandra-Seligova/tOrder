using Microsoft.UI.Xaml.Controls;

namespace tOrder.UI
{
    public sealed partial class ProductionOrderView : UserControl
    {
        public LayoutConfigVM LayoutConfig => App.GetService<LayoutConfigVM>();

        public ProductionOrderView()
        {
            this.InitializeComponent();
        }
    }
}
