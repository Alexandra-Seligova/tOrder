/***************************************************************************
 *
 * tOrder Application
 *
 * Company      : SPC solutions s.r.o.
 * Author       : Alexandra Seligová
 *
 * Description  :
 * - Code-behind for the OverviewByIPC page.
 * - Binds the OverviewByIPCViewModel using dependency injection.
 *
 ***************************************************************************/

namespace tOrder.UI
{
    using System;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    using tOrder.UI;

    /// <summary>
    /// Represents a page displaying an overview of IPC machines.
    /// </summary>
    public sealed partial class OverviewByIPC : Page
    {
        public OverviewByIPCVM ViewModel { get; }

        public OverviewByIPC()
        {
            this.InitializeComponent();

            ViewModel = App.GetService<OverviewByIPCVM>();
            this.DataContext = ViewModel;

            this.Loaded += OverviewByIPC_Loaded;

            Console.WriteLine("[OverviewByIPC Page View] Construct");
        }

        private async void OverviewByIPC_Loaded(object sender, RoutedEventArgs e)
        {
            await ViewModel.LoadedCommand.ExecuteAsync(null);
        }
    }
}