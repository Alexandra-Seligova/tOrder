using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;

namespace tOrder.UI
{
    public sealed partial class DashboardContentView : UserControl
    {
        private bool _isWindowEventsHooked = false;

        public DashboardContentView()
        {
            this.InitializeComponent();

            this.Loaded += DashboardContentView_Loaded;
            this.SizeChanged += DashboardContentView_SizeChanged;
        }

        private void DashboardContentView_Loaded(object sender, RoutedEventArgs e)
        {
            TryHookWindowEvents();
        }

        private void TryHookWindowEvents()
        {
            // Pokud už je zaregistrováno, nedělej nic
            if (_isWindowEventsHooked)
                return;

            var window = App.MainAppWindow;
            if (window != null)
            {
                window.SizeChanged += Window_SizeChanged;
                ApplyLayoutBasedOnAspect(window.Bounds.Width, window.Bounds.Height);
                _isWindowEventsHooked = true;
            }
            else
            {
                // Odlož provázání přes Dispatcher (okno ještě nemusí být připravené)
                this.DispatcherQueue.TryEnqueue(() => TryHookWindowEvents());
            }
        }

        private void Window_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            ApplyLayoutBasedOnAspect(e.Size.Width, e.Size.Height);
        }

        private void ApplyLayoutBasedOnAspect(double width, double height)
        {
            if (MainContentGrid == null) return;

            double aspectRatio = width / height;

            if (aspectRatio >= 1.7)
            {
                MainContentGrid.Height = 276;
            }
            else
            {
                MainContentGrid.Height = 500;
            }
        }

        private void DashboardContentView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ApplyScale(e.NewSize.Width);
        }

        private void ApplyScale(double currentWidth)
        {
            const double baseWidth = 1280.0;
            double scale = currentWidth / baseWidth;

            // Aplikuj škálování podle potřeby
            // Např. PortManualScale.ScaleX = scale;
        }
    }
}
