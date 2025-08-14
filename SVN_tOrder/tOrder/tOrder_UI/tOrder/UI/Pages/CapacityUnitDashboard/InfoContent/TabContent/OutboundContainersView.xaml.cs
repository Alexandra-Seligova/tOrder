using Microsoft.UI.Xaml.Controls;

namespace tOrder.UI
{
    public sealed partial class OutboundContainersView : UserControl
    {
        public LayoutConfigVM LayoutConfig => App.GetService<LayoutConfigVM>();

        public OutboundContainersView()
        {
            this.InitializeComponent();
        }
    }
}
