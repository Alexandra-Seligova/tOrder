//===================================================================
// $Workfile:: TimestampToTimeConverter.cs                         $
// $Author:: Alexandra_Seligova                                     $
// $Revision:: 4                                                   $
// $Date:: 2025-06-11 15:44:33 +0200 (st, 11 čvn 2025)            $
//===================================================================
// Description:  - tOrder
//     Converts DateTime to localized time string for UI binding.
//===================================================================

namespace tOrder.Common
{
    //-----------------------------------------------------------
    #region Using directives
    //-----------------------------------------------------------

    using System;
    using System.Diagnostics;
    using System.Globalization;
    using Microsoft.UI.Xaml.Data;

    #endregion //Using directives

    //===================================================================
    // class TimestampToTimeConverter
    //===================================================================

    public class TimestampToTimeConverter : IValueConverter
    {
        //-----------------------------------------------------------
        #region Convert (DateTime → string)
        //-----------------------------------------------------------

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                if (value is null)
                {
#if DEBUG
                    Debugger.Break(); // Unexpected null input
#endif
                    return string.Empty;
                }

                if (value is DateTime dt)
                {
                    // Use parameter as format override if needed
                    return dt.ToString("T", CultureInfo.CurrentCulture); // Default: time only
                }

#if DEBUG
                Debugger.Break(); // Unexpected type
#endif
                return string.Empty;
            }
            catch
            {
#if DEBUG
                Debugger.Break(); // Any other error
#endif
                return string.Empty;
            }
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
