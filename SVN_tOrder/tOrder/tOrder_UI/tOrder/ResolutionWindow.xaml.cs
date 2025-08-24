using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using System;
using Windows.Graphics;

namespace tOrder
{
    public sealed partial class ResolutionWindow : Window
    {
        private AppWindow? _appWindow;

        public ResolutionWindow()
        {
#if DEBUG
            this.InitializeComponent();
            InitWindow();

            ResControl.ResolutionChanged += OnResolutionChangedFromControl;
#endif
        }

        private void InitWindow()
        {
            var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            var windowId = Win32Interop.GetWindowIdFromWindow(hwnd);
            _appWindow = AppWindow.GetFromWindowId(windowId);
            _appWindow.Title = "Debug: Resolution";
        }

        private void OnResolutionChangedFromControl(object? sender, ResolutionChangedEventArgs e)
        {
            // zavoláme App.xaml.cs metodu pro změnu hlavního okna
            if (Application.Current is App app)
            {
                app.ResizeMainWindow(e.Width, e.Height);
            }
        }

        public void SetPositionAndSize(RectInt32 mainWindowRect)
        {
            if (_appWindow == null) return;

            int debugHeight = 240;
            int spacing = 10;

            int debugX = mainWindowRect.X;
            int debugY = Math.Max(0, mainWindowRect.Y - debugHeight - spacing);

            _appWindow.MoveAndResize(new RectInt32(debugX, debugY, mainWindowRect.Width, debugHeight));
        }
    }

}
