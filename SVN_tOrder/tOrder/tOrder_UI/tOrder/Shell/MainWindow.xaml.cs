/***************************************************************************
 *
 * tOrder Application
 *
 * Company      : SPC solutions s.r.o.
 * Author       : Alexandra Seligová
 *
 * Description  :
 * - Main window (UI shell) for the tOrder application.
 * - Initializes and binds the MainWindowViewModel as DataContext.
 *
 ***************************************************************************/
using Microsoft.UI.Xaml;
using Microsoft.UI.Windowing;
using System;
using System.Collections.Generic;
using System.Linq;
using Windows.Graphics;
using Microsoft.UI;

namespace tOrder.Shell
{
    public sealed partial class MainWindow : Window
    {
        private bool _isResizingInternally = false;

        public MainWindow()
        {
            this.InitializeComponent();
            this.SizeChanged += MainWindow_SizeChanged;
        }

        private void MainWindow_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            /*  if (_isResizingInternally) return;

              _isResizingInternally = true;

              var currentSize = e.Size;
              double width = currentSize.Width;
              double height = currentSize.Height;

              double aspectRatio = width / height;

              var targetRatios = new List<(string Label, double Ratio)>
              {
                  ("16:9", 16.0 / 9.0),
                  ("16:10", 16.0 / 10.0),
                  ("4:3", 4.0 / 3.0)
              };

              var closest = targetRatios.OrderBy(r => Math.Abs(aspectRatio - r.Ratio)).First();
              double newHeight = width / closest.Ratio;

              // Změň velikost okna přes AppWindow
              var appWindow = this.AppWindow;
              if (appWindow != null)
              {
                  appWindow.Resize(new SizeInt32((int)width, (int)newHeight));
              }

              _isResizingInternally = false;*/
        }

        private AppWindow AppWindow => Microsoft.UI.Windowing.AppWindow.GetFromWindowId(
            Win32Interop.GetWindowIdFromWindow(WinRT.Interop.WindowNative.GetWindowHandle(this)));
    }
}
