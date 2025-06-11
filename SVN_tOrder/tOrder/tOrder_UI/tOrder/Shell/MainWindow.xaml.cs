/***************************************************************************
 *
 * tOrder Application
 *
 * Company      : SPC solutions s.r.o.
 * Author       : Alexandra Seligová
 *
 * Description  :
 * - Main window (UI shell) for the tOrder application.
 * - Initializes and binds the MainWindowViewModel as DataContext.
 *
 ***************************************************************************/

namespace tOrder.Shell
{
    using Microsoft.UI.Xaml;


    /// <summary>
    /// Represents the main window of the application.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MainWindow"/> class.
        /// </summary>
        public MainWindow()
        {
            this.InitializeComponent();
            // NEpřiřazuj ViewModel – Layout má vlastní
        }
    }
}