namespace tOrder.UI;

using System;
using System.Windows.Input;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

public sealed partial class CardHeadC : UserControl
{
    public CardHeadC()
    {
        this.InitializeComponent();
        Console.WriteLine("[CardHeadC View] Construct");
    }

    // Hlavn� text (nap�. UnitCode)
    public static readonly DependencyProperty CardHeadTextMainProperty =
        DependencyProperty.Register(nameof(CardHeadTextMain), typeof(string), typeof(CardHeadC), new PropertyMetadata(string.Empty));

    // Hlavn� barva pozad� hlavi�ky
    public static readonly DependencyProperty CardHeadMainBrushProperty =
        DependencyProperty.Register(nameof(CardHeadMainBrush), typeof(Brush), typeof(CardHeadC), new PropertyMetadata(new SolidColorBrush(Colors.Green)));

    public Brush CardHeadMainBrush
    {
        get => (Brush)GetValue(CardHeadMainBrushProperty);
        set => SetValue(CardHeadMainBrushProperty, value);
    }

    // Barva pozad� ��sla (indexu)
    public static readonly DependencyProperty CardHeadNumberBrushProperty =
        DependencyProperty.Register(nameof(CardHeadNumberBrush), typeof(Brush), typeof(CardHeadC), new PropertyMetadata(new SolidColorBrush(Colors.DarkGreen)));

    public Brush CardHeadNumberBrush
    {
        get => (Brush)GetValue(CardHeadNumberBrushProperty);
        set => SetValue(CardHeadNumberBrushProperty, value);
    }
    public string CardHeadTextMain
    {
        get => (string)GetValue(CardHeadTextMainProperty);
        set => SetValue(CardHeadTextMainProperty, value);
    }

    // Sekund�rn� text (nap�. UnitId)
    public static readonly DependencyProperty CardHeadTextSecondaryProperty =
        DependencyProperty.Register(nameof(CardHeadTextSecondary), typeof(string), typeof(CardHeadC), new PropertyMetadata(string.Empty));

    public string CardHeadTextSecondary
    {
        get => (string)GetValue(CardHeadTextSecondaryProperty);
        set => SetValue(CardHeadTextSecondaryProperty, value);
    }

    // ��slo vpravo (nap�. Index)
    public static readonly DependencyProperty CardHeadNumberProperty =
        DependencyProperty.Register(nameof(CardHeadNumber), typeof(string), typeof(CardHeadC), new PropertyMetadata(string.Empty));

    public string CardHeadNumber
    {
        get => (string)GetValue(CardHeadNumberProperty);
        set => SetValue(CardHeadNumberProperty, value);
    }

    // Tla��tko (nap�. ToggleSectionsCommand)
    public static readonly DependencyProperty CardHeadButtonProperty =
        DependencyProperty.Register(nameof(CardHeadButton), typeof(ICommand), typeof(CardHeadC), new PropertyMetadata(null));

    public ICommand CardHeadButton
    {
        get => (ICommand)GetValue(CardHeadButtonProperty);
        set => SetValue(CardHeadButtonProperty, value);
    }
}
