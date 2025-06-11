using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System;
using Microsoft.UI.Xaml.Media;
using Windows.UI;
using Microsoft.UI;
using Microsoft.UI.Text;

namespace tOrder.UI;
public sealed partial class TabSelectorView : UserControl
{
    public event Action<int>? TabChanged;

    public int SelectedIndex
    {
        get => (int)GetValue(SelectedIndexProperty);
        set => SetValue(SelectedIndexProperty, value);
    }

    public static readonly DependencyProperty SelectedIndexProperty =
        DependencyProperty.Register(nameof(SelectedIndex), typeof(int), typeof(TabSelectorView),
            new PropertyMetadata(0, OnSelectedIndexChanged));

    private static void OnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is TabSelectorView view && e.NewValue is int newIndex)
        {
            view.UpdateTabVisuals(newIndex);
            view.TabChanged?.Invoke(newIndex);
        }
    }

    public TabSelectorView()
    {
        this.InitializeComponent();
        UpdateTabVisuals(0);
    }

    private void Tab_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && int.TryParse((string)btn.Tag, out int index))
        {
            SelectedIndex = index;
        }
    }

    private void UpdateTabVisuals(int activeIndex)
    {
        for (int i = 0; i < 8; i++)
        {
            var stack = (StackPanel)TabRoot.Children[i];
            var button = (Button)stack.Children[0];
            var border = (Border)stack.Children[1];
            if (i == activeIndex)
            {
                button.Foreground = (SolidColorBrush)Resources["TabActiveBrush"];
                button.FontWeight = FontWeights.Bold;    // opraveno zde!
                border.Background = (SolidColorBrush)Resources["TabActiveBrush"];
            }
            else
            {
                button.Foreground = (SolidColorBrush)Resources["TabInactiveBrush"];
                button.FontWeight = FontWeights.Normal;  // opraveno zde!
                border.Background = new SolidColorBrush(Colors.Transparent);
            }
        }
    }
}
