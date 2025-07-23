

/***************************************************************************
 *
 * tOrder Application
 *
 * Company      : SPC solutions s.r.o.
 * Author       : Alexandra Seligová
 *
 * Description  :
 * - Code-behind for the MainLayout control.
 * - Hosts the NavigationView (sidebar menu) and manages page navigation.
 * - Handles ToggleMenu messages for collapsing/expanding the sidebar.
 * - Updates TopBarVM header during navigation.
 *
 ***************************************************************************/

namespace tOrder.Shell;

using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Input;
using System;
using System.Collections.Generic;
using tOrder.Common;
using tOrder.UI;

public sealed partial class MainLayout : UserControl
{
    /// <summary>ViewModel for the MainLayout control.</summary>
    public MainLayoutVM ViewModel { get; }

    public MainLayout()
    {
        this.InitializeComponent();

        ViewModel = App.GetService<MainLayoutVM>();
        this.DataContext = ViewModel;

        // Toggle menu visibility via UiMessage (used by TopBar)
        WeakReferenceMessenger.Default.Register<UiMessage>(this, (r, m) =>
        {
            if (m.Value == "ToggleMenu")
                NavView.IsPaneOpen = !NavView.IsPaneOpen;
        });

        Console.WriteLine("[MainLayout View] Constructed");
    }

    /// <summary>
    /// Mapping of NavigationView item Tags to associated Pages.
    /// </summary>
    private static readonly Dictionary<string, Type> PageMap = new()
    {
        { "OverviewByIPC", typeof(OverviewByIPC) },
        { "CapacityUnitDashboard", typeof(CapacityUnitDashboard) },
        // Add more tag-page mappings as needed

      // { "SchichtAnfang", typeof(SchichtAnfang) },
      // { "SchichtEnde", typeof(SchichtEnde) },
      // { "Rusten", typeof(Rusten) },
      // { "Korektur", typeof(Korektur) },
      // { "InfoSystem", typeof(InfoSystem) }
       // Přidej další tagy a typy podle potřeby
    };

    /// <summary>
    /// Handles NavigationView item selection and performs page navigation.
    /// Also updates TopBar header text.
    /// </summary>
    private async void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        Console.WriteLine("[MainLayout View] NavigationView_SelectionChanged");

        if (args.SelectedItem is NavigationViewItem selectedItem)
        {
            string? tag = selectedItem.Tag?.ToString();
            string? header = selectedItem.Content?.ToString();

            if (!string.IsNullOrWhiteSpace(tag) && !string.IsNullOrWhiteSpace(header))
            {
                var topBarVM = App.GetService<TopBarVM>();
                topBarVM.Heading = header;

                if (PageMap.TryGetValue(tag, out var pageType))
                {
                    try
                    {
                        ContentFrame.Navigate(pageType);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[MainLayout] Navigation failed: {ex.Message}\n{ex.StackTrace}");
                    }
                }
                else
                {
                    var dialog = new ContentDialog
                    {
                        Title = "Page Not Found",
                        Content = $"Stránka pro tag '{tag}' nebyla nalezena.",
                        CloseButtonText = "OK",
                        XamlRoot = this.XamlRoot
                    };

                    await dialog.ShowAsync();
                }
            }
        }
    }
}
