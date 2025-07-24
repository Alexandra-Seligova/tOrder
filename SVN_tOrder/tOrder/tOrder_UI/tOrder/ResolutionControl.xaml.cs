using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using Windows.Graphics;

namespace tOrder;

public sealed partial class ResolutionControl : UserControl
{
    public event EventHandler<ResolutionChangedEventArgs>? ResolutionChanged;

    private string? _lastAppliedResolution;

    // ViewModel získaný pøes DI (celý layout aplikace)
    private readonly LayoutConfigVM layoutVM;

    public ResolutionControl()
    {
        InitializeComponent();
        layoutVM = App.GetService<LayoutConfigVM>();
        DataContext = layoutVM; // Všechny bindingy v XAML jdou na layoutVM
    }

    /// <summary>
    /// Vrací aktuální rozlišení okna jako string (pro zobrazení ve view)
    /// </summary>
    public string CurrentResolution => $"{(int)layoutVM.WindowWidth} x {(int)layoutVM.WindowHeight}";

    /// <summary>
    /// Nastaví rozlišení na základì Tag tlaèítka (ve formátu "ŠxV")
    /// </summary>
    private void ResizeTo(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is string tag)
        {
            var parts = tag.Split('x');
            if (parts.Length == 2 &&
                int.TryParse(parts[0], out int width) &&
                int.TryParse(parts[1], out int height))
            {
                // Nastav hodnoty do ViewModelu – zpropaguje se do MainWindow a všude jinde
                layoutVM.WindowWidth = width;
                layoutVM.WindowHeight = height;

                _lastAppliedResolution = $"{width}x{height}";
                HighlightActiveButton();
                ResolutionChanged?.Invoke(this, new ResolutionChangedEventArgs(width, height));
            }
        }
    }

    /// <summary>
    /// Zvìtší okno o 20 % a uloží novou velikost do VM.
    /// </summary>
    private void EnlargeWindow_Click(object sender, RoutedEventArgs e)
    {
        int newWidth = (int)(layoutVM.WindowWidth * 1.2);
        int newHeight = (int)(layoutVM.WindowHeight * 1.2);

        layoutVM.WindowWidth = newWidth;
        layoutVM.WindowHeight = newHeight;

        _lastAppliedResolution = $"{newWidth}x{newHeight}";
        HighlightActiveButton();
    }

    /// <summary>
    /// Resetuje okno na výchozí rozlišení (1280x720).
    /// </summary>
    private void ResetWindow_Click(object sender, RoutedEventArgs e)
    {
        const int defaultWidth = 1280;
        const int defaultHeight = 720;

        layoutVM.WindowWidth = defaultWidth;
        layoutVM.WindowHeight = defaultHeight;

        _lastAppliedResolution = $"{defaultWidth}x{defaultHeight}";
        HighlightActiveButton();
    }

    /// <summary>
    /// Zvýrazní aktivní tlaèítko rozlišení podle posledního nastaveného.
    /// </summary>
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
                        if ($"{tag.Replace(" ", "")}" == _lastAppliedResolution?.Replace(" ", ""))
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

    /// <summary>
    /// Obsluha zamèení pomìru stran – pro další rozšíøení do ViewModelu
    /// </summary>
    private void AspectRatioCheckBox_Checked(object sender, RoutedEventArgs e)
    {
        if (sender == Lock16_9CheckBox)
        {
            Lock4_3CheckBox.IsChecked = false;
            Lock16_10CheckBox.IsChecked = false;
            Console.WriteLine("[ResolutionControl] Locked aspect ratio to 16:9");
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
        }
    }
}

// Událost pro zmìnu rozlišení (volitelnì, pro napojení dalších èástí UI)
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
