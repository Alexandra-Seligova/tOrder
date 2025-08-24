//===================================================================
// $Workfile:: TabToVisibilityConverter.cs                          $
// $Author:: Alexandra_Seligova                                     $
// $Revision:: 1                                                    $
// $Date:: 2025-08-15 22:17:00 +0200 (čt, 15 srp 2025)              $
//===================================================================
// Description: SPC - tOrder
//     Converts selected tab name to Visibility for conditional UI.
//===================================================================

namespace tOrder.Common
{
    //-----------------------------------------------------------
    #region Using directives
    //-----------------------------------------------------------

    using System;
    using System.Diagnostics;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Data;

    #endregion //Using directives

    //===================================================================
    // class TabToVisibilityConverter
    //===================================================================

    public sealed class TabToVisibilityConverter : IValueConverter
    {
        //-----------------------------------------------------------
        #region Convert (string → Visibility)
        //-----------------------------------------------------------

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                if (value is null || parameter is null)
                {
#if DEBUG
                    Debugger.Break(); // Null input is unexpected
#endif
                    return Visibility.Collapsed;
                }

                return string.Equals(value.ToString(), parameter.ToString(), StringComparison.Ordinal)
                    ? Visibility.Visible
                    : Visibility.Collapsed;
            }
            catch
            {
#if DEBUG
                Debugger.Break(); // Conversion error
#endif
                return Visibility.Collapsed;
            }
        }

        #endregion //Convert

        //-----------------------------------------------------------
        #region ConvertBack (Not Supported)
        //-----------------------------------------------------------

        public object ConvertBack(object value, Type targetType, object parameter, string language) =>
            throw new NotSupportedException(); // One-way only

        #endregion //ConvertBack
    }

    //===================================================================
}
