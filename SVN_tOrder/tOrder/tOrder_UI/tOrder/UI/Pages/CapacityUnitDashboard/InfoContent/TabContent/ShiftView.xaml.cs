using Microsoft.UI.Xaml.Controls;

namespace tOrder.UI
{
    public sealed partial class ShiftView : UserControl
    {
        public LayoutConfigVM LayoutConfig => App.GetService<LayoutConfigVM>();

        public ShiftView()
        {
            this.InitializeComponent();
        }
    }
}
