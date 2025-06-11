//===================================================================
// $Workfile:: AlertTypeToBrushConverter.cs                        $
// $Author:: Alexandra_Seligova                                     $
// $Revision:: 4                                                   $
// $Date:: 2025-06-11 15:44:33 +0200 (st, 11 čvn 2025)            $
//===================================================================
// Description: SPC - tOrder
//     Converts AlertType to SolidColorBrush.
//     TODO: Replace inline colors with StaticResource.
//===================================================================

namespace tOrder.Common
{
    //-----------------------------------------------------------
    #region Using directives
    //-----------------------------------------------------------

    using System;
    using System.Diagnostics;
    using Microsoft.UI;
    using Microsoft.UI.Xaml.Data;
    using Microsoft.UI.Xaml.Media;
    using tOrder.Common;
    using tOrder.Shell;

    #endregion //Using directives

    //===================================================================
    // class AlertTypeToBrushConverter
    //===================================================================

    public class AlertTypeToBrushConverter : IValueConverter
    {
        //-----------------------------------------------------------
        #region Constants
        //-----------------------------------------------------------

        private static readonly SolidColorBrush DefaultBrush = new(Colors.Gray); // fallback brush

        #endregion //Constants

        //-----------------------------------------------------------
        #region Convert (AlertType → Brush)
        //-----------------------------------------------------------

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                if (value is null)
                {
#if DEBUG
                    Debugger.Break(); // Null value during dev
#endif
                    return DefaultBrush;
                }

                if (value is AlertType alertType)
                {
                    return alertType switch
                    {
                        AlertType.Info => new SolidColorBrush(Colors.SkyBlue),
                        AlertType.Warning => new SolidColorBrush(Colors.Goldenrod),
                        AlertType.Error => new SolidColorBrush(Colors.IndianRed),
                        AlertType.Critical => new SolidColorBrush(Colors.DarkRed),
                        _ => DefaultBrush
                    };
                }

#if DEBUG
                Debugger.Break(); // Invalid input type
#endif
                return DefaultBrush;
            }
            catch
            {
#if DEBUG
                Debugger.Break(); // Conversion error
#endif
                return DefaultBrush;
            }
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
