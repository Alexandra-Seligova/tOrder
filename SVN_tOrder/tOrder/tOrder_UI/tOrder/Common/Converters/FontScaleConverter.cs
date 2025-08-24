using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;

namespace tOrder.Common
{
    public class FontScaleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                double baseSize = (value is double d) ? d : 14;

                if (Application.Current?.Resources is ResourceDictionary resources)
                {
                    if (resources.TryGetValue("FontScale", out var scaleObj))
                    {
                        if (scaleObj is double scale)
                        {
                            return baseSize * scale;
                        }
                        else
                        {
                            Console.WriteLine("[FontScaleConverter] Hodnota 'FontScale' není typu double.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("[FontScaleConverter] Klíč 'FontScale' nebyl nalezen v Resources.");
                    }
                }
                else
                {
                    Console.WriteLine("[FontScaleConverter] Application.Current nebo Resources jsou null.");
                }

                return baseSize;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FontScaleConverter] Chyba při převodu: {ex.Message}");
                return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            Console.WriteLine("[FontScaleConverter] ConvertBack není implementován.");
            throw new NotImplementedException();
        }
    }
}
