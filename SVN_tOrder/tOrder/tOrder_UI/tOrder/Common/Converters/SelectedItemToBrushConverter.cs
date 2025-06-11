//===================================================================
// $Workfile:: SelectedItemToBrushConverter.cs                    $
// $Author:: Alexandra_Seligova                                     $
// $Revision:: 4                                                   $
// $Date:: 2025-06-11 15:44:33 +0200 (st, 11 čvn 2025)            $
//===================================================================
// Description: SPC - tOrder
//     Converts selected item equality to a highlight brush.
//===================================================================

namespace tOrder.Common
{
    //-----------------------------------------------------------
    #region Using directives
    //-----------------------------------------------------------

    using System;
    using Microsoft.UI;
    using Microsoft.UI.Xaml.Data;
    using Microsoft.UI.Xaml.Media;
    using Windows.UI;

    #endregion //Using directives

    //===================================================================
    // class SelectedItemToBrushConverter
    //===================================================================

    public class SelectedItemToBrushConverter : IValueConverter
    {
        //-----------------------------------------------------------
        #region Convert
        //-----------------------------------------------------------

        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value == null || parameter == null)
                return new SolidColorBrush(Colors.DimGray); // Default (non-selected)

            return Equals(value, parameter)
                ? new SolidColorBrush(Colors.DarkSlateBlue) // Selected
                : new SolidColorBrush(Colors.DimGray);      // Not selected
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
