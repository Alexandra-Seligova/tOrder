using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;

namespace tOrder.UI
{
    public sealed partial class InboundContainersView : UserControl
    {
        public LayoutConfigVM LayoutConfig => App.GetService<LayoutConfigVM>();

        public ObservableCollection<InboundOperation> PreviousOperations { get; set; }

        public InboundContainersView()
        {
            this.InitializeComponent();

            // Init sample data
            PreviousOperations = new ObservableCollection<InboundOperation>
            {
                new InboundOperation
                {
                    Datum = "20.5.2024 13:13:13",
                    Schicht = "Tagschicht",
                    Bediener = "1712 Friedrich II. der Große",
                    ProdAktivitaet = "Mikron Multistar mit NX",
                    Kapazitaetsstelle = "MST-03",
                    Gesamtmenge = "6666 Stk."
                }
            };

            this.DataContext = this;
        }
    }

    public class InboundOperation
    {
        public string Datum { get; set; }
        public string Schicht { get; set; }
        public string Bediener { get; set; }
        public string ProdAktivitaet { get; set; }
        public string Kapazitaetsstelle { get; set; }
        public string Gesamtmenge { get; set; }
    }
}
