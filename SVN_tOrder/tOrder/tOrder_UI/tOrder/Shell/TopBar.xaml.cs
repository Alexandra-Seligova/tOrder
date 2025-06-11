/***************************************************************************
 *
 * tOrder Application
 *
 * Company      : SPC solutions s.r.o.
 * Author       : Alexandra Seligová
 *
 * Description  :
 * - Code-behind for the TopBar UserControl.
 * - Connects the XAML UI to the TopBarViewModel.
 *
 ***************************************************************************/

namespace tOrder.Shell
{
    using System;
    using Microsoft.UI.Xaml.Controls;

    using tOrder.Shell;

    /// <summary>
    /// Code-behind for the TopBar control.
    /// </summary>
    public sealed partial class TopBar : UserControl
    {
        /// <summary>Gets the ViewModel for this control.</summary>
        public TopBarVM VM
        {
            get;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TopBar"/> class.
        /// </summary>
        public TopBar()
        {
            this.InitializeComponent();
            // Získání ViewModelu přes App.GetService<T>() (DI)
            VM = App.GetService<TopBarVM>();
            this.DataContext = VM;
            Console.WriteLine("[TopBar View] Construct");
        }
    }
}