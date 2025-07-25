//===================================================================
// $Workfile:: ResolutionControl.xaml.cs                            $
// $Author:: Alexandra_Seligova                                     $
// $Revision:: 1                                                    $
// $Date:: 2025-07-25 01:50:00 +0200 (pá, 25 èvc 2025)              $
//===================================================================
// Description: SPC - tOrder
//     Code-behind for ResolutionControl (layout preview, DPI testing)
//===================================================================

using Microsoft.UI;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using Windows.Graphics;

namespace tOrder;

/// <summary>
/// Partial class for ResolutionControl. Provides logic for interacting
/// with resolution presets, aspect locks, and visual feedback.
/// </summary>
public sealed partial class ResolutionControl : UserControl
{
    //-----------------------------------------------------------
    #region Fields & Properties
    //-----------------------------------------------------------

    public event EventHandler<ResolutionChangedEventArgs>? ResolutionChanged;

    private string? _lastAppliedResolution;

    private readonly LayoutConfigVM layoutVM;

    public string CurrentResolution => $"{(int)layoutVM.WindowWidth} x {(int)layoutVM.WindowHeight}";

    #endregion

    //-----------------------------------------------------------
    #region Constructor
    //-----------------------------------------------------------

    public ResolutionControl()
    {
        InitializeComponent();

        layoutVM = App.GetService<LayoutConfigVM>();
        DataContext = layoutVM;
    }

    #endregion

    //-----------------------------------------------------------
    #region Resolution Change Handlers
    //-----------------------------------------------------------

    private void ResizeTo(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is string tag)
        {
            var parts = tag.Split('x');
            if (parts.Length == 2 &&
                int.TryParse(parts[0], out int width) &&
                int.TryParse(parts[1], out int height))
            {
                layoutVM.WindowWidth = width;
                layoutVM.WindowHeight = height;

                _lastAppliedResolution = $"{width}x{height}";
                HighlightActiveButton();
                ResolutionChanged?.Invoke(this, new ResolutionChangedEventArgs(width, height));
            }
        }
    }

    private void EnlargeWindow_Click(object sender, RoutedEventArgs e)
    {
        int newWidth = (int)(layoutVM.WindowWidth * 1.2);
        int newHeight = (int)(layoutVM.WindowHeight * 1.2);

        layoutVM.WindowWidth = newWidth;
        layoutVM.WindowHeight = newHeight;

        _lastAppliedResolution = $"{newWidth}x{newHeight}";
        HighlightActiveButton();
    }

    private void ResetWindow_Click(object sender, RoutedEventArgs e)
    {
        const int defaultWidth = 1280;
        const int defaultHeight = 720;

        layoutVM.WindowWidth = defaultWidth;
        layoutVM.WindowHeight = defaultHeight;

        _lastAppliedResolution = $"{defaultWidth}x{defaultHeight}";
        HighlightActiveButton();
    }

    #endregion

    //-----------------------------------------------------------
    #region Visual Feedback – Highlight Active Button
    //-----------------------------------------------------------

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
                        if (tag.Replace(" ", "") == _lastAppliedResolution?.Replace(" ", ""))
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

    #endregion

    //-----------------------------------------------------------
    #region Aspect Ratio Locks
    //-----------------------------------------------------------

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

    #endregion
}

//===================================================================
// class ResolutionChangedEventArgs
//===================================================================

/// <summary>
/// Represents a resolution change event.
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
