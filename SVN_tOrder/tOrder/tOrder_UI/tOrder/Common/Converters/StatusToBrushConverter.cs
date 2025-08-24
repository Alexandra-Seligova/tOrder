//===================================================================
// $Workfile:: StatusToBrushConverter.cs                           $
// $Author:: Alexandra_Seligova                                     $
// $Revision:: 4                                                   $
// $Date:: 2025-06-11 15:44:33 +0200 (st, 11 čvn 2025)            $
//===================================================================
// Description:  - tOrder
//     Converts status text to corresponding SolidColorBrush.
//===================================================================

namespace tOrder.Common
{
    //-----------------------------------------------------------
    #region Using directives
    //-----------------------------------------------------------

    using System;
    using System.Diagnostics;
    using Microsoft.UI.Xaml.Data;
    using Microsoft.UI.Xaml.Media;
    using Windows.UI;

    #endregion //Using directives

    //===================================================================
    // class StatusToBrushConverter
    //===================================================================

    public sealed class StatusToBrushConverter : IValueConverter
    {
        //-----------------------------------------------------------
        #region Brushes
        //-----------------------------------------------------------

        public SolidColorBrush GreenBrush { get; set; } = new(Color.FromArgb(255, 0, 153, 61));     // "Läuft"
        public SolidColorBrush OrangeBrush { get; set; } = new(Color.FromArgb(255, 255, 140, 0));   // "Gestoppt"
        public SolidColorBrush RedBrush { get; set; } = new(Color.FromArgb(255, 192, 57, 43));      // Default/fallback

        #endregion //Brushes

        //-----------------------------------------------------------
        #region Convert
        //-----------------------------------------------------------

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            try
            {
                if (value is null)
                {
#if DEBUG
                    Debugger.Break(); // Null input detected during development
#endif
                    return RedBrush;
                }

                string? status = value as string;
                if (string.IsNullOrWhiteSpace(status))
                    return RedBrush;

                return status switch
                {
                    "Läuft" => GreenBrush,
                    "Gestoppt" => OrangeBrush,
                    _ => RedBrush
                };
            }
            catch
            {
#if DEBUG
                Debugger.Break(); // Unhandled exception in converter
#endif
                return RedBrush;
            }
        }

        #endregion //Convert

        //-----------------------------------------------------------
        #region ConvertBack
        //-----------------------------------------------------------

        public object ConvertBack(object value, Type targetType, object parameter, string language) =>
            throw new NotImplementedException(); // One-way binding only

        #endregion //ConvertBack
    }

    //===================================================================
}
