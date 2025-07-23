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
            if (_isWindowEventsHooked)
                return;

            var window = App.MainAppWindow;
            if (window != null)
            {
                var size = window.Size;
                ApplyLayoutBasedOnAspect(size.Width, size.Height);
                _isWindowEventsHooked = true;
            }
            else
            {
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

            LogRowHeight();
        }

        private void DashboardContentView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ApplyScale(e.NewSize.Width);
            LogRowHeight();
            //  MainContentGrid.Height = e.NewSize.Height;
            // LogRowHeight();
        }

        private void ApplyScale(double currentWidth)
        {
            const double baseWidth = 1280.0;
            double scale = currentWidth / baseWidth;

            // Aplikuj škálování podle potřeby
            // Např. PortManualScale.ScaleX = scale;
        }
        private void LogRowHeight()
        {
            if (this.Content is Grid mainGrid &&
                mainGrid.RowDefinitions.Count >= 2)
            {
                double middleRowHeight = mainGrid.RowDefinitions[1].ActualHeight;
                MainContentGrid.Height = middleRowHeight;
                Console.WriteLine($"[DashboardContentView] Middle Row (Row 1) Height: {middleRowHeight:F0}px");
            }
        }
    }
}
