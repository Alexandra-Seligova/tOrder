using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.UI.Windowing;
using Windows.Graphics;

namespace tOrder
{
    /// <summary>
    /// Model pro layout konfiguraci – pouze data.
    /// </summary>
    public class LayoutConfigModel
    {
        public double WindowWidth { get; set; } = 1280;   // Aktuální šířka okna aplikace
        public double WindowHeight { get; set; } = 960;   // Aktuální výška okna aplikace
        public double Scale { get; set; } = 1;            // Celkové měřítko layoutu (globální scale)
        public double ScaleX { get; set; } = 1;           // Nezávislé horizontální škálování
        public double ScaleY { get; set; } = 1;           // Nezávislé vertikální škálování
        public double ScaleText { get; set; } = 1;        // Škálování textu
        public double DesignWidth { get; set; } = 1024;   // Návrhová šířka (výchozí šířka layoutu)
        public double DesignHeight { get; set; } = 768;   // Návrhová výška (výchozí výška layoutu)
    }

    /// <summary>
    /// ViewModel – proxy pro LayoutConfigModel, umožňuje binding, validace a výpis změn.
    /// Zahrnuje aktuální hodnoty displeje (DisplayWidth, DisplayHeight, Dpi) a validuje vstupy.
    /// </summary>
    public class LayoutConfigVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public LayoutConfigModel Model { get; }

        // Validace – konstanty
        public const double MinWindowWidth = 640;
        public const double MaxWindowWidth = 8192;
        public const double MinWindowHeight = 480;
        public const double MaxWindowHeight = 4320;
        public const double MinAspectRatio = 1.2;      // např. 6:5
        public const double MaxAspectRatio = 2.4;      // např. 24:10
        public const double MinScale = 0.5;
        public const double MaxScale = 4.0;
        public const int WindowSizeStep = 4;           // lze měnit pouze po násobcích 4 px

        // Zobrazení – pouze pro čtení
        public int DisplayWidth { get; private set; }
        public int DisplayHeight { get; private set; }
        public double DisplayDpi { get; private set; }

        public LayoutConfigVM(LayoutConfigModel model)
        {
            Model = model ?? throw new ArgumentNullException(nameof(model));
            UpdateDisplayInfo();
        }

        // Aktualizace informací o obrazovce a DPI
        public void UpdateDisplayInfo()
        {
            // Získání rozměrů primární obrazovky
            var area = DisplayArea.GetFromPoint(new PointInt32(0, 0), DisplayAreaFallback.Primary).WorkArea;
            DisplayWidth = area.Width;
            DisplayHeight = area.Height;
            // DPI info - UWP/WinUI 3: často fixní 96, jinak by bylo potřeba získat přes Win32 nebo Composition API
            DisplayDpi = 96.0; // Pro většinu případů, pro reálné DPI viz pokročilejší API
            OnPropertyChanged(nameof(DisplayWidth));
            OnPropertyChanged(nameof(DisplayHeight));
            OnPropertyChanged(nameof(DisplayDpi));
        }

        // ----------------
        //  Vlastnosti s validací a výpisem změn
        // ----------------

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

        // -----------------------
        //  Validace
        // -----------------------

        private double ValidateWindowWidth(double value)
        {
            // Ořízneme na povolené hranice, zaokrouhlíme na násobek WindowSizeStep, a omezíme na aktuální displej
            double v = Math.Max(MinWindowWidth, Math.Min(value, Math.Min(MaxWindowWidth, DisplayWidth)));
            v = Math.Round(v / WindowSizeStep) * WindowSizeStep;
            return v;
        }

        private double ValidateWindowHeight(double value)
        {
            double v = Math.Max(MinWindowHeight, Math.Min(value, Math.Min(MaxWindowHeight, DisplayHeight)));
            v = Math.Round(v / WindowSizeStep) * WindowSizeStep;
            return v;
        }

        private double ValidateScale(double value)
        {
            return Math.Max(MinScale, Math.Min(value, MaxScale));
        }

        // -----------------------
        //  PropertyChanged trigger
        // -----------------------

        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }

    // ------------------------------
    // Pomocná metoda pro napojení do MainWindow (validace a změna rozměrů)
    // ------------------------------
    public static class LayoutConfigVMHelper
    {
        /// <summary>
        /// Propojí změny velikosti okna v LayoutConfigVM s App.ResizeMainWindow a plnou validací.
        /// </summary>
        public static void AttachWindowResizeHandler(LayoutConfigVM layoutVM)
        {
            if (layoutVM == null) return;

            layoutVM.UpdateDisplayInfo();

            layoutVM.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(layoutVM.WindowWidth) || e.PropertyName == nameof(layoutVM.WindowHeight))
                {
                    int w = (int)layoutVM.WindowWidth;
                    int h = (int)layoutVM.WindowHeight;

                    // Možno rozšířit i o kontrolu aspect ratio
                    double aspect = w / (double)h;
                    if (aspect < LayoutConfigVM.MinAspectRatio || aspect > LayoutConfigVM.MaxAspectRatio)
                    {
                        Console.WriteLine($"[LayoutConfig] Aspect ratio out of bounds: {aspect:0.00}");
                        // Můžeš zde přidat další korekci/hlášku/potlačení
                    }

                    // Volání App pouze pokud je rozumné okno
                    App.GetService<App>()?.ResizeMainWindow(w, h);
                    Console.WriteLine($"[LayoutConfig] Applied window size: {w} x {h}");
                }
            };
        }
    }
}
