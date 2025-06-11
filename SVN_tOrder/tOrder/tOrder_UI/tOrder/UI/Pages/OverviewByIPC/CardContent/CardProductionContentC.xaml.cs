

namespace tOrder.UI;

using System;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

public sealed partial class CardProductionContentC : UserControl
{
    public CardProductionContentC()
    {
        this.InitializeComponent();
        Console.WriteLine("[CardProductionContentC View] Construct");
    }

    // Status: Název a hodnota
    public static readonly DependencyProperty CardContentTextStatusNameProperty =
        DependencyProperty.Register(nameof(CardContentTextStatusName), typeof(string), typeof(CardProductionContentC), new PropertyMetadata(string.Empty));

    public string CardContentTextStatusName
    {
        get => (string)GetValue(CardContentTextStatusNameProperty);
        set => SetValue(CardContentTextStatusNameProperty, value);
    }

    public static readonly DependencyProperty CardContentTextStatusTypeProperty =
        DependencyProperty.Register(nameof(CardContentTextStatusType), typeof(string), typeof(CardProductionContentC), new PropertyMetadata(string.Empty));

    public string CardContentTextStatusType
    {
        get => (string)GetValue(CardContentTextStatusTypeProperty);
        set => SetValue(CardContentTextStatusTypeProperty, value);
    }

    // Info Rows 0–3
    public static readonly DependencyProperty CardContentTextName0Property =
        DependencyProperty.Register(nameof(CardContentTextName0), typeof(string), typeof(CardProductionContentC), new PropertyMetadata(string.Empty));

    public string CardContentTextName0
    {
        get => (string)GetValue(CardContentTextName0Property);
        set => SetValue(CardContentTextName0Property, value);
    }

    public static readonly DependencyProperty CardContentTextValue0Property =
        DependencyProperty.Register(nameof(CardContentTextValue0), typeof(string), typeof(CardProductionContentC), new PropertyMetadata(string.Empty));

    public string CardContentTextValue0
    {
        get => (string)GetValue(CardContentTextValue0Property);
        set => SetValue(CardContentTextValue0Property, value);
    }

    public static readonly DependencyProperty CardContentTextName1Property =
        DependencyProperty.Register(nameof(CardContentTextName1), typeof(string), typeof(CardProductionContentC), new PropertyMetadata(string.Empty));

    public string CardContentTextName1
    {
        get => (string)GetValue(CardContentTextName1Property);
        set => SetValue(CardContentTextName1Property, value);
    }

    public static readonly DependencyProperty CardContentTextValue1Property =
        DependencyProperty.Register(nameof(CardContentTextValue1), typeof(string), typeof(CardProductionContentC), new PropertyMetadata(string.Empty));

    public string CardContentTextValue1
    {
        get => (string)GetValue(CardContentTextValue1Property);
        set => SetValue(CardContentTextValue1Property, value);
    }

    public static readonly DependencyProperty CardContentTextName2Property =
        DependencyProperty.Register(nameof(CardContentTextName2), typeof(string), typeof(CardProductionContentC), new PropertyMetadata(string.Empty));

    public string CardContentTextName2
    {
        get => (string)GetValue(CardContentTextName2Property);
        set => SetValue(CardContentTextName2Property, value);
    }

    public static readonly DependencyProperty CardContentTextValue2Property =
        DependencyProperty.Register(nameof(CardContentTextValue2), typeof(string), typeof(CardProductionContentC), new PropertyMetadata(string.Empty));

    public string CardContentTextValue2
    {
        get => (string)GetValue(CardContentTextValue2Property);
        set => SetValue(CardContentTextValue2Property, value);
    }

    public static readonly DependencyProperty CardContentTextName3Property =
        DependencyProperty.Register(nameof(CardContentTextName3), typeof(string), typeof(CardProductionContentC), new PropertyMetadata(string.Empty));

    public string CardContentTextName3
    {
        get => (string)GetValue(CardContentTextName3Property);
        set => SetValue(CardContentTextName3Property, value);
    }

    public static readonly DependencyProperty CardContentTextValue3Property =
        DependencyProperty.Register(nameof(CardContentTextValue3), typeof(string), typeof(CardProductionContentC), new PropertyMetadata(string.Empty));

    public string CardContentTextValue3
    {
        get => (string)GetValue(CardContentTextValue3Property);
        set => SetValue(CardContentTextValue3Property, value);
    }
    // LocalState property pro barvení stavu
    public static readonly DependencyProperty LocalStateProperty =
        DependencyProperty.Register(
            nameof(LocalState),
            typeof(CapacityState),
            typeof(CardProductionContentC),
            new PropertyMetadata(CapacityState.Off));

    public CapacityState LocalState
    {
        get => (CapacityState)GetValue(LocalStateProperty);
        set => SetValue(LocalStateProperty, value);
    }

    public static readonly DependencyProperty StatusBrushProperty =
DependencyProperty.Register(
    nameof(StatusBrush),
    typeof(SolidColorBrush),
    typeof(CardProductionContentC),
    new PropertyMetadata(new SolidColorBrush(Colors.Red))
);

    public SolidColorBrush StatusBrush
    {
        get => (SolidColorBrush)GetValue(StatusBrushProperty);
        set => SetValue(StatusBrushProperty, value);
    }

}