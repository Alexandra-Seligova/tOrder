//===================================================================
// $Workfile:: MainWindow.xaml.cs                                   $
// $Author:: Alexandra_Seligova                                     $
// $Revision:: 7                                                    $
// $Date:: 2025-07-24 23:05:00 +0200 (čt, 24 čvc 2025)              $
//===================================================================
// Description: SPC - tOrder
//     Main application window shell. Hosts the MainLayout and 
//     integrates LayoutConfigVM for responsive size handling.
//===================================================================

namespace tOrder;

#region 🔧 Using directives

using System;
using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using tOrder.Shell;
using WinRT.Interop;
using Windows.Graphics;

#endregion

//===================================================================
// 📦 App – Main WinUI 3 Application Class
//===================================================================

public partial class App : Application
{
    // 🔗 Global DI container
    public static IServiceProvider Services { get; private set; } = null!;

    // 🪟 Main window instances (XAML + AppWindow)
    public static Window MainWindowInstance { get; private set; } = null!;
    public static AppWindow MainAppWindow { get; private set; } = null!;

    // 🏠 Local main window reference
    private Window m_mainWindow = null!;

    //===================================================================
    // 🚀 Constructor
    //===================================================================
    public App()
    {
        Services = Program.AppHost.Services;
        InitializeComponent();
        RequestedTheme = ApplicationTheme.Light;

        // 🔎 Detect if app runs as MSIX or unpackaged
        bool isPackaged = AppEnvironment.IsPackaged;
        Console.WriteLine($"[App] Packaging Mode: {(isPackaged ? "Packaged" : "Unpackaged")}");

        // 🖥️ Print screen resolution of primary monitor
        var screen = DisplayArea.GetFromPoint(new PointInt32(0, 0), DisplayAreaFallback.Primary).WorkArea;
        Console.WriteLine($"[Win] Screen Resolution: {screen.Width}x{screen.Height} (primary)");
    }

    //===================================================================
    // 🧭 OnLaunched – Initialize main window and content
    //===================================================================
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
                var rootFrame = new Frame();
                rootFrame.Navigate(typeof(MainLayout)); // Main layout navigation
                m_mainWindow.Content = rootFrame;
            }

            m_mainWindow.Activate(); // Activate the window

            var dispatcher = DispatcherQueue.GetForCurrentThread();

            dispatcher.TryEnqueue(() =>
            {
                // Attach AppWindow for native control
                IntPtr hWnd = WindowNative.GetWindowHandle(m_mainWindow);
                WindowId windowId = Win32Interop.GetWindowIdFromWindow(hWnd);
                MainAppWindow = AppWindow.GetFromWindowId(windowId);

                // Center the window on the screen
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
                // Show diagnostic window (ResolutionWindow) for dev layout preview
                var debugWindow = new ResolutionWindow();
                debugWindow.Activate();
                debugWindow.SetPositionAndSize(new RectInt32(nLeft, nTop, tOrderConfig.WindowWidth, tOrderConfig.WindowHeight));
#endif
            });

            // Catch all unhandled exceptions and prevent crash
            this.UnhandledException += (sender, e) =>
            {
                LogUnhandledException(e.Exception);
                e.Handled = true;
            };
        }
        catch (Exception ex)
        {
            LogUnhandledException(ex);
            if (Debugger.IsAttached)
                Debugger.Break();
        }
    }

    //===================================================================
    // 📐 ResizeMainWindow – Dynamically change main window size
    //===================================================================
    public void ResizeMainWindow(int width, int height)
    {
        try
        {
            if (MainAppWindow is null)
            {
                Console.WriteLine("[ResizeMainWindow] MainAppWindow is null");
                return;
            }

            var pos = MainAppWindow.Position;
            MainAppWindow.MoveAndResize(new RectInt32(pos.X, pos.Y, width, height));
            Console.WriteLine($"[ResizeMainWindow] Resized to {width}x{height} at position {pos.X},{pos.Y}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[ResizeMainWindow] Error: {ex.Message}");
        }
    }

    //===================================================================
    // 🧱 GetService<T> – Resolve service from global container
    //===================================================================
    public static T GetService<T>() where T : class =>
        Services.GetRequiredService<T>();

    //===================================================================
    // 🪓 LogUnhandledException – Console print for fatal errors
    //===================================================================
    private static void LogUnhandledException(Exception ex) =>
        Console.WriteLine($"[Unhandled Exception] {ex.Message}\n{ex.StackTrace}");
}

//===================================================================
// 📦 AppEnvironment – Check if app runs as MSIX package
//===================================================================

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
//===================================================================
// 📘 Overview: App.xaml.cs
//===================================================================
//
// This file defines the main entry class of the tOrder application
// (`App : Application`), responsible for initializing the application,
// handling dependency injection, setting up the main window, and
// integrating support services.
//
// 🧩 Key responsibilities:
//
// ✅ Dependency Injection
//    - Gets the DI container from `Program.AppHost.Services`.
//    - Provides `App.GetService<T>()` method for resolving global services.
//
// ✅ Main Window Setup
//    - Instantiates the main window (`MainWindow`), sets up content
//      via navigation to `MainLayout`, and activates the window.
//
// ✅ AppWindow & Layout
//    - Retrieves native `AppWindow` handle to enable full window control.
//    - Centers and resizes the main window using `tOrderConfig`.
//
// ✅ Unpackaged Support
//    - Logs whether the app is running as MSIX package or unpackaged.
//    - Optional console output during startup.
//
// ✅ Diagnostic Tools
//    - In DEBUG builds, opens an additional `ResolutionWindow`
//      for layout and resolution testing.
//
// ✅ Exception Handling
//    - Captures all unhandled exceptions globally to avoid crashes.
//
// 📦 Additional Class: AppEnvironment
//    - Provides `IsPackaged` boolean to determine packaging mode.
//
// This class serves as the glue between DI, XAML shell window,
// and runtime environment setup.
//
//===================================================================
