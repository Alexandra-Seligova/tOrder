//===================================================================
// $Workfile:: PopupDisplayControlVM.cs
// $Author:: Alexandra Seligová
// $Revision:: 2
// $Date:: 2025-05-28
//===================================================================

namespace tOrder.UI
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using Microsoft.UI.Text;
    using System.Collections.ObjectModel;
    using Windows.UI.Text;

    public sealed partial class PopupDataItem
    {
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }

    public partial class PopupDisplayControlVM : ObservableObject
    {
        [ObservableProperty]
        private string popupTitle = string.Empty;

        [ObservableProperty]
        private FontWeight popupTitleWeight = FontWeights.Normal;

        [ObservableProperty]
        private bool isOpen;

        [ObservableProperty]
        private ObservableCollection<PopupDataItem> popupContent = new();
    }
}
