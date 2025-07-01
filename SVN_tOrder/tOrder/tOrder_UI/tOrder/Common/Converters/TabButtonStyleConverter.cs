using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace tOrder.Common
{
    public class TabButtonStyleConverter : IValueConverter
    {
        public Style? SelectedStyle { get; set; }
        public Style? UnselectedStyle { get; set; }

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                if (value is int containerId && parameter is int selectedId)
                {
                    return containerId == selectedId ? SelectedStyle : UnselectedStyle;
                }

                Console.WriteLine($"[warning] TabButtonStyleConverter: Nepodařilo se rozpoznat vstupní hodnoty. " +
                                  $"value={value?.GetType().Name ?? "null"}, parameter={parameter?.GetType().Name ?? "null"}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[error] TabButtonStyleConverter selhal: {ex.Message}");
            }

            return UnselectedStyle;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            Console.WriteLine("[info] TabButtonStyleConverter: ConvertBack není implementován.");
            throw new NotImplementedException();
        }
    }
}
