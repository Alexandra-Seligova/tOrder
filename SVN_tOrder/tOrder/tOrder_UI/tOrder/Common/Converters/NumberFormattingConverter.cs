//===================================================================
// $Workfile:: NumberFormattingConverter.cs                        $
// $Author:: Alexandra_Seligova                                     $
// $Revision:: 4                                                   $
// $Date:: 2025-06-11 15:44:33 +0200 (st, 11 čvn 2025)            $
//===================================================================
// Description: SPC - tOrder
//     Formats integers using German locale with thousands separator.
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
    // class NumberFormattingConverter
    //===================================================================

    public sealed class NumberFormattingConverter : IValueConverter
    {
        //-----------------------------------------------------------
        #region Convert (int/uint → "1.234")
        //-----------------------------------------------------------

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var format = new CultureInfo("de-DE"); // Use German number format

            return value switch
            {
                uint number => number.ToString("N0", format),
                int i => i.ToString("N0", format),
                _ => string.Empty
            };
        }

        #endregion //Convert

        //-----------------------------------------------------------
        #region ConvertBack
        //-----------------------------------------------------------

        public object ConvertBack(object value, Type targetType, object parameter, string language) =>
            throw new NotImplementedException(); // One-way only

        #endregion //ConvertBack
    }

    //===================================================================
}
