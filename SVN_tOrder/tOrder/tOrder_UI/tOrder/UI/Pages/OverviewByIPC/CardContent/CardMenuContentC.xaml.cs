
/***************************************************************************
 *
 * tOrder Application
 *
 * Company      :  solutions s.r.o.
 * Author       : Alexandra Seligová
 *
 * Description  :
 * - Code-behind for FloatingMenu control.
 * - Binds FloatingMenuViewModel via x:Bind.
 *
 ***************************************************************************/
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace tOrder.UI
{
    internal class MenuItem(string text, string page)
    {
        public string Text { get; set; } = text;
        public string Page { get; set; } = page;
    }
    public sealed partial class CardMenuContentC : UserControl
    {
        private MenuItem[] data = new MenuItem[9] {
        new MenuItem("Einstellteile", ""),
        new MenuItem("Rusten", ""),
        new MenuItem("Muster", ""),
        new MenuItem("ETF", ""),
        new MenuItem("WZ-BlattWechsel", ""),
        new MenuItem("WZ Wechsel", ""),
        new MenuItem("WZ Historie", ""),
        new MenuItem("Auftrags-verwaltung", ""),
        new MenuItem("Sample", ""),
    };
        // DependencyProperty pro MenuItems (bindovatelný v parent XAML)
        public ObservableCollection<MenuItemM> MenuItems
        {
            get => (ObservableCollection<MenuItemM>)GetValue(MenuItemsProperty);
            set => SetValue(MenuItemsProperty, value);
        }

        public static readonly DependencyProperty MenuItemsProperty =
            DependencyProperty.Register(nameof(MenuItems),
                typeof(ObservableCollection<MenuItemM>),
                typeof(CardMenuContentC),
                new PropertyMetadata(new ObservableCollection<MenuItemM>()));

        public CardMenuContentC()
        {
            this.InitializeComponent();
            Console.WriteLine("[CardMenuContentC View] Construct");
        }
        // Kliknutí na tlaèítko v menu
        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("[CardMenuContentC View] MenuButton_Click");
            if (sender is Button button && button.DataContext is MenuItemM item)
            {
                // Mùžeš zde spustit akci, event, cokoliv
                System.Diagnostics.Debug.WriteLine($"FloatingMenu: kliknuto na {item.Text} (Page={item.Page})");
                // Mùžeš pøípadnì vyvolat vlastní event nebo zavolat Command z parentu, atd.
            }
        }
        public void Toggle()
        {
            if (Visibility == Visibility.Collapsed)
            {
                Visibility = Visibility.Visible;
            }
            else
            {
                Visibility = Visibility.Collapsed;
            }
        }
    }
}
