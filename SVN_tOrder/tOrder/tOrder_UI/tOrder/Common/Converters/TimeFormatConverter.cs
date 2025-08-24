//===================================================================
// $Workfile:: TimeFormatConverter.cs                              $
// $Author:: Alexandra_Seligova                                     $
// $Revision:: 4                                                   $
// $Date:: 2025-06-11 15:44:33 +0200 (st, 11 čvn 2025)            $
//===================================================================
// Description:  - tOrder
//     Converts DateTime to "HH:mm" time-only string.
//===================================================================

namespace tOrder.Common
{
    //-----------------------------------------------------------
    #region Using directives
    //-----------------------------------------------------------

    using System;
    using System.Globalization;
    using Microsoft.UI.Xaml.Data;

    #endregion //Using directives

    //===================================================================
    // class TimeFormatConverter
    //===================================================================

    public class TimeFormatConverter : IValueConverter
    {
        //-----------------------------------------------------------
        #region Convert (DateTime → string)
        //-----------------------------------------------------------

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value is DateTime dt
                ? dt.ToString("HH:mm", CultureInfo.CurrentCulture)  // Time only: 24h format
                : string.Empty;
        }

        #endregion //Convert

        //-----------------------------------------------------------
        #region ConvertBack (Not Supported)
        //-----------------------------------------------------------

        public object ConvertBack(object value, Type targetType, object parameter, string language) =>
            throw new NotImplementedException(); // One-way only

        #endregion //ConvertBack
    }

    //===================================================================
}
