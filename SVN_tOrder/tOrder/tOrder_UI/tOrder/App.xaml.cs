//===================================================================
// $Workfile:: App.xaml.cs                                          $
// $Author:: Alexandra_Seligova                                      $
// $Revision:: 4                                                    $
// $Date:: 2025-06-11 15:44:33 +0200 (st, 11 čvn 2025)             $
//===================================================================
// Description: SPC - tOrder
//     Basic Application wrapper without DI or navigation layout
//===================================================================

namespace tOrder;

//-----------------------------------------------------------
#region Using directives
//-----------------------------------------------------------

using System;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

using WinRT.Interop;

using tOrder.Shell;
using Windows.Graphics;
using Microsoft.UI.Dispatching;

#endregion //Using directives

//===================================================================
// class App
//===================================================================

public partial class App : Application
{
    //-----------------------------------------------------------
    #region Fields & Properties
    //-----------------------------------------------------------

    public static IServiceProvider Services { get; private set; } = null!;
    public static Window MainAppWindow { get; private set; } = null!; // Statická reference na hlavní okno

    private Window m_mainWindow = null!;

    #endregion //Fields & Properties

    //-----------------------------------------------------------
    #region Constructor
    //-----------------------------------------------------------

    public App()
    {
        Services = Program.AppHost.Services;

        this.InitializeComponent();
        this.RequestedTheme = ApplicationTheme.Light;

        bool bIsPackaged = AppEnvironment.IsPackaged;
        Console.WriteLine($"[App] Packaging Mode: {(bIsPackaged ? "Packaged" : "Unpackaged")}");

        var screen = DisplayArea.GetFromPoint(new Windows.Graphics.PointInt32(0, 0), DisplayAreaFallback.Primary).WorkArea;
        Console.WriteLine($"[Win] Screen Resolution: {screen.Width}x{screen.Height} at center");
    }

    #endregion //Constructor

    //-----------------------------------------------------------
    #region Application Launch
    //-----------------------------------------------------------

    // App.xaml.cs - upravená metoda OnLaunched
    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        try
        {
            if (m_mainWindow == null)
            {
                m_mainWindow = Services.GetRequiredService<MainWindow>();
                MainAppWindow = m_mainWindow;
            }

            if (m_mainWindow.Content == null)
            {
                var frameRoot = new Frame();
                frameRoot.Navigate(typeof(MainLayout));
                m_mainWindow.Content = frameRoot;
            }

            m_mainWindow.Activate();

            var dispatcher = DispatcherQueue.GetForCurrentThread();

            dispatcher.TryEnqueue(() =>
            {
                IntPtr hWnd = WindowNative.GetWindowHandle(m_mainWindow);
                WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
                AppWindow appWin = AppWindow.GetFromWindowId(windowId);

                var screen = DisplayArea.GetFromWindowId(windowId, DisplayAreaFallback.Primary).WorkArea;
                int nLeft = screen.X + (screen.Width - tOrderConfig.WindowWidth) / 2;
                int nTop = screen.Y + (screen.Height - tOrderConfig.WindowHeight) / 2;

                appWin.MoveAndResize(new Windows.Graphics.RectInt32(
                    nLeft,
                    nTop,
                    tOrderConfig.WindowWidth,
                    tOrderConfig.WindowHeight
                ));
#if DEBUG
                var debugWindow = new ResolutionWindow();
                debugWindow.Activate();

                var mainRect = new RectInt32(nLeft, nTop, tOrderConfig.WindowWidth, tOrderConfig.WindowHeight);
                debugWindow.SetPositionAndSize(mainRect);
#endif

            });

            this.UnhandledException += (sender, e) =>
            {
                LogUnhandledException(e.Exception);
                e.Handled = true;
            };
        }
        catch (Exception oEx)
        {
            LogUnhandledException(oEx);
            if (Debugger.IsAttached)
                Debugger.Break();
        }
    }



    #endregion //Application Launch
    public void ResizeMainWindow(int width, int height)
    {
        try
        {
            if (MainAppWindow is null)
            {
                Console.WriteLine("[ResizeMainWindow] MainAppWindow is null");
                return;
            }

            IntPtr hWnd = WindowNative.GetWindowHandle(MainAppWindow);
            WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
            AppWindow appWindow = AppWindow.GetFromWindowId(windowId);

            // Zachovej aktuální pozici
            var currentPos = appWindow.Position;

            // Varianta se zachováním X, Y:
            var rect = new RectInt32(currentPos.X, currentPos.Y, width, height);
            appWindow.MoveAndResize(rect);

            Console.WriteLine($"[ResizeMainWindow] Změněno na {width}x{height} při pozici {currentPos.X},{currentPos.Y}");

            // Zakomentovaná logika pro centrování:
            /*
            var screen = DisplayArea.GetFromWindowId(windowId, DisplayAreaFallback.Primary).WorkArea;
            int nLeft = screen.X + (screen.Width - width) / 2;
            int nTop = screen.Y + (screen.Height - height) / 2;
            appWindow.MoveAndResize(new RectInt32(nLeft, nTop, width, height));
            */
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ResizeMainWindow] Chyba: {ex.Message}");
        }
    }




    //-----------------------------------------------------------
    #region Helpers
    //-----------------------------------------------------------

    public static T GetService<T>() where T : class
    {
        return Services.GetRequiredService<T>();
    }

    private static void LogUnhandledException(Exception oEx)
    {
        Console.WriteLine($"[Unhandled Exception] {oEx.Message}\n{oEx.StackTrace}");
    }

    #endregion //Helpers
}

//===================================================================
// class AppEnvironment
//===================================================================

public static class AppEnvironment
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
