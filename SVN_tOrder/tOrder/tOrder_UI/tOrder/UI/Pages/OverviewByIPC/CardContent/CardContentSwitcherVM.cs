
namespace tOrder.UI;
    using System;
    using System.Collections.ObjectModel;
    using CommunityToolkit.Mvvm.ComponentModel;
    using Microsoft.UI.Xaml.Media;
    using Windows.UI;

    public partial class CardContentSwitcherVM : ObservableObject
    {
        private readonly CapacityPositionCardVM _vm;

        // Sekce
        [ObservableProperty]
        private SectionType activeSection;

        [ObservableProperty]
        private ObservableCollection<MenuItemM> menuItems = [];
        /// <summary>
        /// Událost pro code-behind na změnu sekce.
        /// </summary>
        public event EventHandler<SectionType>? ActiveSectionChanged;

        public CardContentSwitcherVM(CapacityPositionCardVM vm)
        {
            _vm = vm;

            // Forward počáteční hodnoty
            activeSection = _vm.ActiveSection;

            // Reakce na změnu v parent VM
            _vm.PropertyChanged += (_, e) =>
            {
                if (e.PropertyName == nameof(_vm.ActiveSection))
                {
                    if (ActiveSection != _vm.ActiveSection)
                    {
                        ActiveSection = _vm.ActiveSection;
                    }
                }
            };
        }

        // Forward property z parent VM
        public string Status => _vm.Status;

        public CapacityState LocalState => _vm.LocalState;
        public string Type => _vm.Type;
        public string TextOrderNumber => _vm.TextOrderNumber;
        public string TextArticle => _vm.TextArticle;
        public string TextHaeringCharge => _vm.TextHaeringCharge;
        public string TextMaterialBatch => _vm.TextMaterialBatch;
        public ObservableCollection<InfoItem> MainInfo => _vm.MainInfo;


        // Z pozice – status brush (stav stroje)
        public SolidColorBrush StatusBrush => _vm.StatusBrush;



        // Přímá volba sekce
        public void ShowSection(SectionType type) => ActiveSection = type;

        partial void OnActiveSectionChanged(SectionType oldValue, SectionType newValue)
        {
            _vm.ActiveSection = newValue; // zpětný zápis
            ActiveSectionChanged?.Invoke(this, newValue);
        }
    }
