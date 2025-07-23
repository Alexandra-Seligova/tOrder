//===================================================================
// $Workfile:: PopupDisplayControl.xaml.cs
// $Author:: Alexandra Seligová
// $Revision:: 8
// $Date:: 2025-05-28
//===================================================================
#nullable enable

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Specialized;

namespace tOrder.UI
{
    public sealed partial class PopupDisplayControl : UserControl
    {
        public PopupDisplayControl()
        {
            this.InitializeComponent();
            Console.WriteLine($"[PopupDisplayControl view] Construct");

        }



        private void ClosePopup_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine($"[PopupDisplayControl view] ClosePopup_Click");
            if (DataContext is PopupDisplayControlVM vm)
                vm.IsOpen = false;
        }

        public PopupDisplayControlVM ViewModel => (PopupDisplayControlVM)this.DataContext;
    }
}
