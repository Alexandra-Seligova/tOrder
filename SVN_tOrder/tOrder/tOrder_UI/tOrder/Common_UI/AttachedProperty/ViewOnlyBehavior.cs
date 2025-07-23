using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Windows.UI;

namespace tOrder.Common
{
    public static class ViewOnlyBehavior
    {
        public static readonly DependencyProperty IsViewOnlyProperty =
            DependencyProperty.RegisterAttached(
                "IsViewOnly",
                typeof(bool),
                typeof(ViewOnlyBehavior),
                new PropertyMetadata(false, OnIsViewOnlyChanged));

        public static void SetIsViewOnly(TextBox element, bool value)
            => element.SetValue(IsViewOnlyProperty, value);

        public static bool GetIsViewOnly(TextBox element)
            => (bool)element.GetValue(IsViewOnlyProperty);

        private static void OnIsViewOnlyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not TextBox textBox) return;

            bool isViewOnly = (bool)e.NewValue;

            if (isViewOnly)
            {
                textBox.IsReadOnly = true;
                textBox.IsTabStop = false;
                textBox.IsHitTestVisible = false;

                textBox.BorderThickness = new Thickness(0);
                textBox.Background = new SolidColorBrush(Color.FromArgb(255, 187, 187, 187)); // #BBBBBB
                textBox.Foreground = new SolidColorBrush(Colors.Black);
                textBox.Padding = new Thickness(2);

                textBox.TextAlignment = TextAlignment.Right;
                textBox.VerticalContentAlignment = VerticalAlignment.Center;
                textBox.TextWrapping = TextWrapping.NoWrap;

                var transparent = new SolidColorBrush(Colors.Transparent);
                textBox.FocusVisualPrimaryBrush = transparent;
                textBox.FocusVisualSecondaryBrush = transparent;
                textBox.SelectionHighlightColorWhenNotFocused = transparent;
            }
        }
    }
}
