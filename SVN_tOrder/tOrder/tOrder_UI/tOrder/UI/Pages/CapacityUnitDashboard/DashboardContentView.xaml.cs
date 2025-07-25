using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using tOrder.Common;

namespace tOrder.UI
{
    /// <summary>
    /// Displays dashboard tab content with dynamic height adjustment based on window size.
    /// </summary>
    public sealed partial class DashboardContentView : UserControl
    {
        public LayoutConfigVM LayoutConfig => App.GetService<LayoutConfigVM>();

        private bool _isWindowEventsHooked = false;

        public DashboardContentView()
        {
            InitializeComponent();
            Loaded += OnLoaded;
            SizeChanged += OnSizeChanged;
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            UIElementRegistry.RegisterElement(nameof(MainContentGrid), MainContentGrid);
            TryHookWindowEvents();
        }

        private void TryHookWindowEvents()
        {
            if (_isWindowEventsHooked) return;

            var window = App.MainAppWindow;
            if (window != null)
            {
                ApplyContentHeight(window.Size.Width, window.Size.Height);
                _isWindowEventsHooked = true;
            }
            else
            {
                DispatcherQueue.TryEnqueue(() => TryHookWindowEvents());
            }
        }

        private void ApplyContentHeight(double width, double height)
        {
            if (MainContentGrid == null) return;

            // Optional: Pull height reference from a globally registered layout container
            var reference = UIElementRegistry.GetElementByName("MainLayoutRoot") as FrameworkElement;
            if (reference != null)
            {
                MainContentGrid.Height = reference.ActualHeight * 0.5;
                Console.WriteLine($"[Registry] Using MainLayoutRoot: {MainContentGrid.Height:F0}px");
            }
            else
            {
                MainContentGrid.Height = 500;
                Console.WriteLine("[Fallback] MainContentGrid.Height = 500px");
            }

            LogRowHeight();
            LogTopBarHeight();
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ApplyScale(e.NewSize.Width);
            LogRowHeight();
            LogTopBarHeight();
        }

        private void ApplyScale(double currentWidth)
        {
            const double baseWidth = 1280.0;
            double scale = currentWidth / baseWidth;
            // Example: PortManualScale.ScaleX = scale;
        }

        private void LogRowHeight()
        {
            if (Content is Grid mainGrid && mainGrid.RowDefinitions.Count >= 2)
            {
                double rowHeight = mainGrid.RowDefinitions[1].ActualHeight;
                MainContentGrid.Height = rowHeight;
                Console.WriteLine($"[DashboardContentView] Row 1 Height: {rowHeight:F0}px");
            }
        }

        private void LogTopBarHeight()
        {
            var v = UIElementRegistry.ListRegisteredElements();
            var topBarGrid = UIElementRegistry.GetElementByName("TopBarGrid") as FrameworkElement;
            if (topBarGrid != null)
            {
                Console.WriteLine($"[TopBar] TopBarGrid.Height = {topBarGrid.ActualHeight:F0}px");
            }
        }
    }
}
