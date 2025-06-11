//===================================================================
// $Workfile:: AlertIconVisibilityConverter.cs                     $
// $Author:: Alexandra_Seligova                                     $
// $Revision:: 4                                                   $
// $Date:: 2025-06-11 15:44:33 +0200 (st, 11 čvn 2025)            $
//===================================================================
// Description: SPC - tOrder
//     Shows alert icon only when AlertType is not None.
//===================================================================

namespace tOrder.Common
{
    //-----------------------------------------------------------
    #region Using directives
    //-----------------------------------------------------------

    using System;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Data;
    using tOrder.Common;
    using tOrder.Shell;

    #endregion //Using directives

    //===================================================================
    // class AlertIconVisibilityConverter
    //===================================================================

    public class AlertIconVisibilityConverter : IValueConverter
    {
        //-----------------------------------------------------------
        #region Convert (AlertType → Visibility)
        //-----------------------------------------------------------

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            return value is AlertType type && type != AlertType.None
                ? Visibility.Visible
                : Visibility.Collapsed;
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
