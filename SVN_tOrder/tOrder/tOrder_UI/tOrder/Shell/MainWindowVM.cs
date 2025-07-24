//===================================================================
// $Workfile:: MainWindowVM.cs                                     $
// $Author:: Alexandra_Seligova                                    $
// $Revision:: 5                                                   $
// $Date:: 2025-07-24 23:22:00 +0200 (čt, 24 čvc 2025)             $
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

    #endregion // Using directives

    //===================================================================
    // class MainWindowVM
    //===================================================================

    /// <summary>
    /// ViewModel for the main window, managing top-level UI interactions such as:
    /// - page navigation breadcrumbs
    /// - contextual UI commands
    /// - global window state flags
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
            // Initialize commands here if needed
        }

        #endregion // Constructor

        //-----------------------------------------------------------
        #region Commands & Properties
        //-----------------------------------------------------------

        // 🔖 Breadcrumb trail for current page
        [ObservableProperty]
        private ObservableCollection<string> breadcrumbs = new();

        // 🧭 Back navigation command (optional binding from UI)
        [RelayCommand]
        private void GoBack()
        {
            NavigationService.GoBack();
        }

        // ℹ️ Page title (optional – can be bound to TopBar)
        [ObservableProperty]
        private string pageTitle = string.Empty;

        // 🪟 Window state flags (can be bound for adaptive layout)
        [ObservableProperty]
        private bool isBackButtonVisible;

        #endregion // Commands & Properties
    }

    //===================================================================
}

/*
===============================================================================
🪟 MainWindow – UI Shell for the tOrder Application
===============================================================================

MainWindow acts as the visual entry point of the UI. It hosts the root layout
(MainLayout) and serves as a container for all navigation and application-level
UI structures.

Responsibilities:
- Initializes root visual layout (NavigationView, TopBar, AlertPanel)
- Hosts the main content frame (`Frame`) used for page navigation
- Captures global inputs (keyboard shortcuts, authentication, etc.)
- Binds to `MainWindowVM` to expose shared UI states and commands
  (e.g. breadcrumbs, current page title, UI flags)

MainWindow does not handle domain logic directly – that is delegated to
subordinate ViewModels. It focuses on layout and shell hosting logic.

MainLayout includes:
- Left navigation menu (NavigationView)
- Top header bar (TopBar)
- Page container (Frame)
- Alert display area (global notifications)

This structure centralizes navigation, UI state handling, and service access
in a clean and testable MVVM setup.

===============================================================================
*/
