//===================================================================
// $Workfile:: BooleanToVisibilityInverseConverter.cs              $
// $Author:: Alexandra_Seligova                                     $
// $Revision:: 4                                                   $
// $Date:: 2025-06-11 15:44:33 +0200 (st, 11 čvn 2025)            $
//===================================================================
// Description: SPC - tOrder
//     Inverse converter – shows when bool is false.
//===================================================================

namespace tOrder.Common
{
    using System;
    using System.Diagnostics;
    using Microsoft.UI.Xaml;
    using Microsoft.UI.Xaml.Data;

    public class BooleanToVisibilityInverseConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
#if DEBUG
            Debug.WriteLine($"[Converter] InverseBool → value = {value}");
#endif
            try
            {
                if (value is not bool b)
                {
#if DEBUG
                    Debug.WriteLine("[Converter] Invalid input for InverseBool");
                    Debugger.Break();
#endif
                    return Visibility.Visible; // fallback
                }

                return !b ? Visibility.Visible : Visibility.Collapsed;
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine($"[Converter Error] InverseBool: {ex}");
                Debugger.Break();
#endif
                return Visibility.Visible;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            try
            {
                if (value is not Visibility v)
                {
#if DEBUG
                    Debug.WriteLine("[Converter] Invalid ConvertBack input for InverseBool");
                    Debugger.Break();
#endif
                    return true;
                }

                return v != Visibility.Visible;
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine($"[Converter Error] ConvertBack InverseBool: {ex}");
                Debugger.Break();
#endif
                return true;
            }
        }
    }
}
