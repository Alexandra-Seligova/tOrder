//===================================================================
// $Workfile:: MainWindowVM.cs                                     $
// $Author:: Alexandra_Seligova                                     $
// $Revision:: 4                                                   $
// $Date:: 2025-06-11 15:44:33 +0200 (st, 11 čvn 2025)            $
//===================================================================
// Description: SPC - tOrder
//     ViewModel for MainWindow – breadcrumb handling & UI commands
//===================================================================

namespace tOrder.Shell
{
    //-----------------------------------------------------------
    #region Using directives
    //-----------------------------------------------------------

    using System.Collections.ObjectModel;
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;

    using tOrder.Common;

    #endregion //Using directives

    //===================================================================
    // class MainWindowVM
    //===================================================================

    /// <summary>
    /// ViewModel for the main window, managing breadcrumb navigation and UI commands.
    /// </summary>
    public partial class MainWindowVM : BaseVM
    {
        //-----------------------------------------------------------
        #region Constructor
        //-----------------------------------------------------------

        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindowVM"/> class.
        /// </summary>
        public MainWindowVM(
            INavigationService navigationService,
            INotificationService notificationService,
            IUserContextService userContext)
            : base(navigationService, notificationService, userContext)
        {
        }

        #endregion //Constructor

        //-----------------------------------------------------------
        #region Commands & Properties
        //-----------------------------------------------------------

        // Sem přijdou další observable properties, commands apod.

        #endregion //Commands & Properties
    }

    //===================================================================
}

/*
=========================================================================
🪟 MainWindow – hlavní okno aplikace tOrder
=========================================================================

MainWindow slouží jako vstupní bod uživatelského rozhraní a hostuje základní 
layout celé aplikace. Je to **vizuální kontejner** pro všechny ostatní komponenty.

Zajišťuje:
- inicializaci základního UI prostředí (např. Layout, TopBar, AlertPanel),
- držení navigačního rámce (`Frame`) pro načítání jednotlivých stránek (Pages),
- globální vstupy od uživatele (např. klávesové zkratky, přihlášení),
- propojení View s `MainWindowViewModel`, který spravuje breadcrumb cestu,
  stránkový titul a případné další stavy hlavního okna.

MainWindow sám o sobě **neřeší doménovou logiku** – tu předává do podřízených ViewModelů,
ale definuje hlavní rozvržení a hostuje v sobě `Layout.xaml`, který kombinuje:
- navigační menu vlevo (NavigationView),
- horní lištu (TopBar),
- hlavní obsah stránky (Frame/Page),
- alert panel (výstražná oznámení)

Díky této struktuře je možné z jednoho místa řídit navigaci, přístup k UI stavům
a správu globálních služeb (např. oznámení, dialogy).

=========================================================================

*/
