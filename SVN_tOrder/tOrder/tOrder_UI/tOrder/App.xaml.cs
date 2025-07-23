//===================================================================
// $Workfile:: App.xaml.cs                                          $
// $Author:: Alexandra_Seligova                                      $
// $Revision:: 5                                                    $
// $Date:: 2025-07-17                                              $
//===================================================================
// Description: SPC - tOrder
//     Application entry with AppWindow and DI integration
//===================================================================

namespace tOrder;

using System;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using WinRT.Interop;
using Windows.Graphics;
using Microsoft.UI.Dispatching;
using Microsoft.UI;
using tOrder.Shell;

public partial class App : Application
{
    public static WindowDebugViewModel DebugVM { get; private set; } = null!;
    public static IServiceProvider Services { get; private set; } = null!;

    public static Window MainWindowInstance { get; private set; } = null!;
    public static AppWindow MainAppWindow { get; private set; } = null!;

    private Window m_mainWindow = null!;

    public App()
    {
        Services = Program.AppHost.Services;
        DebugVM = Services.GetRequiredService<WindowDebugViewModel>();

        this.InitializeComponent();
        this.RequestedTheme = ApplicationTheme.Light;

        bool bIsPackaged = AppEnvironment.IsPackaged;
        Console.WriteLine($"[App] Packaging Mode: {(bIsPackaged ? "Packaged" : "Unpackaged")}");

        var screen = DisplayArea.GetFromPoint(new Windows.Graphics.PointInt32(0, 0), DisplayAreaFallback.Primary).WorkArea;
        Console.WriteLine($"[Win] Screen Resolution: {screen.Width}x{screen.Height} at center");
    }

    protected override void OnLaunched(LaunchActivatedEventArgs args)
    {
        try
        {
            if (m_mainWindow == null)
            {
                m_mainWindow = Services.GetRequiredService<MainWindow>();
                MainWindowInstance = m_mainWindow;
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
                MainAppWindow = AppWindow.GetFromWindowId(windowId);

                var screen = DisplayArea.GetFromWindowId(windowId, DisplayAreaFallback.Primary).WorkArea;
                int nLeft = screen.X + (screen.Width - tOrderConfig.WindowWidth) / 2;
                int nTop = screen.Y + (screen.Height - tOrderConfig.WindowHeight) / 2;

                MainAppWindow.MoveAndResize(new RectInt32(
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

    public void ResizeMainWindow(int width, int height)
    {
        try
        {
            if (MainAppWindow is null)
            {
                Console.WriteLine("[ResizeMainWindow] MainAppWindow is null");
                return;
            }

            var currentPos = MainAppWindow.Position;

            var rect = new RectInt32(currentPos.X, currentPos.Y, width, height);
            MainAppWindow.MoveAndResize(rect);

            Console.WriteLine($"[ResizeMainWindow] Změněno na {width}x{height} při pozici {currentPos.X},{currentPos.Y}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ResizeMainWindow] Chyba: {ex.Message}");
        }
    }

    public static T GetService<T>() where T : class
    {
        return Services.GetRequiredService<T>();
    }

    private static void LogUnhandledException(Exception oEx)
    {
        Console.WriteLine($"[Unhandled Exception] {oEx.Message}\n{oEx.StackTrace}");
    }
}

public static class AppEnvironment
{
    public static bool IsPackaged
    {
        get
        {
            try { var _ = Windows.ApplicationModel.Package.Current; return true; }
            catch { return false; }
        }
    }
}
