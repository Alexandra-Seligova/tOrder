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
        public OverviewByIPCVM VM { get; }

        /// <summary>
        /// Layout configuration settings (e.g., scale, width) provided via DI.
        /// </summary>
        public LayoutConfigVM LayoutConfig => App.GetService<LayoutConfigVM>();

        public OverviewByIPC()
        {
            this.InitializeComponent();

            VM = App.GetService<OverviewByIPCVM>();
            this.DataContext = VM;

            this.Loaded += OverviewByIPC_Loaded;

            Console.WriteLine("[OverviewByIPC Page View] Construct");
        }

        private async void OverviewByIPC_Loaded(object sender, RoutedEventArgs e)
        {
            await VM.LoadedCommand.ExecuteAsync(null);
        }
    }
}