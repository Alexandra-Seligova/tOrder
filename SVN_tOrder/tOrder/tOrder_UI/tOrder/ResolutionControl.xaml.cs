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
using Microsoft.UI.Xaml.Controls.Primitives;
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
    public LayoutConfigVM LayoutConfig => App.GetService<LayoutConfigVM>();

    public string CurrentResolution => $"{(int)LayoutConfig.WindowWidth} x {(int)LayoutConfig.WindowHeight}";


    //-----------------------------------------------------------
    // Zoom GUI Controls (DesignWidth based)
    //-----------------------------------------------------------

    private const int MinDesignWidth = 600;
    private const int MaxDesignWidth = 1400;
    private const int DefaultDesignWidth = 1024;
    private const int ZoomStep = 50;



    #endregion

    //-----------------------------------------------------------
    #region Constructor
    //-----------------------------------------------------------

    public ResolutionControl()
    {
        InitializeComponent();

        DataContext = LayoutConfig;
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
                LayoutConfig.WindowWidth = width;
                LayoutConfig.WindowHeight = height;

                _lastAppliedResolution = $"{width}x{height}";
                HighlightActiveButton();
                ResolutionChanged?.Invoke(this, new ResolutionChangedEventArgs(width, height));
            }
        }
    }

    private void EnlargeWindow_Click(object sender, RoutedEventArgs e)
    {
        int newWidth = (int)(LayoutConfig.WindowWidth * 1.2);
        int newHeight = (int)(LayoutConfig.WindowHeight * 1.2);

        LayoutConfig.WindowWidth = newWidth;
        LayoutConfig.WindowHeight = newHeight;

        _lastAppliedResolution = $"{newWidth}x{newHeight}";
        HighlightActiveButton();
    }

    private void ResetWindow_Click(object sender, RoutedEventArgs e)
    {
        const int defaultWidth = 1280;
        const int defaultHeight = 720;

        LayoutConfig.WindowWidth = defaultWidth;
        LayoutConfig.WindowHeight = defaultHeight;

        _lastAppliedResolution = $"{defaultWidth}x{defaultHeight}";
        HighlightActiveButton();
    }
    private void ZoomIn_Click(object sender, RoutedEventArgs e)
    {
        LayoutConfig.DesignWidth = Math.Min(MaxDesignWidth, LayoutConfig.DesignWidth + ZoomStep);
    }

    private void ZoomOut_Click(object sender, RoutedEventArgs e)
    {
        LayoutConfig.DesignWidth = Math.Max(MinDesignWidth, LayoutConfig.DesignWidth - ZoomStep);
    }

    private void ZoomReset_Click(object sender, RoutedEventArgs e)
    {
        LayoutConfig.DesignWidth = DefaultDesignWidth;
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
