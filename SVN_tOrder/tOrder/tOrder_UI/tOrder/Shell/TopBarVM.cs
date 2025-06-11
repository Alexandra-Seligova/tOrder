//===================================================================
// $Workfile:: TopBarVM.cs                                         $
// $Author:: Alexandra_Seligova                                     $
// $Revision:: 4                                                   $
// $Date:: 2025-06-11 15:44:33 +0200 (st, 11 čvn 2025)            $
//===================================================================
// Description: SPC - tOrder
//     ViewModel for TopBar – header, clock, notifications, alerts
//===================================================================

namespace tOrder.Shell
{
    //-----------------------------------------------------------
    #region Using directives
    //-----------------------------------------------------------

    using System;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.Threading.Tasks;
    using System.Timers;

    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;
    using CommunityToolkit.Mvvm.Messaging;

    using tOrder.Common;
    #endregion //Using directives

    //===================================================================
    // class TopBarVM
    //===================================================================

    /// <summary>
    /// ViewModel managing the data for the TopBar control.
    /// </summary>
    public partial class TopBarVM : BaseVM
    {
        //-----------------------------------------------------------
        #region Fields
        //-----------------------------------------------------------

        private readonly Timer m_timer;
        private readonly IDataService m_dataService;

        #endregion //Fields

        //-----------------------------------------------------------
        #region Observable Properties
        //-----------------------------------------------------------

        [ObservableProperty]
        private ObservableCollection<string> breadCrumpItems = new() { "Prod.Aufträge" };

        // BreadCrumpText generované z kolekce
        public string BreadCrumpText => string.Join(" > ", BreadCrumpItems ?? new());




        private string breadCrumpText = "";

        [ObservableProperty]
        private string heading = string.Empty;

        [ObservableProperty]
        private string currentTime = string.Empty;

        [ObservableProperty]
        private string currentDate = string.Empty;

        [ObservableProperty]
        private NotificationType notificationStatus = NotificationType.None;

        [ObservableProperty]
        private ObservableCollection<string> notificationHistory = new();

        [ObservableProperty]
        private AlertType alertStatus;

        [ObservableProperty]
        private string userName = string.Empty;

        [ObservableProperty]
        private string userSurname = string.Empty;

        [ObservableProperty]
        private string sessionDuration = string.Empty;

        #endregion //Observable Properties

        //-----------------------------------------------------------
        #region Commands
        //-----------------------------------------------------------

        [RelayCommand]
        private void ToggleMenu()
        {
            WeakReferenceMessenger.Default.Send(new UiMessage("ToggleMenu"));
        }

        #endregion

        //-----------------------------------------------------------
        #region Constructor
        //-----------------------------------------------------------

        public TopBarVM(
            INavigationService navigationService,
            INotificationService notificationService,
            IUserContextService userContext,
            IDataService dataService)
            : base(navigationService, notificationService, userContext)
        {
            m_dataService = dataService;

            m_timer = new Timer(1000);
            m_timer.Elapsed += OnTimerElapsed;
            m_timer.Start();

            UpdateTimeAndDate();

            BreadCrumpItems.CollectionChanged += (s, e) => OnPropertyChanged(nameof(BreadCrumpText));
        }

        #endregion //Constructor

        //-----------------------------------------------------------
        #region Timer Events
        //-----------------------------------------------------------

        private async void OnTimerElapsed(object? sender, ElapsedEventArgs e)
        {
            try
            {
                UpdateTimeAndDate();
                await RefreshNotificationAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[TopBar Timer Error] {ex.Message}");
            }
        }

        #endregion //Timer Events

        //-----------------------------------------------------------
        #region Helpers
        //-----------------------------------------------------------

        private void UpdateTimeAndDate()
        {
            try
            {
                var now = DateTime.Now;

                CurrentTime = now.ToShortTimeString();
                CurrentDate = now.ToShortDateString();
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine($"[DEBUG] Chyba při aktualizaci času a data: {ex.Message}");
#endif
                CurrentTime = "--:--";
                CurrentDate = "??.??.????";
            }
        }

        public async Task RefreshNotificationAsync()
        {
            // NotificationStatus = await m_dataService.GetCurrentNotificationStatusAsync();

            // var history = await m_dataService.GetNotificationHistoryAsync();
            // NotificationHistory = new ObservableCollection<string>(history);

            //AlertStatus = AlertService.GetLatestAlertLevel();
        }

        #endregion //Helpers
    }

    //===================================================================
}
