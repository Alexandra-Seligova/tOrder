//===================================================================
// $Workfile:: Program.cs                                           $
// $Author:: Alexandra_Seligova                                      $
// $Revision:: 4                                                    $
// $Date:: 2025-06-11 15:44:33 +0200 (st, 11 čvn 2025)             $
//===================================================================
// Description: SPC - tOrder
//     Application Entry Point: WinUI 3 + Dependency Injection
//===================================================================

namespace tOrder;

//-----------------------------------------------------------
#region Using directives
//-----------------------------------------------------------

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


#endregion //Using directives

//===================================================================
// class tOrderConfig
//===================================================================

public static class tOrderConfig
{
    //-----------------------------------------------------------
    #region Debugging
    //-----------------------------------------------------------

    public const bool EnableConsole = true;

    #endregion //Debugging

    //-----------------------------------------------------------
    #region Window Size
    //-----------------------------------------------------------
    //16:9 1280 x 720  
    //4:3  1024 x 768
    //public const int WindowWidth = 1280;
    //public const int WindowHeight = 720;
    public const int WindowWidth = 1024;
    public const int WindowHeight = 768;
    public const bool CenterOnScreen = true;

    #endregion //Window Size

    //-----------------------------------------------------------
    #region Console settings
    //-----------------------------------------------------------

    public const int ConsoleWidth = 120;
    public const int ConsoleHeight = 30;

    public static (int X, int Y, int Width, int Height) GetConsolePosition(int appWindowX, int appWindowY)
    {
        return (appWindowX - 800, appWindowY, 800, WindowHeight);
    }

    #endregion //Console settings
}


//===================================================================
// class Program
//===================================================================

public static class Program
{
    //-----------------------------------------------------------
    #region Fields
    //-----------------------------------------------------------

    public static IHost AppHost { get; private set; } = null!;
    private static IServiceProvider m_services = null!;

    #endregion //Fields

    //-----------------------------------------------------------
    #region Main Entry
    //-----------------------------------------------------------

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

    #endregion //Main Entry

    //-----------------------------------------------------------
    #region Host Configuration
    //-----------------------------------------------------------

    private static IHostBuilder ConfigureHost(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureServices((_, services) =>
            {
                RegisterServices(services);
            });
    }

    #endregion //Host Configuration

    //-----------------------------------------------------------
    #region Services Registration
    //-----------------------------------------------------------

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

        services.AddTransient<PopupDisplayControlVM>(); // 🆕

        services.AddSingleton<MainWindow>();
        services.AddSingleton<App>();

        if (tOrderConfig.EnableConsole && !PackageHelper.IsPackaged)
        {
            ConsoleManager.Init(width: 120, bufferHeight: 1000, windowHeight: 30);
            ConsoleManager.SetFont("Consolas", 18); // Volitelné
        }

    }


    #endregion //Services Registration
}




//===================================================================
// class NativeMethods
//===================================================================

internal static class NativeMethods
{
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool AllocConsole();

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern bool SetConsoleScreenBufferSize(IntPtr hConsoleOutput, COORD size);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr GetStdHandle(int nStdHandle);

    [DllImport("user32.dll", SetLastError = true)]
    public static extern bool MoveWindow(IntPtr hWnd, int X, int Y, int nWidth, int nHeight, bool bRepaint);

    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr GetConsoleWindow();

    public const int STD_OUTPUT_HANDLE = -11;

    [StructLayout(LayoutKind.Sequential)]
    public struct COORD
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
    public unsafe struct CONSOLE_FONT_INFO_EX
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
        ref CONSOLE_FONT_INFO_EX consoleCurrentFontEx
    );

}

//===================================================================
// class PackageHelper
//===================================================================

public static class PackageHelper
{
    public static bool IsPackaged
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
