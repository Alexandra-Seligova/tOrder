

namespace tOrder.UI;
using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
public sealed partial class CardContentSwitcherC : UserControl
{
    public CardContentSwitcherVM? VM => DataContext as CardContentSwitcherVM;

    public CardContentSwitcherC()
    {
        this.InitializeComponent();
        this.Loaded += CardContentSwitcherC_Loaded;
        Console.WriteLine("[CardContentSwitcherC View] Construct");
    }

    private void CardContentSwitcherC_Loaded(object sender, RoutedEventArgs e)
    {
        Console.WriteLine("[CardContentSwitcherC View] CardContentSwitcherC_Loaded");
        if (VM != null)
        {
            // Na zmìnu sekce nastaví visibility jednotlivých controlù
            VM.ActiveSectionChanged += (_, section) => SetContentVisibility(section);
            SetContentVisibility(VM.ActiveSection); // nastavení pøi startu
        }
    }

    private void SetContentVisibility(SectionType section)
    {
        CardProductionContentC.Visibility = section == SectionType.Production ? Visibility.Visible : Visibility.Collapsed;
        CardMenuContentC.Visibility = section == SectionType.Menu ? Visibility.Visible : Visibility.Collapsed;
        CardOeeContentC.Visibility = section == SectionType.OEE ? Visibility.Visible : Visibility.Collapsed;
    }
}
