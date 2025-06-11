namespace tOrder.UI;

    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using System;
    using System.Windows.Input;

    public sealed partial class CardFooterC : UserControl
    {
        public CardFooterC()
        {
            this.InitializeComponent();
            Console.WriteLine("[CardFooterC View] Construct");
        }

        // Lev� text (nap�. jm�no oper�tora)
        public static readonly DependencyProperty FooterLeftTextProperty =
            DependencyProperty.Register(nameof(FooterLeftText), typeof(string), typeof(CardFooterC), new PropertyMetadata(string.Empty));

        public string FooterLeftText
        {
            get => (string)GetValue(FooterLeftTextProperty);
            set => SetValue(FooterLeftTextProperty, value);
        }

        // Prost�edn� text (nap�. ID oper�tora nebo ��ta�)
        public static readonly DependencyProperty FooterCenterTextProperty =
            DependencyProperty.Register(nameof(FooterCenterText), typeof(string), typeof(CardFooterC), new PropertyMetadata(string.Empty));

        public string FooterCenterText
        {
            get => (string)GetValue(FooterCenterTextProperty);
            set => SetValue(FooterCenterTextProperty, value);
        }

        // Prav� text (nap�. v�ce info nebo button/ikona)
        public static readonly DependencyProperty FooterRightTextProperty =
            DependencyProperty.Register(nameof(FooterRightText), typeof(string), typeof(CardFooterC), new PropertyMetadata(string.Empty));

        public string FooterRightText
        {
            get => (string)GetValue(FooterRightTextProperty);
            set => SetValue(FooterRightTextProperty, value);
        }

        // Tla��tko vpravo (command)
        public static readonly DependencyProperty FooterButtonCommandProperty =
            DependencyProperty.Register(nameof(FooterButtonCommand), typeof(ICommand), typeof(CardFooterC), new PropertyMetadata(null));

        public ICommand FooterButtonCommand
        {
            get => (ICommand)GetValue(FooterButtonCommandProperty);
            set => SetValue(FooterButtonCommandProperty, value);
        }
    }
