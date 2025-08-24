//===================================================================
// $Workfile:: MainWindow.xaml.cs                                  $
// $Author:: Alexandra_Seligova                                    $
// $Revision:: 7                                                   $
// $Date:: 2025-07-24 23:05:00 +0200 (čt, 24 čvc 2025)             $
//===================================================================
// Description:  - tOrder
//     Main application window shell. Hosts the MainLayout and 
//     integrates LayoutConfigVM for responsive size handling.
//===================================================================

namespace tOrder.Shell
{
    //-----------------------------------------------------------
    #region Using directives
    //-----------------------------------------------------------

    using Microsoft.UI.Xaml;
    using Microsoft.UI.Windowing;
    using Microsoft.UI;
    using System;
    using System.ComponentModel;
    using Windows.Graphics;

    #endregion // Using directives

    //===================================================================
    // class MainWindow : Window
    //===================================================================

    /// <summary>
    /// MainWindow acts as the visual shell of the application.
    /// Hosts MainLayout and applies responsive layout constraints 
    /// via LayoutConfigVM. All UI logic is delegated.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        //-----------------------------------------------------------
        #region Fields
        //-----------------------------------------------------------

        private bool _isResizingInternally = false;
        private LayoutConfigVM layoutVM;

        #endregion // Fields

        //-----------------------------------------------------------
        #region Constructor
        //-----------------------------------------------------------

        public MainWindow()
        {
            this.InitializeComponent();
            this.SizeChanged += MainWindow_SizeChanged;


            // Resolve LayoutConfigVM via DI container
            layoutVM = App.GetService<LayoutConfigVM>();
            if (layoutVM != null)
            {
                layoutVM.UpdateDisplayInfo();
                layoutVM.PropertyChanged += LayoutVM_PropertyChanged;
            }
        }

        #endregion // Constructor

        //-----------------------------------------------------------
        #region Event Handlers
        //-----------------------------------------------------------

        /// <summary>
        /// Reacts to ViewModel changes (WindowWidth / WindowHeight)
        /// and resizes the window accordingly, within allowed bounds.
        /// </summary>
        private void LayoutVM_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName is nameof(layoutVM.WindowWidth) or nameof(layoutVM.WindowHeight))
            {
                int minW = (int)LayoutConfigVM.MinWindowWidth;
                int minH = (int)LayoutConfigVM.MinWindowHeight;
                int maxW = Math.Min((int)LayoutConfigVM.MaxWindowWidth, layoutVM.DisplayWidth);
                int maxH = Math.Min((int)LayoutConfigVM.MaxWindowHeight, layoutVM.DisplayHeight);

                int newWidth = Clamp((int)layoutVM.WindowWidth, minW, maxW);
                int newHeight = Clamp((int)layoutVM.WindowHeight, minH, maxH);

                App.GetService<App>().ResizeMainWindow(newWidth, newHeight);

#if DEBUG
                Console.WriteLine($"[MainWindow] Resized to {newWidth} x {newHeight} (max: {maxW}x{maxH}, display: {layoutVM.DisplayWidth}x{layoutVM.DisplayHeight})");
#endif
            }
        }

        /// <summary>
        /// Handles user-initiated window resize (mouse drag, etc.).
        /// Enforces min/max window constraints.
        /// </summary>
        private void MainWindow_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            int minW = (int)LayoutConfigVM.MinWindowWidth;
            int minH = (int)LayoutConfigVM.MinWindowHeight;
            int maxW = (int)LayoutConfigVM.MaxWindowWidth;
            int maxH = (int)LayoutConfigVM.MaxWindowHeight;

            int newWidth = Clamp((int)e.Size.Width, minW, maxW);
            int newHeight = Clamp((int)e.Size.Height, minH, maxH);

            if (newWidth != (int)e.Size.Width || newHeight != (int)e.Size.Height)
            {
                AppWindow?.Resize(new SizeInt32(newWidth, newHeight));
            }
        }

        #endregion // Event Handlers

        //-----------------------------------------------------------
        #region Private Helpers
        //-----------------------------------------------------------

        /// <summary>
        /// Gets the AppWindow instance for the current native window.
        /// </summary>
        private AppWindow AppWindow =>
            AppWindow.GetFromWindowId(
                Win32Interop.GetWindowIdFromWindow(
                    WinRT.Interop.WindowNative.GetWindowHandle(this)));

        /// <summary>
        /// Ensures a value stays within defined bounds.
        /// </summary>
        private static int Clamp(int value, int min, int max) =>
            Math.Max(min, Math.Min(value, max));

        #endregion // Private Helpers


        //-----------------------------------------------------------
        #region Window Configuration (🧪 DEMO – full AppWindow setup)
        //-----------------------------------------------------------

        /// <summary>
        /// Applies full demo configuration to the AppWindow.
        /// You can selectively comment/uncomment as needed.
        /// </summary>
        private void ConfigureAppWindow()
        {
            var appWindow = AppWindow;

            //───────────────────────────────────────────────────────
            // 🏷️ Title text
            //───────────────────────────────────────────────────────
            appWindow.Title = "tOrder Application";

            //───────────────────────────────────────────────────────
            // 📐 Initial size (applied once on startup)
            //───────────────────────────────────────────────────────
            appWindow.Resize(new SizeInt32(1280, 768));

            //───────────────────────────────────────────────────────
            // 📍 Initial screen position (optional)
            //───────────────────────────────────────────────────────
            appWindow.Move(new PointInt32(100, 100));

            //───────────────────────────────────────────────────────
            // 🪟 Presenter behavior (must be casted to Overlapped)
            //───────────────────────────────────────────────────────
            if (appWindow.Presenter is OverlappedPresenter presenter)
            {
                presenter.IsResizable = false;
                presenter.IsMaximizable = false;
                presenter.IsMinimizable = true;

                // Optional – hide OS border + title bar (experimental!)
                // presenter.SetBorderAndTitleBar(false);
            }

            //───────────────────────────────────────────────────────
            // 🎛️ Custom title bar (use your own UI element)
            //───────────────────────────────────────────────────────
            this.ExtendsContentIntoTitleBar = false;

            // Optionally, use:
            // this.SetTitleBar(MyCustomGrid);

            //───────────────────────────────────────────────────────
            // 🖼️ Content scaling (optional – used for DPI / zoom)
            //───────────────────────────────────────────────────────
            // this.XamlRoot.RasterizationScale = 1.25;

            //───────────────────────────────────────────────────────
            // 🖼️ Background color (set by theme or manually)
            //───────────────────────────────────────────────────────
            // appWindow.TitleBar.BackgroundColor = Colors.DarkSlateGray;
            // appWindow.TitleBar.ForegroundColor = Colors.White;

            //───────────────────────────────────────────────────────
            // 🎯 Icon (not settable via API – must use manifest)
            //───────────────────────────────────────────────────────
            // Set via .rc file or AppManifest – no runtime API

            //───────────────────────────────────────────────────────
            // 🌙 Optional: TitleBar theming (if using default chrome)
            //───────────────────────────────────────────────────────
            // appWindow.TitleBar.PreferredTheme = AppWindowTitleBarTheme.Dark;
        }

        #endregion // Window Configuration
    }

    //===================================================================
}

/*
===============================================================================
 ✅ Window Configuration via AppWindow API (MainWindow.xaml.cs)
===============================================================================

  Parameter                     | API / Method
-------------------------------|------------------------------------------------
  Window size                  | AppWindow.Resize(new SizeInt32(width, height))
  Window position              | AppWindow.Move(new PointInt32(x, y))
  Window title                 | AppWindow.Title = "MyApp";
  Display mode (normal/full)   | Use SetPresenter(...) or preconfigured presenter
  Custom title bar             | this.ExtendsContentIntoTitleBar = true
                               | this.SetTitleBar(MyTitleBarElement);
  Min/max size constraints     | OverlappedPresenter.IsResizable = false
                               | SizeChanged event + manual validation
  Window icon                  | Set via app manifest (.rc) — no direct API
===============================================================================
*/
