
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace tOrder.UI
{
    public sealed partial class DashboardContentPortC : UserControl
    {
        public DashboardContentPortC()
        {
            this.InitializeComponent();
        }
        public string ButtonText
        {
            get => (string)GetValue(ButtonTextProperty);
            set => SetValue(ButtonTextProperty, value);
        }

        public static readonly DependencyProperty ButtonTextProperty =
            DependencyProperty.Register(nameof(ButtonText), typeof(string), typeof(DashboardContentPortC), new PropertyMetadata("Behälter öffnen zur\nBearbeitung"));

        public string HeaderText
        {
            get => (string)GetValue(HeaderTextProperty);
            set => SetValue(HeaderTextProperty, value);
        }

        public static readonly DependencyProperty HeaderTextProperty =
            DependencyProperty.Register(nameof(HeaderText), typeof(string), typeof(DashboardContentPortC), new PropertyMetadata("Eingang Behälter"));

        public string OutputText
        {
            get => (string)GetValue(OutputTextProperty);
            set => SetValue(OutputTextProperty, value);
        }

        public static readonly DependencyProperty OutputTextProperty =
            DependencyProperty.Register(nameof(OutputText), typeof(string), typeof(DashboardContentPortC), new PropertyMetadata("Ausgang 1."));
    }

}
