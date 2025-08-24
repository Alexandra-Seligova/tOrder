//===================================================================
// $Workfile:: DateFormatConverter.cs                              $
// $Author:: Alexandra_Seligova                                    $
// $Revision:: 1                                                   $
// $Date:: 2025-08-01                                              $
//===================================================================
// Description:  - tOrder
//     Converts DateTime to "dd.MM.yyyy" format.
//===================================================================

namespace tOrder.Common
{
    using System;
    using System.Globalization;
    using Microsoft.UI.Xaml.Data;

    public class DateFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value is DateTime dt
                ? dt.ToString("dd.MM.yyyy", CultureInfo.CurrentCulture)
                : string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language) =>
            throw new NotImplementedException();
    }
}
