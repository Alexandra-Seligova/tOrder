using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;

namespace tOrder.UI
{
    public sealed partial class ChargenContentC : UserControl
    {
        // Kolekce pro vstupn� comboboxy (pro vstupn� a v�stupn� n�doby)
        public ObservableCollection<string> EingangLabels { get; set; }
        public ObservableCollection<int> EingangNumbers { get; set; }
        public ObservableCollection<string> AusgangLabels { get; set; }
        public ObservableCollection<int> AusgangNumbers { get; set; }

        public ChargenContentC()
        {
            this.InitializeComponent();

            // Uk�zkov� data
            EingangLabels = new ObservableCollection<string> { "Eingang 1.", "Eingang 2.", "Eingang 3." };
            EingangNumbers = new ObservableCollection<int> { 8, 12, 24 };
            AusgangLabels = new ObservableCollection<string> { "Ausgang 1.", "Ausgang 2." };
            AusgangNumbers = new ObservableCollection<int> { 8, 16 };

            // Nastaven� DataContextu pro binding do XAML
            this.DataContext = this;
        }
    }
}
