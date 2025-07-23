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
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Controls;

    using tOrder.Shell;

    /// <summary>
    /// Code-behind for the TopBar control.
    /// </summary>
    public sealed partial class TopBar : UserControl
    {
        public TopBarVM VM { get; }

        private string _tooltipText = "TopBar";

        public string TooltipText
        {
            get => _tooltipText;
            set
            {
                if (_tooltipText != value)
                {
                    _tooltipText = value;
                    ToolTipService.SetToolTip(TopBarBorder, _tooltipText);
                }
            }
        }

        public TopBar()
        {
            this.InitializeComponent();
            VM = App.GetService<TopBarVM>();
            this.DataContext = VM;

            TopBarGrid.SizeChanged += TopBarGrid_SizeChanged;

            UpdateTooltip();
        }

        private void TopBarGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            UpdateTooltip();
        }

        private void UpdateTooltip()
        {
            int columnCount = TopBarGrid.ColumnDefinitions.Count;
            TooltipText = $"TopBar {TopBarGrid.ActualWidth:0}x{TopBarGrid.ActualHeight:0} {columnCount}xcol.";
        }
    }

}