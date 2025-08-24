using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;

namespace tOrder.Common
{
    public class TabTextForegroundConverter : IValueConverter
    {
        public Brush SelectedBrush { get; set; }
        public Brush UnselectedBrush { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int containerId && parameter is int selectedContainerId)
            {
                return containerId == selectedContainerId ? SelectedBrush : UnselectedBrush;
            }

            return UnselectedBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

}
