using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;

namespace tOrder.UI;

public sealed partial class ArbeitsplanContentC : UserControl
{
    public LayoutConfigVM LayoutConfig => App.GetService<LayoutConfigVM>();

    public ObservableCollection<ArbeitsplanRow> ArbeitsplanRows { get; set; } = new();

    public ArbeitsplanContentC()
    {
        this.InitializeComponent();

        // Pøíkladová data dle obrázku
        ArbeitsplanRows.Add(new ArbeitsplanRow
        {
            Afo = 30,
            TzVon = 20,
            TzBis = 30,
            Beschreibung = "Mikron Multistar mit NX",
            Arbeitsgang = "MULTISTAR NX",
            KapGruppe = "MST NX",
            AfoTyp = "Normal",
            Extern = "-"
        });
        ArbeitsplanRows.Add(new ArbeitsplanRow
        {
            Afo = 35,
            TzVon = 30,
            TzBis = 35,
            Beschreibung = "Rissprüfung Sortierautomat",
            Arbeitsgang = "RISSPRUEFUNG",
            KapGruppe = "SORT",
            AfoTyp = "Normal",
            Extern = "-"
        });
        ArbeitsplanRows.Add(new ArbeitsplanRow
        {
            Afo = 40,
            TzVon = 35,
            TzBis = 40,
            Beschreibung = "Sortiermaschinen",
            Arbeitsgang = "SORTIERMASCHINE",
            KapGruppe = "SORT",
            AfoTyp = "Normal",
            Extern = "-"
        });

        // Napojení na DataGrid
        ArbeitsplanDataGrid.ItemsSource = ArbeitsplanRows;
    }

    private void OnAlternativeArbeitsplatzClicked(object sender, RoutedEventArgs e)
    {
        var dialog = new ContentDialog
        {
            Title = "Alternative Arb.Pl",
            Content = "Tato funkce zatím není implementována.",
            CloseButtonText = "OK",
            XamlRoot = this.XamlRoot
        };
        _ = dialog.ShowAsync();
    }

    private void OnTbkLagerClicked(object sender, RoutedEventArgs e)
    {
        var dialog = new ContentDialog
        {
            Title = "TBK Lager",
            Content = "Tato funkce zatím není implementována.",
            CloseButtonText = "OK",
            XamlRoot = this.XamlRoot
        };
        _ = dialog.ShowAsync();
    }
}

public class ArbeitsplanRow
{
    public int Afo { get; set; }
    public int TzVon { get; set; }
    public int TzBis { get; set; }
    public string Beschreibung { get; set; } = string.Empty;
    public string Arbeitsgang { get; set; } = string.Empty;
    public string KapGruppe { get; set; } = string.Empty;
    public string AfoTyp { get; set; } = string.Empty;
    public string Extern { get; set; } = string.Empty;
}
