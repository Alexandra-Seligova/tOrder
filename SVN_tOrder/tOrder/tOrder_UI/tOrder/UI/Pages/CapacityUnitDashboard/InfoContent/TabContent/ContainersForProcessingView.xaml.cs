using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;

namespace tOrder.UI
{
    public sealed partial class ContainersForProcessingView : UserControl
    {
        public LayoutConfigVM LayoutConfig => App.GetService<LayoutConfigVM>();

        public ObservableCollection<OperationItem> Operations { get; } = new();
        public ObservableCollection<ContentItem> CurrentContents { get; } = new();

        public ContainersForProcessingView()
        {
            this.InitializeComponent();

            // Sample data for first table
            Operations.Add(new OperationItem
            {
                Datum = "20.5.2024 13:13:13",
                BehNr = "111",
                RueckmNr = "999999",
                Schicht = "Tagschicht",
                Bediener = "1712",
                ProdAktivitaet = "Mikron Multistar mit NX",
                KapStelle = "MST-03",
                Tz = "10",
                ArbPlan = "F 00V H28 042/2",
                GesamtStk = "6666"
            });

            // Sample data for second table
            CurrentContents.Add(new ContentItem
            {
                Datum = "20.5.2024 21:21:21",
                Schicht = "Nachtschicht 1",
                Bediener = "1797 Wilhelm I.",
                Zuwachs = "+2222 Stk.",
                Gesamtmenge = "2222 Stk."
            });
            CurrentContents.Add(new ContentItem
            {
                Datum = "20.5.2024 23:23:23",
                Schicht = "Nachtschicht 2",
                Bediener = "1831 Friedrich III.",
                Zuwachs = "+4444 Stk.",
                Gesamtmenge = "6666 Stk."
            });
        }
    }

    public class OperationItem
    {
        public string Datum { get; set; }
        public string BehNr { get; set; }
        public string RueckmNr { get; set; }
        public string Schicht { get; set; }
        public string Bediener { get; set; }
        public string ProdAktivitaet { get; set; }
        public string KapStelle { get; set; }
        public string Tz { get; set; }
        public string ArbPlan { get; set; }
        public string GesamtStk { get; set; }
    }

    public class ContentItem
    {
        public string Datum { get; set; }
        public string Schicht { get; set; }
        public string Bediener { get; set; }
        public string Zuwachs { get; set; }
        public string Gesamtmenge { get; set; }
    }
}
