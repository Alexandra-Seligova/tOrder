using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;

namespace tOrder.UI
{
    public sealed partial class OutboundContainersView : UserControl
    {
        public LayoutConfigVM LayoutConfig => App.GetService<LayoutConfigVM>();

        public ObservableCollection<OutboundContent> CurrentContents { get; set; }

        public OutboundContainersView()
        {
            this.InitializeComponent();

            // Sample data
            CurrentContents = new ObservableCollection<OutboundContent>
            {
                new OutboundContent
                {
                    Datum = "",
                    Schicht = "",
                    Bediener = "",
                    Zuwachs = "",
                    Gesamtmenge = ""
                }
            };

            this.DataContext = this;
        }
    }

    public class OutboundContent
    {
        public string Datum { get; set; }
        public string Schicht { get; set; }
        public string Bediener { get; set; }
        public string Zuwachs { get; set; }
        public string Gesamtmenge { get; set; }
    }
}
