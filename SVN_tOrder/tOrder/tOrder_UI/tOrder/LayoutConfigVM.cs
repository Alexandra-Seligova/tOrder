//===================================================================
// $Workfile:: LayoutConfigVM.cs                                   $
// $Author:: Alexandra_Seligova                                    $
// $Revision:: 3                                                   $
// $Date:: 2025-07-25 01:25:00 +0200 (pá, 25 čvc 2025)             $
//===================================================================
// Description:  - tOrder
//     ViewModel and data model for layout configuration (window size,
//     scale, DPI info, design resolution) with live validation.
//===================================================================

namespace tOrder;

#region Using directives
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Microsoft.UI.Windowing;
using Windows.Graphics;
#endregion

//===================================================================
// class LayoutConfigModel
//===================================================================

public class LayoutConfigModel
{
    public double WindowWidth { get; set; } = 1280;
    public double WindowHeight { get; set; } = 960;
    public double Scale { get; set; } = 1;
    public double ScaleX { get; set; } = 1;
    public double ScaleY { get; set; } = 1;
    public double ScaleText { get; set; } = 1;
    public double DesignWidth { get; set; } = 1024;
    public double DesignHeight { get; set; } = 768;
}

//===================================================================
// class LayoutConfigVM
//===================================================================

public class LayoutConfigVM : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;

    public LayoutConfigModel Model { get; }

    public const double MinWindowWidth = 640;
    public const double MaxWindowWidth = 8192;
    public const double MinWindowHeight = 480;
    public const double MaxWindowHeight = 4320;
    public const double MinAspectRatio = 1.2;
    public const double MaxAspectRatio = 2.4;
    public const double MinScale = 0.5;
    public const double MaxScale = 4.0;
    public const int WindowSizeStep = 4;

    public int DisplayWidth { get; private set; }
    public int DisplayHeight { get; private set; }
    public double DisplayDpi { get; private set; }

    public const int MinDesignWidth = 600;
    public const int MaxDesignWidth = 1400;

    public double GuiScaleNormalized
    {
        get => (DesignWidth - 1024) / 400.0; // 0 odpovídá 1024
        set
        {
            DesignWidth = (int)Math.Round(1024 + value * 50);
            OnPropertyChanged(nameof(GuiScaleNormalized));
        }
    }

    public LayoutConfigVM(LayoutConfigModel model)
    {
        Model = model ?? throw new ArgumentNullException(nameof(model));
        UpdateDisplayInfo();
    }

    public void UpdateDisplayInfo()
    {
        var area = DisplayArea.GetFromPoint(new PointInt32(0, 0), DisplayAreaFallback.Primary).WorkArea;
        DisplayWidth = area.Width;
        DisplayHeight = area.Height;
        DisplayDpi = 96.0;
        OnPropertyChanged(nameof(DisplayWidth));
        OnPropertyChanged(nameof(DisplayHeight));
        OnPropertyChanged(nameof(DisplayDpi));
    }

    #region Properties

    public double WindowWidth
    {
        get => Model.WindowWidth;
        set
        {
            var validated = ValidateWindowWidth(value);
            if (Model.WindowWidth != validated)
            {
                Console.WriteLine($"[LayoutConfig] WindowWidth: {Model.WindowWidth} → {validated}");
                Model.WindowWidth = validated;
                OnPropertyChanged();
            }
        }
    }

    public double WindowHeight
    {
        get => Model.WindowHeight;
        set
        {
            var validated = ValidateWindowHeight(value);
            if (Model.WindowHeight != validated)
            {
                Console.WriteLine($"[LayoutConfig] WindowHeight: {Model.WindowHeight} → {validated}");
                Model.WindowHeight = validated;
                OnPropertyChanged();
            }
        }
    }

    public double Scale
    {
        get => Model.Scale;
        set
        {
            var validated = ValidateScale(value);
            if (Model.Scale != validated)
            {
                Console.WriteLine($"[LayoutConfig] Scale: {Model.Scale} → {validated}");
                Model.Scale = validated;
                OnPropertyChanged();
            }
        }
    }

    public double ScaleX
    {
        get => Model.ScaleX;
        set
        {
            var validated = ValidateScale(value);
            if (Model.ScaleX != validated)
            {
                Console.WriteLine($"[LayoutConfig] ScaleX: {Model.ScaleX} → {validated}");
                Model.ScaleX = validated;
                OnPropertyChanged();
            }
        }
    }

    public double ScaleY
    {
        get => Model.ScaleY;
        set
        {
            var validated = ValidateScale(value);
            if (Model.ScaleY != validated)
            {
                Console.WriteLine($"[LayoutConfig] ScaleY: {Model.ScaleY} → {validated}");
                Model.ScaleY = validated;
                OnPropertyChanged();
            }
        }
    }

    public double ScaleText
    {
        get => Model.ScaleText;
        set
        {
            var validated = ValidateScale(value);
            if (Model.ScaleText != validated)
            {
                Console.WriteLine($"[LayoutConfig] ScaleText: {Model.ScaleText} → {validated}");
                Model.ScaleText = validated;
                OnPropertyChanged();
            }
        }
    }

    public double DesignWidth
    {
        get => Model.DesignWidth;
        set
        {
            var v = Math.Max(32, Math.Min(value, 16384));
            if (Model.DesignWidth != v)
            {
                Console.WriteLine($"[LayoutConfig] DesignWidth: {Model.DesignWidth} → {v}");
                Model.DesignWidth = v;
                OnPropertyChanged();
            }
        }
    }

    public double DesignHeight
    {
        get => Model.DesignHeight;
        set
        {
            var v = Math.Max(32, Math.Min(value, 16384));
            if (Model.DesignHeight != v)
            {
                Console.WriteLine($"[LayoutConfig] DesignHeight: {Model.DesignHeight} → {v}");
                Model.DesignHeight = v;
                OnPropertyChanged();
            }
        }
    }

    #endregion

    #region Validation

    private double ValidateWindowWidth(double value)
    {
        double v = Math.Max(MinWindowWidth, Math.Min(value, Math.Min(MaxWindowWidth, DisplayWidth)));
        return Math.Round(v / WindowSizeStep) * WindowSizeStep;
    }

    private double ValidateWindowHeight(double value)
    {
        double v = Math.Max(MinWindowHeight, Math.Min(value, Math.Min(MaxWindowHeight, DisplayHeight)));
        return Math.Round(v / WindowSizeStep) * WindowSizeStep;
    }

    private double ValidateScale(double value)
    {
        return Math.Max(MinScale, Math.Min(value, MaxScale));
    }

    #endregion

    #region Change Notification

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    #endregion
}

//===================================================================
// class LayoutConfigVMHelper
//===================================================================

public static class LayoutConfigVMHelper
{
    public static void AttachWindowResizeHandler(LayoutConfigVM layoutVM)
    {
        if (layoutVM == null) return;

        layoutVM.UpdateDisplayInfo();

        layoutVM.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName is nameof(layoutVM.WindowWidth) or nameof(layoutVM.WindowHeight))
            {
                int w = (int)layoutVM.WindowWidth;
                int h = (int)layoutVM.WindowHeight;
                double aspect = w / (double)h;

                if (aspect < LayoutConfigVM.MinAspectRatio || aspect > LayoutConfigVM.MaxAspectRatio)
                {
                    Console.WriteLine($"[LayoutConfig] Aspect ratio out of bounds: {aspect:0.00}");
                }

                App.GetService<App>()?.ResizeMainWindow(w, h);
                Console.WriteLine($"[LayoutConfig] Applied window size: {w} x {h}");
            }
        };
    }
}

/*
===============================================================================
🧩 LayoutConfigVM – Runtime Layout Model + DPI + Validation
===============================================================================

This ViewModel manages layout sizing, design resolution and scale factors
for the tOrder app. It holds validated window dimensions and reacts to changes
with automatic window resizing.

Linked to:
- App.GetService<App>().ResizeMainWindow()
- View scaling via binding (DesignWidth, LayoutScale, etc.)

Also includes helper binding to current display resolution and DPI.

Intended for use as singleton service.
===============================================================================
*/
