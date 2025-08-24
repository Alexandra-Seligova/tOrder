//===================================================================
// $Workfile:: MainLayout.xaml.cs                                  $
// $Author:: Alexandra_Seligova                                    $
// $Revision:: 1                                                   $
// $Date:: 2025-07-24 23:55:00 +0200 (čt, 24 čvc 2025)             $
//===================================================================
// Description:  - tOrder
//     Code-behind for the MainLayout control. Hosts the NavigationView,
//     manages page navigation, and updates UI shell state.
//===================================================================

namespace tOrder.Shell;

using CommunityToolkit.Mvvm.Messaging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using tOrder.Common;
using tOrder.UI;

public sealed partial class MainLayout : UserControl
{
    //-----------------------------------------------------------
    #region Fields & Properties
    //-----------------------------------------------------------

    /// <summary>
    /// ViewModel instance for the MainLayout view.
    /// </summary>
    public MainLayoutVM VM { get; }

    /// <summary>
    /// Layout configuration settings (e.g., scale, width) provided via DI.
    /// </summary>
    public LayoutConfigVM LayoutConfig => App.GetService<LayoutConfigVM>();

    /// <summary>
    /// Mapping between navigation tags and corresponding page types.
    /// </summary>
    private static readonly Dictionary<string, Type> PageMap = new()
    {
        { "OverviewByIPC", typeof(OverviewByIPC) },
        { "CapacityUnitDashboard", typeof(CapacityUnitDashboard) },

        // Add more mappings as needed:
        // { "SchichtAnfang", typeof(SchichtAnfang) },
        // { "SchichtEnde", typeof(SchichtEnde) },
        // { "Rusten", typeof(Rusten) },
        // { "InfoSystem", typeof(InfoSystem) },
    };

    #endregion // Fields & Properties

    //-----------------------------------------------------------
    #region Constructor
    //-----------------------------------------------------------

    /// <summary>
    /// Initializes the MainLayout control and sets up messaging.
    /// </summary>
    public MainLayout()
    {
        this.InitializeComponent();

        VM = App.GetService<MainLayoutVM>();
        this.DataContext = VM;

        // Listen to UI messages to toggle the navigation pane
        WeakReferenceMessenger.Default.Register<UiMessage>(this, (r, m) =>
        {
            if (m.Value == "ToggleMenu")
                NavView.IsPaneOpen = !NavView.IsPaneOpen;
        });

        Console.WriteLine("[MainLayout View] Constructed");
    }

    #endregion // Constructor

    //-----------------------------------------------------------
    #region Navigation Handling
    //-----------------------------------------------------------

    /// <summary>
    /// Handles selection changes in the NavigationView.
    /// Navigates to the selected page and updates the top bar header.
    /// </summary>
    private async void NavigationView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
    {
        Console.WriteLine("[MainLayout View] NavigationView_SelectionChanged");

        if (args.SelectedItem is not NavigationViewItem selectedItem)
            return;

        string? tag = selectedItem.Tag?.ToString();
        string? header = selectedItem.Content?.ToString();

        if (string.IsNullOrWhiteSpace(tag) || string.IsNullOrWhiteSpace(header))
            return;

        // Update TopBar heading
        var topBarVM = App.GetService<TopBarVM>();
        topBarVM.Heading = header;

        // Navigate if page mapping exists
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
            // Show fallback dialog if page not found
            var dialog = new ContentDialog
            {
                Title = "Page Not Found",
                Content = $"No page found for tag '{tag}'.",
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };

            await dialog.ShowAsync();
        }
    }

    #endregion // Navigation Handling
}
