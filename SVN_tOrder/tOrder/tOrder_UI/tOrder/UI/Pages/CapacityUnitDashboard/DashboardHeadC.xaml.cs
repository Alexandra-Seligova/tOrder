using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Windows.UI;

namespace tOrder.UI
{
    public sealed partial class DashboardHeadC : UserControl
    {
        private int _statusIndex = 0;

        // Kombinace stavů
        private readonly (Color Color, string Glyph, string Text)[] _statusStates = new[]
        {
            (Colors.LimeGreen, "▶", "Läuft"),
            (Colors.Orange, "■", "Einstellung"),
            (Colors.Red, "■", "Frei"),
            (Colors.Red, "■", "Gestoppt"),
            (Color.FromArgb(255, 0, 255, 255), "⏸", "Pause") // tyrkys = ARGB(255, 0, 255, 255)
        };

        public DashboardHeadC()
        {
            this.InitializeComponent();
        }

        private void StatusIcon_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (sender is Grid statusGrid && statusGrid.FindName("StatusGlyph") is FontIcon icon && statusGrid.FindName("StatusText") is TextBlock label)
            {
                _statusIndex = (_statusIndex + 1) % _statusStates.Length;

                var (color, glyph, text) = _statusStates[_statusIndex];

                statusGrid.Background = new SolidColorBrush(color);
                icon.Glyph = glyph;
                label.Text = text;
            }
        }
    }
}
