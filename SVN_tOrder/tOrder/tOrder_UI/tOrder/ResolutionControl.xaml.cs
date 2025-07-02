using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Graphics;

namespace tOrder
{
    public sealed partial class ResolutionControl : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Event vyvolaný pøi zmìnì rozlišení.
        /// </summary>
        public event EventHandler<ResolutionChangedEventArgs>? ResolutionChanged;

        private string? _lastAppliedResolution;
        private string _currentResolution = "—";
        private string _currentPageName = "—";

        public string CurrentResolution
        {
            get => _currentResolution;
            set => SetField(ref _currentResolution, value);
        }

        public string CurrentPageName
        {
            get => _currentPageName;
            set => SetField(ref _currentPageName, value);
        }

        public ResolutionControl()
        {
            this.InitializeComponent();
            this.DataContext = this;
            UpdateBindings();
        }

        private void ResizeTo(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is string tag)
            {
                var parts = tag.Split('x');
                if (parts.Length == 2 &&
                    int.TryParse(parts[0], out int width) &&
                    int.TryParse(parts[1], out int height))
                {
                    ResizeMainWindow(width, height);

                    CurrentResolution = $"{width} x {height}";
                    _lastAppliedResolution = CurrentResolution;

                    HighlightActiveButton();

                    // Vyvolání eventu
                    ResolutionChanged?.Invoke(this, new ResolutionChangedEventArgs(width, height));
                }
            }
        }

        private void HighlightActiveButton()
        {
            foreach (var element in RootGrid.Children)
            {
                if (element is StackPanel panel)
                {
                    foreach (var child in panel.Children)
                    {
                        if (child is Button button && button.Tag is string tag)
                        {
                            if ($"{tag.Replace('x', 'x')}" == _lastAppliedResolution?.Replace(" ", ""))
                            {
                                button.Background = new SolidColorBrush(ColorHelper.FromArgb(255, 112, 140, 180));
                                button.Foreground = new SolidColorBrush(Colors.White);
                            }
                            else
                            {
                                button.ClearValue(Button.BackgroundProperty);
                                button.ClearValue(Button.ForegroundProperty);
                            }
                        }
                    }
                }
            }
        }

        private void ResizeMainWindow(int width, int height)
        {
            var mainWindow = Program.MainAppWindow;
            if (mainWindow != null)
            {
                mainWindow.Resize(new SizeInt32(width, height));
                mainWindow.Move(new PointInt32(100, 100));
            }
        }

        public void UpdateBindings()
        {
            var main = Program.MainAppWindow;
            if (main != null)
            {
                var size = main.Size;
                CurrentResolution = $"{size.Width} x {size.Height}";
            }

            CurrentPageName = Program.CurrentPageName ?? "—";
        }

        private void SetField<T>(ref T field, T value, [CallerMemberName] string? name = null)
        {
            if (!Equals(field, value))
            {
                field = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
            }
        }
    }

    /// <summary>
    /// Argumenty pro ResolutionChanged event.
    /// </summary>
    public class ResolutionChangedEventArgs : EventArgs
    {
        public int Width { get; }
        public int Height { get; }

        public ResolutionChangedEventArgs(int width, int height)
        {
            Width = width;
            Height = height;
        }
    }
}
