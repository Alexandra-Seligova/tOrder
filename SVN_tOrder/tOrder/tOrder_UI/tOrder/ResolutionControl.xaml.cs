using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Windows.Graphics;

namespace tOrder;
public sealed partial class ResolutionControl : UserControl, INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public event EventHandler<ResolutionChangedEventArgs>? ResolutionChanged;

    private string? _lastAppliedResolution;
    private string _currentResolution = "—";
    private string _currentPageName = "—";
    private string _aspectRatioLock = string.Empty;

    public ResolutionControl()
    {
        InitializeComponent();
        DataContext = this;
        UpdateBindings();
    }

    // ==== Binding vlastnosti (double) ====
    private double scale, scaleX, scaleY, scaleText;
    private double designWidth, designHeight, windowWidth, windowHeight;

    public double Scale { get => scale; set => SetField(ref scale, value); }
    public double ScaleX { get => scaleX; set => SetField(ref scaleX, value); }
    public double ScaleY { get => scaleY; set => SetField(ref scaleY, value); }
    public double ScaleText { get => scaleText; set => SetField(ref scaleText, value); }

    public double DesignWidth { get => designWidth; set => SetField(ref designWidth, value); }
    public double DesignHeight { get => designHeight; set => SetField(ref designHeight, value); }
    public double WindowWidth { get => windowWidth; set => SetField(ref windowWidth, value); }
    public double WindowHeight { get => windowHeight; set => SetField(ref windowHeight, value); }

    // ==== Binding vlastnosti (string/int) ====
    public string CurrentResolution { get => _currentResolution; set => SetField(ref _currentResolution, value); }
    public string CurrentPageName { get => _currentPageName; set => SetField(ref _currentPageName, value); }

    public string BoundElementType { get; set; } = "Grid";
    public string BoundElementName { get; set; } = "ResolutionView";
    public string GroupName { get; set; } = "Main";
    public string SubGroupName { get; set; } = "Display";

    public int Width { get; set; } = 1234;
    public int Height { get; set; } = 765;



    // ==== Notify util ====
    private void OnPropertyChanged(string name)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    private bool SetField<T>(ref T field, T value, [CallerMemberName] string? prop = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value)) return false;
        field = value;
        OnPropertyChanged(prop!);
        return true;
    }
    /*  
       private void SetField<T>(ref T field, T value, [CallerMemberName] string? name = null)
    {
        if (!Equals(field, value))
        {
            field = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

*/
    // ==== Init ====
    private void UpdateBindings()
    {
        WindowWidth = ActualWidth;
        WindowHeight = ActualHeight;
        CurrentResolution = $"{(int)WindowWidth}x{(int)WindowHeight}";
    }
    /*
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
    */


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
                ResolutionChanged?.Invoke(this, new ResolutionChangedEventArgs(width, height));
            }
        }
    }

    private void ResizeMainWindow(int width, int height)
    {
        var mainWindow = Program.MainAppWindow;
        if (mainWindow != null)
        {
            var displayArea = DisplayArea.GetFromWindowId(mainWindow.Id, DisplayAreaFallback.Primary);
            var workArea = displayArea.WorkArea;

            width = Math.Min(width, workArea.Width);
            height = Math.Min(height, workArea.Height);

            int posX = (workArea.Width - width) / 2;
            int posY = (workArea.Height - height) / 2;

            mainWindow.MoveAndResize(new RectInt32(posX, posY, width, height));
        }
    }

    private void EnlargeWindow_Click(object sender, RoutedEventArgs e)
    {
        var mainWindow = Program.MainAppWindow;
        if (mainWindow != null)
        {
            var currentSize = mainWindow.Size;
            int newWidth = (int)(currentSize.Width * 1.2);
            int newHeight = (int)(currentSize.Height * 1.2);

            mainWindow.Resize(new Windows.Graphics.SizeInt32(newWidth, newHeight));
            CurrentResolution = $"{newWidth} x {newHeight}";
            HighlightActiveButton();
        }
    }

    private void ResetWindow_Click(object sender, RoutedEventArgs e)
    {
        const int defaultWidth = 1280;
        const int defaultHeight = 720;

        ResizeMainWindow(defaultWidth, defaultHeight);
        CurrentResolution = $"{defaultWidth} x {defaultHeight}";
        HighlightActiveButton();
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


    private void AspectRatioCheckBox_Checked(object sender, RoutedEventArgs e)
    {
        if (sender == Lock16_9CheckBox)
        {
            Lock4_3CheckBox.IsChecked = false;
            Lock16_10CheckBox.IsChecked = false;
            Console.WriteLine("[ResolutionControl] Locked aspect ratio to 16:9");
            // Tady by bylo možné nastavit skuteèný zámek rátia do ViewModelu
        }
        else if (sender == Lock4_3CheckBox)
        {
            Lock16_9CheckBox.IsChecked = false;
            Lock16_10CheckBox.IsChecked = false;
            Console.WriteLine("[ResolutionControl] Locked aspect ratio to 4:3");
        }
        else if (sender == Lock16_10CheckBox)
        {
            Lock16_9CheckBox.IsChecked = false;
            Lock4_3CheckBox.IsChecked = false;
            Console.WriteLine("[ResolutionControl] Locked aspect ratio to 16:10");
        }
    }

    private void AspectRatioCheckBox_Unchecked(object sender, RoutedEventArgs e)
    {
        if (Lock16_9CheckBox.IsChecked != true &&
            Lock4_3CheckBox.IsChecked != true &&
            Lock16_10CheckBox.IsChecked != true)
        {
            Console.WriteLine("[ResolutionControl] Aspect ratio lock removed.");
            // Odstranìní zámku zde
        }
    }

}




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
