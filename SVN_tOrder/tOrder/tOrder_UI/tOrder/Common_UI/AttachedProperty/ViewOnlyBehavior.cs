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
                textBox.Background = new SolidColorBrush(Color.FromArgb(255, 187, 187, 187)); // #BBBBBB
                textBox.FontSize = 16; // ⬅️ Zvětšení textu

            }
            else
            {
                textBox.ClearValue(TextBox.IsReadOnlyProperty);
                textBox.ClearValue(TextBox.BackgroundProperty);
                textBox.ClearValue(TextBox.FontSizeProperty);
            }
        }
    }

}
