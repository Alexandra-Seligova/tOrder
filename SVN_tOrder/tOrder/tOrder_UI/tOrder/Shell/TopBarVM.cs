//===================================================================
// $Workfile:: TopBarVM.cs                                         $
// $Author:: Alexandra_Seligova                                    $
// $Revision:: 6                                                   $
// $Date:: 2025-07-25 00:44:00 +0200 (pá, 25 čvc 2025)             $
//===================================================================
// Description:  - tOrder
//     ViewModel for TopBar – header, clock, notifications, alerts
//===================================================================

namespace tOrder.Shell;

#region Using directives
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;

using tOrder.Common;
#endregion

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

    private readonly Timer         m_timer;
    private readonly IDataService  m_dataService;

    #endregion

    //-----------------------------------------------------------
    #region Observable Properties
    //-----------------------------------------------------------

    [ObservableProperty] private ObservableCollection<string> breadCrumpItems = new() { "Prod.Aufträge" };
    public string BreadCrumpText => string.Join(" > ", BreadCrumpItems ?? new());

    [ObservableProperty] private string heading = string.Empty;
    [ObservableProperty] private string currentTime = string.Empty;
    [ObservableProperty] private string currentDate = string.Empty;
    [ObservableProperty] private NotificationType notificationStatus = NotificationType.None;
    [ObservableProperty] private ObservableCollection<string> notificationHistory = new();
    [ObservableProperty] private AlertType alertStatus;
    [ObservableProperty] private string userName = string.Empty;
    [ObservableProperty] private string userSurname = string.Empty;
    [ObservableProperty] private string sessionDuration = string.Empty;

    #endregion

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

        BreadCrumpItems.CollectionChanged += (_, _) =>
        {
            OnPropertyChanged(nameof(BreadCrumpText));
        };
    }

    #endregion

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

    #endregion

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
            Debug.WriteLine($"[DEBUG] Time update failed: {ex.Message}");
#endif
            CurrentTime = "--:--";
            CurrentDate = "??.??.????";
        }
    }

    public async Task RefreshNotificationAsync()
    {
        // TODO: implement actual notification polling
        // NotificationStatus     = await m_dataService.GetCurrentNotificationStatusAsync();
        // NotificationHistory    = new ObservableCollection<string>(await m_dataService.GetNotificationHistoryAsync());
        // AlertStatus            = AlertService.GetLatestAlertLevel();
        await Task.CompletedTask;
    }

    #endregion
}
