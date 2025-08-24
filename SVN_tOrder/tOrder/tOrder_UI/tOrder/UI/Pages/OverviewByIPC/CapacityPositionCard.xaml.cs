/***************************************************************************
 *
 * tOrder Application
 *
 * Company      :  solutions s.r.o.
 * Author       : Alexandra Seligová
 *
 * Description  :
 * - Code-behind for the MachineCard control.
 * - Binds the MachineCardViewModel using dependency injection.
 *
 ***************************************************************************/

namespace tOrder.UI
{
    using System;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;
    /// <summary>
    /// Represents a visual card for displaying machine and production status information.
    /// </summary>
    public sealed partial class CapacityPositionCard : UserControl
    {
        /// <summary>
        /// ViewModel associated with this card instance.
        /// </summary>
        public CapacityPositionCardVM? VM { get; private set; }

        /// <summary>
        /// Default constructor used by XAML.
        /// ViewModel is expected to be assigned via DataContext externally.
        /// </summary>
        public CapacityPositionCard()
        {
            this.InitializeComponent();
            this.DataContextChanged += CapacityPositionCard_DataContextChanged;
            Console.WriteLine("[CapacityPositionCard Card View] Construct");
        }

        /// <summary>
        /// Factory-based constructor to initialize ViewModel and assign DataContext.
        /// </summary>
        public CapacityPositionCard(CapacityUnitM unit, CapacityPositionM position)
        {
            this.InitializeComponent();

            var factory = App.GetService<ICapacityCardVMFactory>();
            VM = factory.Create(unit, position);
            this.DataContext = VM;
            Console.WriteLine("[CapacityPositionCard Card View] Construct unit+position");
        }

        /// <summary>
        /// Handler to sync DataContext with strong-typed ViewModel.
        /// Called automatically when DataContext changes.
        /// </summary>
        private void CapacityPositionCard_DataContextChanged(FrameworkElement sender, DataContextChangedEventArgs args)
        {
            VM = args.NewValue as CapacityPositionCardVM;
        }

        /// <summary>
        /// Optional manual popup toggle handler for footer icon, if used directly in XAML.
        /// </summary>
        private void OnShowPopupClick(object sender, RoutedEventArgs e)
        {
            if (VM is not null)
            {
                VM.ShowOperatorPopup = !VM.ShowOperatorPopup;
            }
        }


    }
}
