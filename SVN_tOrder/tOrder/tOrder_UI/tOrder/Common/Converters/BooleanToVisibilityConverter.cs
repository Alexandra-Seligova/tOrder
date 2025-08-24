//===================================================================
// $Workfile:: BooleanToVisibilityConverter.cs                     $
// $Author:: Alexandra_Seligova                                     $
// $Revision:: 4                                                   $
// $Date:: 2025-06-11 15:44:33 +0200 (st, 11 čvn 2025)            $
//===================================================================
// Description:  - tOrder
//     Converts bool to Visibility (Visible/Collapsed).
//     Supports inversion via ConverterParameter = "Inverse".
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

    #endregion

    //===================================================================
    // class BooleanToVisibilityConverter
    //===================================================================

    public class BooleanToVisibilityConverter : IValueConverter
    {
        //-----------------------------------------------------------
        #region Convert (bool → Visibility)
        //-----------------------------------------------------------

        public object Convert(object value, Type targetType, object parameter, string language)
        {
#if DEBUG
            // Console.WriteLine($"[Converter] BooleanToVisibility → value = {value}, param = {parameter}");
#endif
            try
            {
                if (value is null)
                {
#if DEBUG
                    Debugger.Break(); // Null input detected
#endif
                    return Visibility.Collapsed;
                }

                bool invert = string.Equals(parameter?.ToString(), "Inverse", StringComparison.OrdinalIgnoreCase);
                bool flag = value is bool b && b;

                return (invert ? !flag : flag) ? Visibility.Visible : Visibility.Collapsed;
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine($"[Converter Error] BooleanToVisibility: {ex}");
                Debugger.Break();
#endif
                return Visibility.Collapsed;
            }
        }

        #endregion

        //-----------------------------------------------------------
        #region ConvertBack (Visibility → bool)
        //-----------------------------------------------------------

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            try
            {
                if (value is not Visibility visibility)
                {
#if DEBUG
                    Debugger.Break(); // Invalid cast
#endif
                    return false;
                }

                bool invert = string.Equals(parameter?.ToString(), "Inverse", StringComparison.OrdinalIgnoreCase);
                return invert ? visibility != Visibility.Visible : visibility == Visibility.Visible;
            }
            catch (Exception ex)
            {
#if DEBUG
                Console.WriteLine($"[Converter Error] ConvertBack: {ex}");
                Debugger.Break();
#endif
                return false;
            }
        }

        #endregion
    }
}
