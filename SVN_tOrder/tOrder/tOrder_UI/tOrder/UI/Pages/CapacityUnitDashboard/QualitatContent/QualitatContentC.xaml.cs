using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace tOrder.UI;

public sealed partial class QualitatContentC : UserControl
{
    public LayoutConfigVM LayoutConfig => App.GetService<LayoutConfigVM>();

    public ObservableCollection<QualitatMessungItem> AllItems { get; set; } = new();
    public ObservableCollection<QualitatMessungItem> FilteredItems { get; set; } = new();

    private string? _activeFilter = null;

    public QualitatContentC()
    {
        InitializeComponent();
        AllItems = LoadSampleData();
        UpdateFilter(null);
    }

    #region Filtrace

    private void UpdateFilter(string? regelkarte)
    {
        _activeFilter = regelkarte;
        FilteredItems.Clear();

        var items = string.IsNullOrEmpty(regelkarte)
            ? AllItems
            : new ObservableCollection<QualitatMessungItem>(AllItems.Where(i => i.Regelkarte == regelkarte));

        foreach (var item in items)
            FilteredItems.Add(item);

        // aktualizace vizuálního stavu toggle buttonů
        if (FilterGroup1 != null) FilterGroup1.IsChecked = regelkarte == "SPC";
        if (FilterGroup2 != null) FilterGroup2.IsChecked = regelkarte == "Unwert";
        if (FilterGroup3 != null) FilterGroup3.IsChecked = regelkarte == "Attributiv";
    }

    private void OnFilterClicked(object sender, RoutedEventArgs e)
    {
        if (sender is ToggleButton btn)
        {
            string? selected = btn.Name switch
            {
                "FilterGroup1" => "SPC",
                "FilterGroup2" => "Unwert",
                "FilterGroup3" => "Attributiv",
                _ => null
            };

            if (selected is null) return;

            // přepínání mezi aktivací a deaktivací filtru
            if (_activeFilter == selected)
                UpdateFilter(null);
            else
                UpdateFilter(selected);
        }
    }

    private void OnFilterClearClicked(object sender, RoutedEventArgs e)
    {
        UpdateFilter(null);
    }

    #endregion

    #region Not Implemented Alert

    private void OnUnimplementedClicked(object sender, RoutedEventArgs e)
    {
        var dialog = new ContentDialog
        {
            Title = "Funkce není implementována",
            Content = "Tato funkce je prozatím vypnuta.",
            CloseButtonText = "OK",
            XamlRoot = this.XamlRoot
        };
        _ = dialog.ShowAsync();
    }

    #endregion

    #region Testovací data

    private ObservableCollection<QualitatMessungItem> LoadSampleData()
    {
        return new ObservableCollection<QualitatMessungItem>
        {
            new() { Pos = 10, Merk = "D100", Beschreibung = "Außen-ø 11,6 +/-0,04", Regelkarte = "SPC", ZeitintervallMin = 240, Stueckintervall = 240 },
            new() { Pos = 40, Merk = "L205", Beschreibung = "Gesamtlänge 4,12 +/- 0,02", Regelkarte = "SPC", ZeitintervallMin = 240, Stueckintervall = 240 },
            new() { Pos = 50, Merk = "L200", Beschreibung = "Länge 1,84 +/- 0,03", Regelkarte = "SPC", ZeitintervallMin = 240, Stueckintervall = 240 },
            new() { Pos = 60, Merk = "D160", Beschreibung = "Bohrungs-ø 1,4 +/- 0,05", Regelkarte = "SPC", ZeitintervallMin = 240, Stueckintervall = 240 },
            new() { Pos = 70, Merk = "L200", Beschreibung = "Länge 2,61 +/- 0,03", Regelkarte = "SPC", ZeitintervallMin = 240, Stueckintervall = 240 },
            new() { Pos = 130, Merk = "W010", Beschreibung = "Winkel 25° ±5° (2x)", Regelkarte = "Unwert", ZeitintervallMin = 180, Stueckintervall = 180 },
            new() { Pos = 140, Merk = "L265", Beschreibung = "Tiefe 0,1 +/- 0,05 (2x)", Regelkarte = "Unwert", ZeitintervallMin = 180, Stueckintervall = 180 },
            new() { Pos = 150, Merk = "L260", Beschreibung = "Bohrtiefe 1,5 -0,1", Regelkarte = "Unwert", ZeitintervallMin = 180, Stueckintervall = 180 },
            new() { Pos = 160, Merk = "F520", Beschreibung = "Ebenheit 0,04", Regelkarte = "Attributiv", ZeitintervallMin = 200, Stueckintervall = 200 },
            new() { Pos = 170, Merk = "K300", Beschreibung = "Kanten -0,085/-0,02", Regelkarte = "Attributiv", ZeitintervallMin = 200, Stueckintervall = 200 },
            new() { Pos = 180, Merk = "K300", Beschreibung = "Kanten -0,14/-0,04", Regelkarte = "Attributiv", ZeitintervallMin = 200, Stueckintervall = 200 }
        };
    }

    #endregion
}

public class QualitatMessungItem
{
    public int Pos { get; set; }
    public string Merk { get; set; } = string.Empty;
    public string Beschreibung { get; set; } = string.Empty;
    public string Regelkarte { get; set; } = string.Empty;
    public int ZeitintervallMin { get; set; }
    public int Stueckintervall { get; set; }
    public DateTime? LetzteMessungDatum { get; set; }
    public int? LetzteMessungZaehler { get; set; }
    public DateTime? NaechsteMessungDatum { get; set; }
    public int? NaechsteMessungZaehler { get; set; }
}
