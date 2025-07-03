/***************************************************************************
 *
 * tOrder Application
 *
 * Company      : SPC solutions s.r.o.
 * Author       : Alexandra Seligová
 *
 * Description  :
 * - Code-behind for the Layout control.
 * - Hosts the NavigationView and manages page navigation logic.
 *
 ***************************************************************************/

namespace tOrder.Shell;

using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using tOrder.Common;
using tOrder.UI;

public sealed partial class MainLayout : UserControl
{
    public MainLayoutVM ViewModel { get; }

    public MainLayout()
    {
        this.InitializeComponent();
        ViewModel = App.GetService<MainLayoutVM>();
        this.DataContext = ViewModel;


        WeakReferenceMessenger.Default.Register<UiMessage>(this, (r, m) =>
        {
            if (m.Value == "ToggleMenu")
                NavView.IsPaneOpen = !NavView.IsPaneOpen;
        });

        Console.WriteLine("[MainLayout View] Construct");
    }


    // Mapa názvů (Tag) na odpovídající stránky
    private static readonly Dictionary<string, Type> PageMap = new()
        {
            { "OverviewByIPC", typeof(OverviewByIPC) },
            { "CapacityUnitDashboard", typeof(CapacityUnitDashboard) },
        
           // { "SchichtAnfang", typeof(SchichtAnfang) },
           // { "SchichtEnde", typeof(SchichtEnde) },
           // { "Rusten", typeof(Rusten) },
           // { "Korektur", typeof(Korektur) },
           // { "InfoSystem", typeof(InfoSystem) }
            // Přidej další tagy a typy podle potřeby
        };

    private async void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        Console.WriteLine("[MainLayout View] NavigationView_SelectionChanged");
        if (args.SelectedItem is NavigationViewItem selectedItem)
        {
            string? tag = selectedItem.Tag?.ToString();
            string? header = selectedItem.Content?.ToString();

            if (!string.IsNullOrWhiteSpace(tag) && !string.IsNullOrWhiteSpace(header))
            {
                var TopBarViewModel = App.GetService<TopBarVM>();
                TopBarViewModel.Heading = header;

                if (PageMap.TryGetValue(tag, out var pageType))
                {
                    try
                    {
                        ContentFrame.Navigate(pageType);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Navigace selhala: {ex.Message}\n{ex.StackTrace}");
                    }
                }
                else
                {
                    var dialog = new ContentDialog
                    {
                        Title = "Page Not Found",
                        Content = $"Stránka pro tag '{tag}' nebyla nalezena v mapě.",
                        CloseButtonText = "OK",
                        XamlRoot = this.XamlRoot
                    };

                    await dialog.ShowAsync();
                }
            }
        }
    }
}

