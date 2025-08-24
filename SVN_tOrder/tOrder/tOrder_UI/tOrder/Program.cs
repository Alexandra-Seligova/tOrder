//===================================================================
// $Workfile:: Program.cs                                           $
// $Author:: Alexandra_Seligova                                     $
// $Revision:: 5                                                    $
// $Date:: 2025-07-25 02:10:00 +0200 (pá, 25 čvc 2025)              $
//===================================================================
// Description: SPC - tOrder
//     Application Entry Point: WinUI 3 + Dependency Injection
//===================================================================

namespace tOrder;

//-----------------------------------------------------------
// Using directives
//-----------------------------------------------------------

#region Using directives

using System;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;

using Windows.Graphics;

using tOrder.Common;
using tOrder.Shell;
using tOrder.UI;

#endregion // Using directives

//===================================================================
// class tOrderConfig
//===================================================================

/// <summary>
/// Global configuration constants for tOrder application startup,
/// window sizing, console and debug setup.
/// </summary>
public static class tOrderConfig
{
    //-----------------------------------------------------------
    #region Debugging & Console
    //-----------------------------------------------------------

    /// <summary>Enables debug console window at startup.</summary>
    public const bool EnableConsole = true;

    /// <summary>Width of the attached debug console in characters.</summary>
    public const int ConsoleWidth = 100;

    /// <summary>Height of the attached debug console in lines.</summary>
    public const int ConsoleHeight = 30;

    /// <summary>Returns console screen position relative to application window.</summary>
    public static (int X, int Y, int Width, int Height) GetConsolePosition(int appWindowX, int appWindowY)
    {
        return (appWindowX - 800, appWindowY, 800, WindowHeight);
    }

    #endregion // Debugging & Console

    //-----------------------------------------------------------
    #region Window Settings
    //-----------------------------------------------------------

    /// <summary>Default width of the application window (in pixels).</summary>
    public const int WindowWidth = 1280;

    /// <summary>Default height of the application window (in pixels).</summary>
    public const int WindowHeight = 960;

    /// <summary>If true, window will be centered on primary screen at launch.</summary>
    public const bool CenterOnScreen = true;

    #endregion // Window Settings
}

//===================================================================
// class Program
//===================================================================

/// <summary>Application entry and host/service initialization.</summary>
public static class Program
{
    //-----------------------------------------------------------
    #region Fields
    //-----------------------------------------------------------

    public static AppWindow MainAppWindow { get; set; } // Reference to the main AppWindow instance
    public static string CurrentPageName { get; set; } = "Neznámá stránka"; // Current page name for logging/debug

    public static IHost AppHost { get; private set; } = null!; // DI Host for the app
    private static IServiceProvider m_services = null!; // Service provider from DI container

    #endregion // Fields

    //-----------------------------------------------------------
    #region Main Entry
    //-----------------------------------------------------------

    /// <summary>Entry point of the application.</summary>
    [STAThread]
    private static void Main(string[] args)
    {
        try
        {
            if (tOrderConfig.EnableConsole && !PackageHelper.IsPackaged)
            {
                NativeMethods.AllocConsole();
                IntPtr hConsole = NativeMethods.GetStdHandle(NativeMethods.STD_OUTPUT_HANDLE);

                NativeMethods.SetConsoleScreenBufferSize(hConsole, new NativeMethods.COORD(
                    (short)tOrderConfig.ConsoleWidth, (short)tOrderConfig.ConsoleHeight));

                IntPtr hWndConsole = NativeMethods.GetConsoleWindow();

                var screen = DisplayArea.GetFromPoint(new PointInt32(0, 0), DisplayAreaFallback.Primary).WorkArea;
                int winX = screen.X + (screen.Width - tOrderConfig.WindowWidth) / 2;
                int winY = screen.Y + (screen.Height - tOrderConfig.WindowHeight) / 2;

                var (x, y, w, h) = tOrderConfig.GetConsolePosition(winX, winY);
                NativeMethods.MoveWindow(hWndConsole, x, y, w, h, true);

                Console.WriteLine("[Console] Console initialized.");
                Console.WriteLine($"[Console] Console window position set to X:{x}, Y:{y}, Width:{w}, Height:{h}");
                Console.WriteLine($"[Window] Actual console window size: Width:{tOrderConfig.WindowWidth}, Height:{tOrderConfig.WindowHeight}");
            }

            AppHost = ConfigureHost(args).Build();
            m_services = AppHost.Services;

            Application.Start(_ =>
            {
                var ctxDispatcher = new DispatcherQueueSynchronizationContext(DispatcherQueue.GetForCurrentThread());
                SynchronizationContext.SetSynchronizationContext(ctxDispatcher);
                m_services.GetRequiredService<App>();
            });
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("[FATAL ERROR] Application failed to start: " + ex);
            throw;
        }
    }

    #endregion // Main Entry

    //-----------------------------------------------------------
    #region Host Configuration
    //-----------------------------------------------------------

    /// <summary>Configures the DI host and service container.</summary>
    private static IHostBuilder ConfigureHost(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureServices((_, services) =>
            {
                RegisterServices(services);
            });
    }

    #endregion // Host Configuration

    //-----------------------------------------------------------
    #region Services Registration
    //-----------------------------------------------------------

    /// <summary>Registers all services, ViewModels and components.</summary>
    public static void RegisterServices(IServiceCollection services)
    {
        services.AddSingleton<IDataService, MockDataService>();
        services.AddSingleton<INotificationService, NotificationService>();
        services.AddSingleton<IUserContextService, UserContextService>();
        services.AddSingleton<IMetricsService, MetricsService>();
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IDialogService, DialogService>();

        services.AddSingleton<ICapacityCardVMFactory, CapacityCardVMFactory>();

        services.AddSingleton<OverviewByIPCVM>();
        services.AddSingleton<MainWindowVM>();
        services.AddSingleton<TopBarVM>();
        services.AddSingleton<MainLayoutVM>();
        services.AddTransient<CardContentSwitcherVM>();
        services.AddTransient<CapacityPositionCardVM>();
        services.AddTransient<PopupDisplayControlVM>();

        services.AddSingleton<LayoutConfigModel>();
        services.AddSingleton<LayoutConfigVM>();

        services.AddTransient<ResolutionWindow>();

        services.AddSingleton<MainWindow>();
        services.AddSingleton<App>();

        if (tOrderConfig.EnableConsole && !PackageHelper.IsPackaged)
        {
            ConsoleManager.Init(width: 80, bufferHeight: 1000, windowHeight: 30);
            ConsoleManager.SetFont("Consolas", 18); // Optional font override
        }
    }

    #endregion // Services Registration
}


//===================================================================
// class NativeMethods
//===================================================================

internal static class NativeMethods
{
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool AllocConsole(); // Allocates a new console for the calling process.

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool SetConsoleScreenBufferSize(IntPtr hConsoleOutput, COORD size); // Sets console buffer size.

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr GetStdHandle(int nStdHandle); // Retrieves a handle to the standard device.

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint); // Moves or resizes a window.

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr GetConsoleWindow(); // Gets handle to the console window.

    public const int STD_OUTPUT_HANDLE = -11; // Standard output handle constant.

    [StructLayout(LayoutKind.Sequential)]
    public struct COORD // Represents X/Y coordinates in the console buffer.
    {
        public short X;
        public short Y;

        public COORD(short x, short y)
        {
            X = x;
            Y = y;
        }
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public unsafe struct CONSOLE_FONT_INFO_EX // Holds extended font info for console text rendering.
    {
        public uint cbSize;
        public uint nFont;
        public COORD dwFontSize;
        public int FontFamily;
        public int FontWeight;
        fixed char FaceName[32];

        public string FaceNameString
        {
            get
            {
                fixed (char* p = FaceName)
                    return new string(p).TrimEnd('\0');
            }
            set
            {
                fixed (char* p = FaceName)
                {
                    for (int i = 0; i < 32; i++)
                        p[i] = i < value.Length ? value[i] : '\0';
                }
            }
        }
    }

    [DllImport("kernel32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern bool SetCurrentConsoleFontEx(
        IntPtr consoleOutput,
        bool maximumWindow,
        ref CONSOLE_FONT_INFO_EX consoleCurrentFontEx); // Sets extended font attributes for console text.
}

//===================================================================
// class PackageHelper
//===================================================================

public static class PackageHelper
{
    public static bool IsPackaged // Returns true if the app is running as MSIX-packaged.
    {
        get
        {
            try
            {
                var _ = Windows.ApplicationModel.Package.Current;
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
//===================================================================
// 📘 Overview: Program.cs – Application Bootstrap & Configuration
//===================================================================
//
// This file contains the main bootstrap logic for the tOrder
// application. It handles everything necessary to launch and
// configure the app via WinUI 3 and Microsoft.Extensions.Hosting.
//
// 🧩 Responsibilities:
//
// ▸ Entry Point
//   - `Main()` is the application entry, marked with `[STAThread]`.
//   - Starts DI container, registers services, and launches the app.
//
// ▸ Dependency Injection Setup
//   - Uses `Host.CreateDefaultBuilder()` to register services, view models,
//     windows, and factories used across the app.
//   - `Program.Services` holds the root `IServiceProvider` for use in `App`.
//
// ▸ Console Support (Unpackaged Mode Only)
//   - Allocates and configures a debugging console window if enabled
//     and running outside MSIX.
//   - Position and buffer size is adjusted based on the layout window.
//
// ▸ Native Console Control (NativeMethods)
//   - Interop definitions for Win32 APIs to control window positioning,
//     font, size, and console allocation for debugging.
//
// ▸ Window Configuration
//   - Reads from `tOrderConfig` which defines:
//     ▸ Default window size (1280x960)
//     ▸ Console dimensions (100x30)
//     ▸ Positioning logic for aligning console next to app window
//
// ▸ Packaging Detection (PackageHelper)
//   - Safely detects whether the app runs as MSIX (packaged)
//     or directly (unpackaged) – key for feature toggles.
//
// ⚙️ Extensibility:
//
// - You can add new services by appending to `RegisterServices()`.
// - Supports dynamic window resolution, layout previews, and
//   diagnostic tools integration.
//
// 🧱 Startup Flow Diagram:
//
//   +-----------------+
//   |   Program.cs    | ← Main()
//   +-----------------+
//          │
//   ConfigureHost() → RegisterServices()
//          │
//          ▼
//   +-----------------+
//   |    App.xaml.cs   | ← Application.Start()
//   +-----------------+
//          │
//          ▼
//   +-----------------+
//   |   MainWindow     | ← via DI
//   +-----------------+
//          ▼
//   +-----------------+
//   |   MainLayout     |
//   +-----------------+
//
// This structured boot process ensures modularity, DI compatibility,
// and easy debugging with runtime console output in development.
//
//===================================================================
