using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace tOrder.Common
{
    public class SectionTypeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null || parameter == null)
                return Visibility.Collapsed;

            var current = value.ToString();
            var target = parameter.ToString();

            return string.Equals(current, target, StringComparison.OrdinalIgnoreCase)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
            => throw new NotImplementedException();
    }
}
