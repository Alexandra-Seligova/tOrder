//===================================================================
// $Workfile:: MainLayoutVM.cs                                     $
// $Author:: Alexandra_Seligova                                     $
// $Revision:: 4                                                   $
// $Date:: 2025-06-11 15:44:33 +0200 (st, 11 čvn 2025)            $
//===================================================================
// Description: SPC - tOrder
//     ViewModel for the Layout control – heading, navigation, breadcrumbs
//===================================================================

namespace tOrder.Shell
{
    //-----------------------------------------------------------
    #region Using directives
    //-----------------------------------------------------------

    using System;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Collections.ObjectModel;

    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    using tOrder.Common;
    using tOrder.UI;

    #endregion //Using directives

    //===================================================================
    // class NavigationItemModel
    //===================================================================

    /// <summary>
    /// Model reprezentující jednu položku navigace.
    /// </summary>
    public class NavigationItemModel
    {
        public string IconKind { get; }
        public string Glyph { get; }
        public string Label { get; }
        public string Tag { get; }

        public NavigationItemModel(string iconKind, string glyph, string label, string tag)
        {
            IconKind = iconKind;
            Glyph = glyph;
            Label = label;
            Tag = tag;
        }
    }

    //===================================================================
    // class MainLayoutVM
    //===================================================================

    /// <summary>
    /// ViewModel pro hlavní layout – spravuje titulek, navigaci a breadcrumbs.
    /// </summary>
    public partial class MainLayoutVM : BaseVM
    {
        //-----------------------------------------------------------
        #region Fields
        //-----------------------------------------------------------

        private readonly IDataService _dataService;
        private readonly TopBarVM _topBarViewModel;

        #endregion //Fields

        //-----------------------------------------------------------
        #region Observable Properties
        //-----------------------------------------------------------

        [ObservableProperty]
        private string currentHeader = string.Empty;

        [ObservableProperty]
        private string? selectedPageTag;

        [ObservableProperty]
        private ObservableCollection<NavigationItemModel> menuItems = [];

        [ObservableProperty]
        private ObservableCollection<string> breadcrumbItems = [];

        #endregion //Observable Properties

        //-----------------------------------------------------------
        #region Constructor
        //-----------------------------------------------------------

        public MainLayoutVM(
            INavigationService navigationService,
            INotificationService notificationService,
            IUserContextService userContext,
            IDataService dataService,
            TopBarVM topBarViewModel)
            : base(navigationService, notificationService, userContext)
        {
            _dataService = dataService;
            _topBarViewModel = topBarViewModel;
        }

        #endregion //Constructor

        //-----------------------------------------------------------
        #region Initialization
        //-----------------------------------------------------------

        public override async Task InitializeAsync()
        {
            await LoadNavigationItemsAsync();
        }

        private async Task LoadNavigationItemsAsync()
        {
            try
            {
                var navItems = await _dataService.GetNavigationItemsAsync();

                MenuItems = navItems is null
                    ? []
                    : new ObservableCollection<NavigationItemModel>(navItems);
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine($"[DEBUG] Výjimka při načítání navigace: {ex}");
                Debugger.Break();
#endif
                MenuItems = [];
            }
        }

        #endregion //Initialization

        //-----------------------------------------------------------
        #region Commands
        //-----------------------------------------------------------

        [RelayCommand]
        private void SetHeader(string header)
        {
            if (string.IsNullOrWhiteSpace(header))
            {
#if DEBUG
                Debug.WriteLine("[DEBUG] Pokus o nastavení prázdného titulku.");
                Debugger.Break();
#endif
                return;
            }

            CurrentHeader = header;
            _topBarViewModel.Heading = header;
        }

        [RelayCommand]
        private void AddBreadcrumbItem(string item)
        {
            if (string.IsNullOrWhiteSpace(item))
            {
#if DEBUG
                Debug.WriteLine("[DEBUG] Pokus o přidání prázdné breadcrumb položky.");
                Debugger.Break();
#endif
                return;
            }

            BreadcrumbItems.Add(item);
        }

        #endregion //Commands
    }

    //===================================================================
}

/*
=========================================================================
📘 LayoutViewModel – přehled funkcionality
=========================================================================

Tento ViewModel obsluhuje hlavní layout aplikace, který kombinuje:
- horní záhlaví (např. aktuální sekce, název stránky),
- navigační menu s výběrem stránek (Sidebar),
- správu aktivní stránky (SelectedPageTag).

Zajišťuje:
✔️ Dynamické načítání položek menu (`MenuItems`) ze `IDataService` (mock nebo budoucí API),
✔️ Aktualizaci záhlaví pomocí `CurrentHeader` a příkazu `SetHeaderCommand`,
✔️ Spolupráci s `TopBarViewModel` (např. pro zobrazení zpětné navigace, přihlášeného uživatele),
✔️ Dynamické sestavování breadcrumb cesty (`BreadcrumbItems`), která se zobrazuje v horní liště,
✔️ Respektování MVVM architektury – žádná UI logika, čisté datové řízení.

Třída `NavigationItemModel` je součástí tohoto ViewModelu a používá se výhradně pro účely layout navigace.
*/

/*
=========================================================================
⚠ Omezení LayoutViewModel – s čím záměrně nepočítáme
=========================================================================

✘ Nepodporuje víceúrovňové menu nebo dynamické submenu – pouze jednoduchý seznam.
✘ Nespouští navigaci automaticky – pouze nastavuje vybrané tagy a záhlaví.
✘ Nepodporuje persistenci stavu (např. zapamatování poslední stránky).
✘ Nepodporuje úpravu nebo přidávání položek menu za běhu.
✘ Třída `NavigationItemModel` je lokální – není určena pro sdílení mezi ViewModely nebo službami.
✘ Zatím žádná podpora oprávnění/rolí – všechny položky menu jsou vždy viditelné.
✘ Neřeší zobrazování ikon – to je úkolem View (např. přes `FontIcon.Glyph`).

V případě potřeby bude v budoucnu vytvořen LayoutController nebo NavigationService pro složitější scénáře.
*/
