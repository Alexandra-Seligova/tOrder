//===================================================================
// $Workfile:: BaseVM.cs                                           $
// $Author:: Alexandra_Seligova                                     $
// $Revision:: 4                                                   $
// $Date:: 2025-06-11 15:44:33 +0200 (st, 11 čvn 2025)            $
//===================================================================
// Description:  - tOrder
//     Abstract base ViewModel with shared services, guarded lifecycle
//===================================================================

namespace tOrder.Common;

#region Using directives

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

#endregion

public abstract class BaseVM : ObservableObject
{
    #region Services (DI)

    protected IDialogService DialogService { get; }
    protected INavigationService NavigationService { get; }
    protected INotificationService NotificationService { get; }
    protected IUserContextService UserContext { get; }

    #endregion

    #region Lifecycle & State

    protected CancellationTokenSource? LoadCts { get; private set; }

    private bool m_isBusy;
    public bool IsBusy
    {
        get => m_isBusy;
        set
        {
#if DEBUG
            if (value && m_isBusy)
            {
                Debug.WriteLine($"[⚠️ {GetType().Name}] IsBusy was already true! Redundant call?");
                Debugger.Break();
            }
#endif
            SetProperty(ref m_isBusy, value);
        }
    }

    private string? m_errorMessage;
    public string? ErrorMessage
    {
        get => m_errorMessage;
        set => SetProperty(ref m_errorMessage, value);
    }

    public virtual string PageTitle => string.Empty;
    public virtual bool HasErrors => false;

    private bool m_initialized;
    private DateTime m_lastLoadTime = DateTime.MinValue;
    private readonly TimeSpan m_minLoadInterval = TimeSpan.FromSeconds(3);

    #endregion

    #region Constructor

    protected BaseVM(
        INavigationService navigationService,
        INotificationService notificationService,
        IUserContextService userContext)
    {
        NavigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
        NotificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        UserContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
    }

    #endregion

    #region Lifecycle Methods

    public virtual async Task OnLoadedAsync()
    {
        var now = DateTime.Now;
        if (now - m_lastLoadTime < m_minLoadInterval)
        {
#if DEBUG
            Debug.WriteLine($"[⏳ {GetType().Name}] OnLoadedAsync skipped - called too soon.");
#endif
            return;
        }

        m_lastLoadTime = now;

#if DEBUG
        Debug.WriteLine($"[📥 {GetType().Name}] OnLoadedAsync called.");
        Console.WriteLine($"[VM] OnLoadedAsync: {GetType().Name}");
#endif

        if (!m_initialized)
        {
            try
            {
#if DEBUG
                Debug.WriteLine($"[⚙️ {GetType().Name}] Initializing...");
                Console.WriteLine($"[VM] Initializing ViewModel: {GetType().Name}");
#endif
                m_initialized = true;
                await InitializeAsync();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[❌ {GetType().Name}] InitializeAsync failed: {ex}");
                Console.WriteLine($"[VM] InitializeAsync failed: {ex.Message}");
            }
        }

        try
        {
            LoadCts?.Cancel();
            LoadCts = new CancellationTokenSource();
            await LoadDataAsync(LoadCts.Token);
#if DEBUG
            Console.WriteLine($"[VM] LoadDataAsync succeeded: {GetType().Name}");
#endif
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"[❌ {GetType().Name}] LoadDataAsync failed: {ex}");
            Console.WriteLine($"[VM] LoadDataAsync failed: {ex.Message}");
        }
    }

    public virtual Task InitializeAsync() => Task.CompletedTask;
    public virtual Task LoadDataAsync(CancellationToken ct = default) => Task.CompletedTask;

    #endregion

    #region Utility Methods

    protected void RaisePropertyChanged([CallerMemberName] string propertyName = "") =>
        OnPropertyChanged(propertyName);

    protected bool SetPropertyAndNotify<T>(
        ref T backingField, T value,
        [CallerMemberName] string propertyName = "")
    {
        if (Equals(backingField, value)) return false;
        backingField = value;
        OnPropertyChanged(propertyName);
        return true;
    }

    protected void LogEvent(string name, Dictionary<string, object>? props = null)
    {
        App.GetService<IMetricsService>()?.TrackEvent(name, props);
    }

    #endregion
}




/*
| Prvek                  | Vysvětlení |
| ---------------------- | --------------------------------------------------------------------------------------------- |
| `OnLoadedAsync()`      | Voláno z `View` (např. v `Loaded` eventu) – slouží jako životní cyklus pro vstup ViewModelu.  |
| `InitializeAsync()`    | Čistá inicializace (nastavení proměnných, ale bez volání API).                                |
| `LoadDataAsync()`      | Načítání dat (ze servis, přes DI).                                                            |
| `IsBusy`               | Pomáhá ve View s indikací načítání nebo blokací UI.                                           |
| `ErrorMessage`         | Umožňuje View reagovat na chybu (např. zobrazit toast nebo error control).                    |
| `RaisePropertyChanged` | Pro ruční oznámení změny – např. při změně jiné kolekce nebo komplexní logice.                |
| `SetPropertyAndNotify` | Varianta `SetProperty` s vlastní logikou (kdyby bylo potřeba sledovat změny více explicitně). |
| `NotificationService`  | Pro toast/snackbar zprávy – např. potvrzení, chyby, hlášky.                                   |
| `UserContext`          | Přístup k údajům o uživateli – jméno, role, autentizace.                                      |

*/

/*
=========================================================================
📌 Omezení a vědomě neošetřené situace v BaseViewModel
=========================================================================

1. ❌ Nepočítáme s přímou podporou `INotifyDataErrorInfo` nebo validací polí
   - Validace je specifická pro formuláře, ne pro každý ViewModel.
   - Doporučeno dědit z `ValidatableViewModel : BaseViewModel` tam, kde je to potřeba.

2. ❌ Neobsahuje `IMetricsService` jako injektovanou závislost
   - Služba je volaná staticky přes `App.GetService<>()`, protože není potřeba ve všech ViewModelech.
   - Vyhýbáme se „nafouknutí“ konstruktoru základní třídy.

3. ❌ Neřeší synchronizaci s UI vláknem (`DispatcherQueue`)
   - Očekává se, že to provádí volající ViewModel nebo servis.
   - Užitečné pro zjednodušení, ale může být doplněno dle potřeby.

4. ❌ Nepodporuje uložení/zotavení stavu (např. při obnovení aplikace)
   - Uložení a obnova ViewModelu je aplikačně specifická a měla by být řešena externě.

5. ❌ Nepodporuje přímo lokální cache/úložiště
   - Přístup ke cache (např. přes `ICacheService`) je specifický pro daný modul.

6. ❌ Neřeší přístup k překladu (i18n)
   - Lokalizace probíhá typicky přes resource binding nebo specializovanou službu mimo ViewModel.

7. ❌ Nepodporuje automatické aktualizace (polling, websocket, MQTT)
   - Takové mechanizmy patří buď do specializovaného `DataService`, nebo do `Controller` vrstvy.

8. ❌ Neřeší víceúrovňovou navigaci (nested navigation stack)
   - Předpokládáme plochý navigační rámec, kde `INavigationService` pracuje s jedním `Frame`.

=========================================================================
*/

