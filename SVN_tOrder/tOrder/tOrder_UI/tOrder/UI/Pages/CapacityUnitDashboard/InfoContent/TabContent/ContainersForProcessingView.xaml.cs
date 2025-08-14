using Microsoft.UI.Xaml.Controls;

namespace tOrder.UI
{
    public sealed partial class ContainersForProcessingView : UserControl
    {
        public LayoutConfigVM LayoutConfig => App.GetService<LayoutConfigVM>();
        public ContainersForProcessingView()
        {
            this.InitializeComponent();
        }
    }
}
