//===================================================================
// $Workfile:: NotificationTypeToBrushConverter.cs                $
// $Author:: Alexandra_Seligova                                     $
// $Revision:: 4                                                   $
// $Date:: 2025-06-11 15:44:33 +0200 (st, 11 čvn 2025)            $
//===================================================================
// Description: SPC - tOrder
//     Converts NotificationType enum to resource-based Brush.
//===================================================================

namespace tOrder.Common
{
    //-----------------------------------------------------------
    #region Using directives
    //-----------------------------------------------------------

    using System;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Data;

    #endregion //Using directives

    //===================================================================
    // class NotificationTypeToBrushConverter
    //===================================================================

    public sealed class NotificationTypeToBrushConverter : IValueConverter
    {
        //-----------------------------------------------------------
        #region Convert
        //-----------------------------------------------------------

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value switch
            {
                NotificationType.None => Application.Current.Resources["NotificationNoneBrush"],
                NotificationType.Info => Application.Current.Resources["NotificationInfoBrush"],
                NotificationType.Upcoming => Application.Current.Resources["NotificationUpcomingBrush"],
                NotificationType.Warning => Application.Current.Resources["NotificationWarningBrush"],
                NotificationType.Debug => Application.Current.Resources["DebugBrush"],
                _ => Application.Current.Resources["NotificationNoneBrush"]
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
