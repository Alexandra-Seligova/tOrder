using Microsoft.UI.Xaml.Controls;

namespace tOrder.UI
{
    public sealed partial class InboundContainersView : UserControl
    {
        public LayoutConfigVM LayoutConfig => App.GetService<LayoutConfigVM>();

        public InboundContainersView()
        {
            this.InitializeComponent();
        }
    }
}
