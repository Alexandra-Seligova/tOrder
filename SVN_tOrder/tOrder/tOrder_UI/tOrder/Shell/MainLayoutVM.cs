//===================================================================
// $Workfile:: MainLayoutVM.cs                                     $
// $Author:: Alexandra_Seligova                                    $
// $Revision:: 5                                                   $
// $Date:: 2025-07-24 23:32:00 +0200 (čt, 24 čvc 2025)             $
//===================================================================
// Description:  - tOrder
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

    #endregion // Using directives

    //===================================================================
    // class NavigationItemModel
    //===================================================================

    /// <summary>
    /// Represents a single item in the main navigation menu.
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
    /// ViewModel for the main layout control – responsible for managing
    /// current header, navigation menu items, and breadcrumb path.
    /// </summary>
    public partial class MainLayoutVM : BaseVM
    {
        //-----------------------------------------------------------
        #region Fields
        //-----------------------------------------------------------

        private readonly IDataService _dataService;
        private readonly TopBarVM _topBarViewModel;

        #endregion // Fields

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

        #endregion // Observable Properties

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

        #endregion // Constructor

        //-----------------------------------------------------------
        #region Initialization
        //-----------------------------------------------------------

        /// <summary>
        /// Called during application startup or reactivation.
        /// Loads initial navigation menu.
        /// </summary>
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
                Debug.WriteLine($"[DEBUG] Exception while loading navigation: {ex}");
                Debugger.Break();
#endif
                MenuItems = [];
            }
        }

        #endregion // Initialization

        //-----------------------------------------------------------
        #region Commands
        //-----------------------------------------------------------

        /// <summary>
        /// Sets the current heading label and updates TopBar VM.
        /// </summary>
        [RelayCommand]
        private void SetHeader(string header)
        {
            if (string.IsNullOrWhiteSpace(header))
            {
#if DEBUG
                Debug.WriteLine("[DEBUG] Attempted to set an empty header.");
                Debugger.Break();
#endif
                return;
            }

            CurrentHeader = header;
            _topBarViewModel.Heading = header;
        }

        /// <summary>
        /// Adds an item to the breadcrumb navigation list.
        /// </summary>
        [RelayCommand]
        private void AddBreadcrumbItem(string item)
        {
            if (string.IsNullOrWhiteSpace(item))
            {
#if DEBUG
                Debug.WriteLine("[DEBUG] Attempted to add an empty breadcrumb item.");
                Debugger.Break();
#endif
                return;
            }

            BreadcrumbItems.Add(item);
        }

        #endregion // Commands

        //-----------------------------------------------------------
        #region Layout Configuration (🧪 DEMO)
        //-----------------------------------------------------------

        /// <summary>
        /// Clears the current header and breadcrumb list.
        /// Useful when resetting layout state on logout or navigation reset.
        /// </summary>
        [RelayCommand]
        private void ResetLayout()
        {
            CurrentHeader = string.Empty;
            BreadcrumbItems.Clear();
            SelectedPageTag = null;
        }

        /// <summary>
        /// Sets both the selected navigation tag and updates the header.
        /// Used when synchronizing view state from external events.
        /// </summary>
        [RelayCommand]
        private void UpdateLayoutContext((string pageTag, string header) context)
        {
            SelectedPageTag = context.pageTag;
            SetHeader(context.header);
        }

        /// <summary>
        /// Sets predefined breadcrumb structure (e.g. after deep navigation).
        /// </summary>
        [RelayCommand]
        private void SetBreadcrumbs(string[] segments)
        {
            BreadcrumbItems.Clear();
            foreach (var segment in segments.Where(s => !string.IsNullOrWhiteSpace(s)))
            {
                BreadcrumbItems.Add(segment);
            }
        }

        #endregion // Layout Configuration (🧪 DEMO)

    }

    //===================================================================
}
/*
===============================================================================
🧱 MainLayout – Visual Layout Shell (Navigation + Header + Content)
===============================================================================

MainLayout represents the structural container for all core UI components
within the tOrder application. It defines the persistent layout and includes:

- 🔹 Left navigation panel (NavigationView)
- 🔹 Top header bar (TopBar)
- 🔹 Central content frame (Frame) used for dynamic page injection
- 🔹 Optional alert/notification areas

This layout is state-driven via MainLayoutVM, which handles:

- Current header text (shown in TopBar)
- Selected navigation item (Tag-based navigation)
- Breadcrumb trail (observable list of string segments)
- Initial population of menu items (from IDataService)

The ViewModel integrates with shared services like navigation, notifications,
and user context to provide reactive state for all layout-aware components.

MainLayout does **not** contain business logic; it is solely responsible for
presentation-level consistency and shell hosting.

===============================================================================
*/


