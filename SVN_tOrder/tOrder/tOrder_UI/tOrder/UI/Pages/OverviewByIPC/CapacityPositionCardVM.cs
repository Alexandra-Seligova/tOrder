//===================================================================
// $Workfile:: CapacityPositionCardVM.cs                           $
// $Author:: Alexandra_Seligova                                     $
// $Revision:: 4                                                   $
// $Date:: 2025-06-11 15:44:33 +0200 (st, 11 čvn 2025)            $
//===================================================================
// Description:  - tOrder
//     ViewModel for CapacityPositionCard – dynamic and static binding of unit station.
//===================================================================

#region Info context

//-----------------------------------------------------------
// 📘 Context – ViewModel Mapping Guide
//-----------------------------------------------------------
//
// 🧩 DATA SOURCE ↔️ VIEWMODEL ↔️ VIEW
//
// • CapacityUnitM (UnitId)             → UnitId (VM)       → MachineId (XAML)
// • CapacityPositionM (Code)   → UnitCode         → MachineCode (XAML)
// • CapacityPositionM (Index)  → Index
// • CapacityState enum              → Status (mapped string)
// • ProductionType                  → Type
// • Counter                         → Counter
//
// 🧑 PERSON INFO
// • LoggedInPersons (List<PersonPointsM>) → PersonPoints
//     → [0] = VisiblePerson
//     → Skip(1) = AdditionalPersons
//     → HasMoreUsers / HasAdditionalPersons
//
// 📋 INFO FIELDS → MainInfo = ObservableCollection<InfoItem>
//     • [0] Prod.Auftrag
//     • [1] Artikel
//     • [2] Häringcharge
//     • [3] Materialcharge
//
// 🏷️ LOCALIZATION STRINGS
//     • TextStateRunning, TextOrderNumber, TextUnknown...
//     → připraveno pro lokalizaci do .resw
//
// 📐 LAYOUT BINDING
//     • LayoutCardWidth        → Width karty
//     • LayoutPopupWidth       → Width popupu (match)
//     • LayoutMarginCardContent → Outer Margin (např. StackPanel)
//     • LayoutPaddingCardText   → Padding uvnitř komponenty
//     • LayoutRightValueMargin  → Odsazení zprava (číselné hodnoty)
//
// 📌 NOTE:
// Tento výčet slouží jako „mapa“ pro udržitelnost – 
// jasně ukazuje, odkud každá hodnota pochází a kam směřuje.
//
//-----------------------------------------------------------

#endregion


namespace tOrder.UI;

//-----------------------------------------------------------
#region Using directives
//-----------------------------------------------------------

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using tOrder.Common;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using Windows.UI;
using tOrder.Common;

#endregion

//-----------------------------------------------------------
#region ENUMS & STRUCTS
//-----------------------------------------------------------
public enum SectionType
{
    Production,
    Menu,
    OEE
}

/// <summary>
/// Logical states of a capacity position/unit.
/// </summary>



public enum CapacityState
{
    // máme i neznámý stav 
    //TODO: inicializace enumů co se stane když tam nic nedám bude defaultní? 
    [Description("noData!")]
    Undefined = 0,


    [Description("frei")]
    Off,
    [Description("läuft")]
    Running,
    [Description("gestoppt")]
    Stopped,
    [Description("einstellung")]
    Setup
}

public static class CapacityStateExtensions
{
    public static SolidColorBrush GetStatusBrush(this CapacityState state)
    {
        return state switch
        {
            CapacityState.Off => new SolidColorBrush(Color.FromArgb(255, 192, 57, 43)),
            CapacityState.Running => new SolidColorBrush(Color.FromArgb(255, 0, 153, 61)),
            CapacityState.Stopped => new SolidColorBrush(Color.FromArgb(255, 255, 140, 0)),
            CapacityState.Setup => new SolidColorBrush(Color.FromArgb(255, 100, 50, 160)),
            _ => new SolidColorBrush(Colors.Gray)
        };
    }

    public static SolidColorBrush GetIndexBrush(this CapacityState state)
    {
        return state switch
        {
            CapacityState.Off => new SolidColorBrush(Color.FromArgb(255, 140, 140, 140)),
            CapacityState.Running => new SolidColorBrush(Color.FromArgb(255, 0, 153, 61)),
            CapacityState.Stopped => new SolidColorBrush(Color.FromArgb(255, 255, 140, 0)),
            CapacityState.Setup => new SolidColorBrush(Color.FromArgb(255, 150, 60, 200)),
            _ => new SolidColorBrush(Colors.Gray)
        };
    }

    public static string ToLocalizedString(this CapacityState state, string lang = "de")
    {
        return lang switch
        {
            "de" => state switch
            {
                CapacityState.Off => "Frei",           // červená nejsou potřebná data
                CapacityState.Running => "Läuft",      // zelená
                CapacityState.Stopped => "Gestoppt",   // oranžová
                CapacityState.Setup => "Einstellung",  // červená
                _ => "undefined"
            },
            "en" => state switch
            {
                CapacityState.Off => "Free",
                CapacityState.Running => "Running",
                CapacityState.Stopped => "Stopped",
                CapacityState.Setup => "Setup",
                _ => "undefined"
            },
            _ => state.ToString()
        };
    }
}

/// <summary>
/// Simple model representing a person and their point score.
/// Used in footer displays of CapacityPositionCard.
/// </summary>

public static class MyColorHelper
{
    public static Color FromHex(string hex)
    {
        hex = hex?.Replace("#", string.Empty);
        if (string.IsNullOrEmpty(hex))
            throw new ArgumentException("Empty hex color");

        if (hex.Length == 6)
        {
            // RRGGBB
            byte r = Convert.ToByte(hex.Substring(0, 2), 16);
            byte g = Convert.ToByte(hex.Substring(2, 2), 16);
            byte b = Convert.ToByte(hex.Substring(4, 2), 16);
            return Color.FromArgb(255, r, g, b);
        }
        if (hex.Length == 8)
        {
            // AARRGGBB
            byte a = Convert.ToByte(hex.Substring(0, 2), 16);
            byte r = Convert.ToByte(hex.Substring(2, 2), 16);
            byte g = Convert.ToByte(hex.Substring(4, 2), 16);
            byte b = Convert.ToByte(hex.Substring(6, 2), 16);
            return Color.FromArgb(a, r, g, b);
        }
        throw new ArgumentException("Invalid hex color format: " + hex);
    }
    public static Color FromArgb(byte a, byte r, byte g, byte b)
    {
        return Color.FromArgb(a, r, g, b);
    }

}



public struct PortM
{
    public int Index { get; set; }
    public bool IsActive { get; set; }
    public int OutputValue { get; set; }
    public double Content { get; set; }
    public string Unit { get; set; }
}

public struct ActionM
{
    public string Key { get; set; }
    public bool IsEnabled { get; set; }
}
#endregion

//-----------------------------------------------------------
#region INFO PAIRS (Text/Value display)
//-----------------------------------------------------------

/// <summary>
/// Reusable key-value pair model for UI display of metadata.
/// Used in the Info block of CapacityPositionCard.
/// </summary>
public class InfoItem
{
    public string Key { get; set; }
    public string Value { get; set; }

    public InfoItem(string key, string value)
    {
        Key = key;
        Value = value;
    }
}

#endregion

//-----------------------------------------------------------
#region MODELS – CapacityUnit & Position
//-----------------------------------------------------------
public class MenuItemM
{
    public string Text { get; set; }
    public string Page { get; set; }
    public ICommand ClickCommand { get; set; } // nebo RelayCommand, DelegateCommand...

    public MenuItemM(string text, string page)
    {
        Text = text;
        Page = page;
    }
}

/// <summary>
/// CapacityUnitM – represents a machine/unit instance [Einheit].
/// Aggregates one or more CapacityPositions.
/// </summary>
public class CapacityUnitM
{
    public string UnitId { get; init; } // e.g. MST - 01

    public List<CapacityPositionM> Positions { get; set; } = [];

    public CapacityState State { get; set; } = CapacityState.Undefined;
    // NOVĚ: Barva celé jednotky pro "head" (zelená, modrá, fialová, ...)
    public Color UnitColor { get; set; } = Colors.Green; // Default barva

    // NOVĚ: barva hlavičky
    public string? UnitColorHex { get; set; }
    public SolidColorBrush UnitColorBrush =>
        !string.IsNullOrWhiteSpace(UnitColorHex) ? new SolidColorBrush(MyColorHelper.FromHex(UnitColorHex)) : new SolidColorBrush(Colors.Gray);

    public CapacityUnitM(string unitId) => UnitId = unitId;

    public void AddPosition(string code) =>
        Positions.Add(new CapacityPositionM(code, Positions.Count + 1));
}

/// <summary>
/// CapacityPositionM – represents one station/position on a unit [Position].
/// Contains local operational and personnel state.
/// </summary>
public class CapacityPositionM
{

    public string Code { get; set; }
    public int Index { get; set; }
    public string? OrderNumber { get; set; }
    public string? ArticleCode { get; set; }
    public string? HaeringCharge { get; set; }
    public string? MaterialBatch { get; set; }
    public string? ProductionType { get; set; }
    public CapacityState LocalState { get; set; }// automatická defaultní hodnota je = CapacityState.Undefined;
    public uint? Counter { get; set; }

    // --- NOVĚ: Barvy podle stavu ---
    public Color StatusColor { get; set; } = Colors.Red; // default
    public Color IndexColor { get; set; } = Colors.Gray; // default


    public string? StatusColorHex { get; set; }
    public string? IndexColorHex { get; set; }

    public SolidColorBrush StatusBrush =>
        !string.IsNullOrWhiteSpace(StatusColorHex) ? new SolidColorBrush(MyColorHelper.FromHex(StatusColorHex)) : LocalState.GetStatusBrush();

    public SolidColorBrush IndexBrush =>
        !string.IsNullOrWhiteSpace(IndexColorHex) ? new SolidColorBrush(MyColorHelper.FromHex(IndexColorHex)) : LocalState.GetIndexBrush();

    // OEE data
    public double OEE { get; set; }
    public double CapacityUtilization { get; set; }
    public double Efficiency { get; set; }
    public double Quality { get; set; }
    public string LastUpdateDate { get; set; }
    public string LastUpdateTime { get; set; }

    // QA/WZK metrics
    public int RestPiecesToWZK { get; set; }
    public int RestPiecesToQA { get; set; }
    public string RestTimeToQA { get; set; }

    // Associations
    public ObservableCollection<UserM> LoggedInPersons { get; set; } = new();
    public ObservableCollection<PortM> InputBins { get; set; } = new();
    public ObservableCollection<PortM> OutputBins { get; set; } = new();
    public ObservableCollection<ActionM> Actions { get; set; } = new();





    public CapacityPositionM(string code, int index) =>
        (Code, Index) = (code, index);




}

#endregion
public class SectionTemplateSelector : DataTemplateSelector
{
    public DataTemplate? ProductionTemplate { get; set; }
    public DataTemplate? MenuTemplate { get; set; }
    public DataTemplate? OeeTemplate { get; set; }

    protected override DataTemplate SelectTemplateCore(object item)
    {
        if (item is CardContentSwitcherVM vm)
        {
            return vm.ActiveSection switch
            {
                SectionType.Production => ProductionTemplate,
                SectionType.Menu => MenuTemplate,
                SectionType.OEE => OeeTemplate,
                _ => base.SelectTemplateCore(item)
            };
        }

        return base.SelectTemplateCore(item);
    }
}



//-----------------------------------------------------------
#region DISPLAY MODELS
//-----------------------------------------------------------

/// <summary>
/// Display model used for visualizing label + data pair.
/// Typically used in CardDataDisplay controls.
/// </summary>
public sealed class CardDataDisplayModel
{
    public string Label { get; }
    public string Data { get; }

    public CardDataDisplayModel(string label, string data) =>
        (Label, Data) = (label, data);
}

#endregion

//===================================================================
// class CapacityPositionCardVM
//===================================================================

public partial class CapacityPositionCardVM : BaseVM
{
    public CardContentSwitcherVM ContentSwitcherVM { get; }
    //-----------------------------------------------------------
    #region Fields
    //-----------------------------------------------------------

    private readonly CapacityUnitM m_unit;
    private readonly CapacityPositionM m_position;
    private readonly IDataService m_dataService;
    private bool _toggleLock;

    #endregion

    //-----------------------------------------------------------
    #region Observable Properties – Core Data
    //-----------------------------------------------------------

    [ObservableProperty] private ObservableCollection<InfoItem> mainInfo = [];
    [ObservableProperty] private ObservableCollection<CardDataDisplayModel> displayData = [];
    [ObservableProperty] private ObservableCollection<UserM> personPoints = [];

    [ObservableProperty] private UserM? visiblePerson = null;
    [ObservableProperty] private bool showOperatorPopup = false;
    [ObservableProperty]
    private ObservableCollection<MenuItemM> menuItems = [];
    [ObservableProperty]
    private string activeSectionName = "Production";
    [ObservableProperty]
    private SectionType activeSection = SectionType.Production; // Výchozí sekce


    public UserM VisiblePersonSafe => VisiblePerson ?? UserM.Null;

    public bool HasAdditionalPersons => PersonPoints.Count > 1;
    public IEnumerable<UserM> AdditionalPersons => PersonPoints.Skip(1);
    public bool HasMoreUsers => PersonPoints.Count > 1;
    public bool IsStueckZaehlerMissing => !Counter.HasValue || Counter == 0;


    [ObservableProperty] private string unitCode = string.Empty; // * Renamed from MachineCode
    [ObservableProperty] private string unitId = string.Empty;   // * Renamed from MachineId
    [ObservableProperty] private string index = string.Empty;
    [ObservableProperty] private string status = string.Empty;
    [ObservableProperty] private string type = string.Empty;
    [ObservableProperty] private uint? counter = null;



    [ObservableProperty] public CapacityState localState = CapacityState.Off;
    [ObservableProperty] private Color unitHeadColor = MyColorHelper.FromHex("#7FBE92");   // Výchozí hlavička
    [ObservableProperty] private Color statusColor = MyColorHelper.FromHex("#FF3B30");      // Výchozí status
    [ObservableProperty] private Color indexColor = MyColorHelper.FromHex("#FF3B30");       // Výchozí index
    public SolidColorBrush UnitHeadBrush => new(UnitHeadColor);
    public SolidColorBrush StatusBrush => new(StatusColor);
    public SolidColorBrush IndexBrush => new(IndexColor);

    [ObservableProperty]
    private bool isContentVisible = true;// true  false

    [ObservableProperty]
    private bool isMenuVisible = false;

    [ObservableProperty]
    private bool isOeeVisible = false;

    [ObservableProperty]
    private int activeSectionIndex = 0;


    #endregion

    //-----------------------------------------------------------
    #region Observable Properties – Layout config
    //-----------------------------------------------------------

    [ObservableProperty] private Thickness footerMargin = new(10, 0, 8, 3);
    [ObservableProperty] private Thickness headingPadding = new(4, 4, 16, 4);
    [ObservableProperty] private double headerWidth = 228;

    [ObservableProperty] private double layoutCardWidth = 236;
    [ObservableProperty] private double layoutCardHeight = 340;

    [ObservableProperty] private Thickness layoutMarginCardContent = new(4);
    [ObservableProperty] private Thickness layoutPaddingCardText = new(8, 0, 0, 0);

    [ObservableProperty] private double layoutPopupWidth = 236;
    [ObservableProperty] private Thickness layoutPopupContentPadding = new(4);
    [ObservableProperty] private Thickness layoutPersonFooterMargin = new(10, 0, 8, 3);
    [ObservableProperty] private Thickness layoutRightValueMargin = new(0, 0, 20, 0);

    #endregion

    //-----------------------------------------------------------
    #region Observable Properties – Localization
    //-----------------------------------------------------------

    [ObservableProperty] private string textOrderNumber = "Prod.Auftrag:";
    [ObservableProperty] private string textArticle = "Artikel:";
    [ObservableProperty] private string textHaeringCharge = "Häringcharge:";
    [ObservableProperty] private string textMaterialBatch = "Materialcharge:";
    [ObservableProperty] private string textPieceCounter = "StückZähler:";

    [ObservableProperty] private string textStateRunning = "Läuft";
    [ObservableProperty] private string textStateFree = "Frei";
    [ObservableProperty] private string textStateStopped = "Gestoppt";
    [ObservableProperty] private string textStateSetup = "Einstellung";
    [ObservableProperty] private string textStateUnknown = "Unbekannt";
    [ObservableProperty] private string textUnknown = "Unbekannt";

    #endregion

    //-----------------------------------------------------------
    #region Constructor
    //-----------------------------------------------------------

    public CapacityPositionCardVM(
        CapacityUnitM unit,
        CapacityPositionM position,
        IDataService dataService,
        INavigationService navigationService,
        INotificationService notificationService,
        IUserContextService userContext)
        : base(navigationService, notificationService, userContext)
    {
        m_unit = unit;
        m_position = position;
        m_dataService = dataService;

        ContentSwitcherVM = new CardContentSwitcherVM(this); // 🆕 zde je ViewModel pro přepínač obsahu
        //TODO vyzkoušet var v=LocalState

    }

    #endregion

    //-----------------------------------------------------------
    #region Lifecycle
    //-----------------------------------------------------------

    public override async Task InitializeAsync()
    {
        var items = await m_dataService.GetCardMenuItemsAsync();
        MenuItems = new ObservableCollection<MenuItemM>(items);
        ContentSwitcherVM.MenuItems = MenuItems;

        UnitCode = m_position.Code;
        UnitId = m_unit.UnitId;
        Index = m_position.Index.ToString();
        Type = m_position.ProductionType ?? TextUnknown;
        Status = MapStateText(m_position.LocalState);
        LocalState = m_position.LocalState;
        Counter = m_position.Counter;

        // Načíst barvy z modelu (fallback na logiku podle stavu pokud není hex)
        UnitHeadColor = ParseColorOrDefault(m_unit.UnitColorHex, Colors.Green); // Výchozí zelená
        StatusColor = ParseColorOrDefault(m_position.StatusColorHex, m_position.LocalState.GetStatusBrush().Color);
        IndexColor = ParseColorOrDefault(m_position.IndexColorHex, m_position.LocalState.GetIndexBrush().Color);

        MainInfo = new ObservableCollection<InfoItem>
            {
                new(TextOrderNumber, m_position.OrderNumber ?? "00000"),
                new(TextArticle, m_position.ArticleCode ?? "N/A"),
                new(TextHaeringCharge, m_position.HaeringCharge ?? "N/A"),
                new(TextMaterialBatch, m_position.MaterialBatch ?? "N/A"),
            };

        PersonPoints = new ObservableCollection<UserM>(m_position.LoggedInPersons ?? []);
        UpdateVisiblePersonPoints();


        Console.WriteLine("[CapacityPositionCardVM] InitializeAsync");
        await Task.CompletedTask;
    }

    #endregion

    //-----------------------------------------------------------
    #region Commands
    //-----------------------------------------------------------

    [RelayCommand]
    private void SelectItem(MenuItemM item)
    {
        Console.WriteLine($"Card Menu item button -> navigace do {item.Text}");
    }

    public void ShowProduction() => ActiveSection = SectionType.Production;
    public void ShowMenu() => ActiveSection = SectionType.Menu;
    public void ShowOee() => ActiveSection = SectionType.OEE;

    /*
    [RelayCommand]
    public void ToggleSections()
    {
        //ContentSwitcherVM.NextSection();
        OnPropertyChanged(nameof(ActiveSection));
    }
    */
    [RelayCommand]
    private void ToggleSections()
    {
        Console.WriteLine("[CapacityPositionCardVM] ToggleSections");
        if (_toggleLock) return;
        _toggleLock = true;

        IsContentVisible = !IsContentVisible;
        IsMenuVisible = !IsMenuVisible;

        ActiveSectionName = IsContentVisible ? "Production" : "Menu";




        Console.WriteLine($"[Toggle] Active card section: {ActiveSectionName}");


        if (IsContentVisible) { ShowMenu(); Console.WriteLine("Card Content => ShowMenu()"); }
        else if (IsMenuVisible) { ShowProduction(); Console.WriteLine("Card Content => ShowProduction()"); }
        else if (IsOeeVisible)
        {
            Console.WriteLine("Card Content => ShowOee");
            ShowOee();
        }






        Task.Delay(250).ContinueWith(_ => _toggleLock = false, TaskScheduler.FromCurrentSynchronizationContext());
    }



    [RelayCommand]
    private void ShowCondensed() => UpdateVisiblePersonPoints();

    [RelayCommand]
    private void ToggleOperatorPopup()
    {
        OperatorPopupVM.PopupContent.Clear(); // ⬅ důležité
        Console.WriteLine($"ToggleOperatorPopup() called, {PersonPoints.Count} persons.");

        foreach (var p in PersonPoints)
        {
            OperatorPopupVM.PopupContent.Add(new PopupDataItem
            {
                Key = p.FullName,
                Value = p.Id?.ToString() ?? "—"

            });
        }

        OperatorPopupVM.IsOpen = true;
    }



    //TODO: navigace do Dashboard
    public void OnCardTapped(object sender, Microsoft.UI.Xaml.Input.TappedRoutedEventArgs e)
    {
        Console.WriteLine("Card button -> navigace do Dashboard");
        // NavigationService.NavigateTo("Dashboard", m_unit.UnitId);
    }



    #endregion

    //-----------------------------------------------------------
    #region Change Tracking
    //-----------------------------------------------------------

    partial void OnPersonPointsChanged(ObservableCollection<UserM> value)
    {
        UpdateVisiblePersonPoints();
        OnPropertyChanged(nameof(HasMoreUsers));
    }

    #endregion

    //-----------------------------------------------------------
    #region Helpers
    //-----------------------------------------------------------
    private static Color ParseColorOrDefault(string? hex, Color fallback)
    {
        if (string.IsNullOrWhiteSpace(hex))
            return fallback;

        try
        {
            return MyColorHelper.FromHex(hex); // Vždy použij univerzální helper
        }
        catch
        {
            return fallback;
        }
    }
    public static Color FromHex(string hex)
    {
        hex = hex.TrimStart('#');

        if (hex.Length == 6)
        {
            // RRGGBB
            return Color.FromArgb(255,
                Convert.ToByte(hex.Substring(0, 2), 16),
                Convert.ToByte(hex.Substring(2, 2), 16),
                Convert.ToByte(hex.Substring(4, 2), 16));
        }
        else if (hex.Length == 8)
        {
            // AARRGGBB
            return Color.FromArgb(
                Convert.ToByte(hex.Substring(0, 2), 16),
                Convert.ToByte(hex.Substring(2, 2), 16),
                Convert.ToByte(hex.Substring(4, 2), 16),
                Convert.ToByte(hex.Substring(6, 2), 16));
        }
        throw new ArgumentException("Hex barva není ve správném formátu: " + hex);
    }




    private void UpdateVisiblePersonPoints()
    {
        VisiblePerson = PersonPoints.FirstOrDefault();
        OnPropertyChanged(nameof(HasAdditionalPersons));
        OnPropertyChanged(nameof(AdditionalPersons));
    }

    private string MapStateText(CapacityState state) => state switch
    {
        CapacityState.Running => TextStateRunning,
        CapacityState.Off => TextStateFree,
        CapacityState.Stopped => TextStateStopped,
        CapacityState.Setup => TextStateSetup,
        _ => TextStateUnknown
    };


    #endregion

    [ObservableProperty]
    private PopupDisplayControlVM operatorPopupVM = new()
    {
        PopupTitle = "Zugewiesene Betreiber",
        PopupTitleWeight = Microsoft.UI.Text.FontWeights.SemiBold
    };



}
