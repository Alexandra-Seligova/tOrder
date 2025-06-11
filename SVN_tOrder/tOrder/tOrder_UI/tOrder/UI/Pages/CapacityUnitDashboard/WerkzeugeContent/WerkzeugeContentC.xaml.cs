
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System.Collections.ObjectModel;
using Windows.UI;


namespace tOrder.UI
{
    public class WerkzeugRow
    {
        public string KapStelle { get; set; }
        public int Seite { get; set; }
        public string Lage { get; set; }
        public string Stueckzaehler { get; set; }
        public string RueckNr { get; set; }
        public string Werkzeug { get; set; }
        public string WZSeite { get; set; }
        public int Zaehler { get; set; }
        public string StandMenge { get; set; }
        public string Restkap { get; set; }
        public string VerwKap { get; set; }
        public string GeaendertAm { get; set; }
        public string GeaendertDurch { get; set; }
        // Pro barvení procent (možno i jako Brush, zde string)
        public Brush PercentCellBackground { get; set; }
    }

    public sealed partial class WerkzeugeContentC : UserControl
    {
        public ObservableCollection<WerkzeugRow> Rows { get; set; } = new();

        public WerkzeugeContentC()
        {
            this.InitializeComponent();

            // Ukázková data pøesnì podle tvého obrázku
            Rows.Add(new WerkzeugRow
            {
                KapStelle = "MST-01A",
                Seite = 1,
                Lage = "S1/1",
                Stueckzaehler = "10.654",
                RueckNr = "38252",
                Werkzeug = "51-02430",
                WZSeite = "1/1",
                Zaehler = 100,
                StandMenge = "13.000",
                Restkap = "2.446",
                VerwKap = "82%",
                GeaendertAm = "01.01.2024 07:00:00",
                GeaendertDurch = "6666",
                PercentCellBackground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 234, 255, 236)) // zelená
            });
            Rows.Add(new WerkzeugRow
            {
                KapStelle = "MST-01A",
                Seite = 1,
                Lage = "S2/1",
                Stueckzaehler = "10.654",
                RueckNr = "38252",
                Werkzeug = "51-00388",
                WZSeite = "1/1",
                Zaehler = 100,
                StandMenge = "13.000",
                Restkap = "2.446",
                VerwKap = "82%",
                GeaendertAm = "01.01.2024 07:00:00",
                GeaendertDurch = "6666",
                PercentCellBackground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 234, 255, 236))
            });
            Rows.Add(new WerkzeugRow
            {
                KapStelle = "MST-01A",
                Seite = 1,
                Lage = "S3/1",
                Stueckzaehler = "10.654",
                RueckNr = "38252",
                Werkzeug = "51-02509",
                WZSeite = "1/1",
                Zaehler = 100,
                StandMenge = "13.000",
                Restkap = "2.446",
                VerwKap = "82%",
                GeaendertAm = "01.01.2024 07:00:00",
                GeaendertDurch = "6666",
                PercentCellBackground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 234, 255, 236))
            });
            Rows.Add(new WerkzeugRow
            {
                KapStelle = "MST-01A",
                Seite = 1,
                Lage = "S5/1",
                Stueckzaehler = "10.654",
                RueckNr = "38252",
                Werkzeug = "51-02565",
                WZSeite = "1/1",
                Zaehler = 100,
                StandMenge = "10.000",
                Restkap = "0",
                VerwKap = "105%",
                GeaendertAm = "01.01.2024 07:00:00",
                GeaendertDurch = "6666",
                PercentCellBackground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 213, 213)) // èervená
            });
            Rows.Add(new WerkzeugRow
            {
                KapStelle = "MST-01A",
                Seite = 1,
                Lage = "S6/1",
                Stueckzaehler = "10.654",
                RueckNr = "38252",
                Werkzeug = "51-0887",
                WZSeite = "1/6",
                Zaehler = 100,
                StandMenge = "11.000",
                Restkap = "346",
                VerwKap = "96%",
                GeaendertAm = "01.01.2024 07:00:00",
                GeaendertDurch = "6666",
                PercentCellBackground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 255, 249, 224)) // žlutá
            });
            Rows.Add(new WerkzeugRow
            {
                KapStelle = "MST-01A",
                Seite = 1,
                Lage = "SP3/1",
                Stueckzaehler = "10.654",
                RueckNr = "38252",
                Werkzeug = "51-01687",
                WZSeite = "1/1",
                Zaehler = 100,
                StandMenge = "13.000",
                Restkap = "2.446",
                VerwKap = "82%",
                GeaendertAm = "01.01.2024 07:00:00",
                GeaendertDurch = "6666",
                PercentCellBackground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 234, 255, 236))
            });
            Rows.Add(new WerkzeugRow
            {
                KapStelle = "MST-01A",
                Seite = 1,
                Lage = "SP4/1",
                Stueckzaehler = "10.654",
                RueckNr = "38252",
                Werkzeug = "51-01440",
                WZSeite = "3/4",
                Zaehler = 100,
                StandMenge = "13.000",
                Restkap = "2.446",
                VerwKap = "82%",
                GeaendertAm = "01.01.2024 07:00:00",
                GeaendertDurch = "6666",
                PercentCellBackground = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 234, 255, 236))
            });

            // Pøipoj data do gridu (x:Name="WerkzeugeDataGrid")
            this.WerkzeugeDataGrid.ItemsSource = Rows;
        }
    }
}
