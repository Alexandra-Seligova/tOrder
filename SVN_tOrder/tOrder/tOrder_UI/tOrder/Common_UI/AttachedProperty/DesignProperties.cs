using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace tOrder.Common
{
    public static class DesignProperties
    {
        // 📐 Výchozí návrhové hodnoty (readonly)
        public static readonly double DefaultDesignWidth = 1024.0;
        public static readonly double DefaultDesignHeight = 768.0;

        public static readonly bool DefaultLockAspectRatio = false;
        public static readonly bool DefaultLockWidth = false;
        public static readonly bool DefaultLockHeight = false;

        public static readonly string DefaultGroupTag = string.Empty;
        public static readonly string DefaultGroupSubTag = string.Empty;

        #region DesignWidth

        public static readonly DependencyProperty DesignWidthProperty =
            DependencyProperty.RegisterAttached(
                "DesignWidth",
                typeof(double),
                typeof(DesignProperties),
                new PropertyMetadata(DefaultDesignWidth));

        public static void SetDesignWidth(FrameworkElement element, double value)
            => element.SetValue(DesignWidthProperty, value);

        public static double GetDesignWidth(FrameworkElement element)
            => (double)element.GetValue(DesignWidthProperty);

        #endregion

        #region DesignHeight

        public static readonly DependencyProperty DesignHeightProperty =
            DependencyProperty.RegisterAttached(
                "DesignHeight",
                typeof(double),
                typeof(DesignProperties),
                new PropertyMetadata(DefaultDesignHeight));

        public static void SetDesignHeight(FrameworkElement element, double value)
            => element.SetValue(DesignHeightProperty, value);

        public static double GetDesignHeight(FrameworkElement element)
            => (double)element.GetValue(DesignHeightProperty);

        #endregion

        #region LockAspectRatio

        public static readonly DependencyProperty LockAspectRatioProperty =
            DependencyProperty.RegisterAttached(
                "LockAspectRatio",
                typeof(bool),
                typeof(DesignProperties),
                new PropertyMetadata(DefaultLockAspectRatio));

        public static void SetLockAspectRatio(FrameworkElement element, bool value)
            => element.SetValue(LockAspectRatioProperty, value);

        public static bool GetLockAspectRatio(FrameworkElement element)
            => (bool)element.GetValue(LockAspectRatioProperty);

        #endregion

        #region LockWidth

        public static readonly DependencyProperty LockWidthProperty =
            DependencyProperty.RegisterAttached(
                "LockWidth",
                typeof(bool),
                typeof(DesignProperties),
                new PropertyMetadata(DefaultLockWidth));

        public static void SetLockWidth(FrameworkElement element, bool value)
            => element.SetValue(LockWidthProperty, value);

        public static bool GetLockWidth(FrameworkElement element)
            => (bool)element.GetValue(LockWidthProperty);

        #endregion

        #region LockHeight

        public static readonly DependencyProperty LockHeightProperty =
            DependencyProperty.RegisterAttached(
                "LockHeight",
                typeof(bool),
                typeof(DesignProperties),
                new PropertyMetadata(DefaultLockHeight));

        public static void SetLockHeight(FrameworkElement element, bool value)
            => element.SetValue(LockHeightProperty, value);

        public static bool GetLockHeight(FrameworkElement element)
            => (bool)element.GetValue(LockHeightProperty);

        #endregion

        #region GroupTag

        public static readonly DependencyProperty GroupTagProperty =
            DependencyProperty.RegisterAttached(
                "GroupTag",
                typeof(string),
                typeof(DesignProperties),
                new PropertyMetadata(DefaultGroupTag));

        public static void SetGroupTag(FrameworkElement element, string value)
            => element.SetValue(GroupTagProperty, value);

        public static string GetGroupTag(FrameworkElement element)
            => (string)element.GetValue(GroupTagProperty);

        #endregion

        #region GroupSubTag

        public static readonly DependencyProperty GroupSubTagProperty =
            DependencyProperty.RegisterAttached(
                "GroupSubTag",
                typeof(string),
                typeof(DesignProperties),
                new PropertyMetadata(DefaultGroupSubTag));

        public static void SetGroupSubTag(FrameworkElement element, string value)
            => element.SetValue(GroupSubTagProperty, value);

        public static string GetGroupSubTag(FrameworkElement element)
            => (string)element.GetValue(GroupSubTagProperty);

        #endregion

        #region Control

        public static readonly DependencyProperty ControlProperty =
            DependencyProperty.RegisterAttached(
                "Control",
                typeof(string),
                typeof(DesignProperties),
                new PropertyMetadata(string.Empty));

        public static void SetControl(FrameworkElement element, string value)
            => element.SetValue(ControlProperty, value);

        public static string GetControl(FrameworkElement element)
            => (string)element.GetValue(ControlProperty);

        #endregion

        #region Page

        public static readonly DependencyProperty PageProperty =
            DependencyProperty.RegisterAttached(
                "Page",
                typeof(string),
                typeof(DesignProperties),
                new PropertyMetadata(string.Empty));

        public static void SetPage(FrameworkElement element, string value)
            => element.SetValue(PageProperty, value);

        public static string GetPage(FrameworkElement element)
            => (string)element.GetValue(PageProperty);

        #endregion

        #region SubPage

        public static readonly DependencyProperty SubPageProperty =
            DependencyProperty.RegisterAttached(
                "SubPage",
                typeof(string),
                typeof(DesignProperties),
                new PropertyMetadata(string.Empty));

        public static void SetSubPage(FrameworkElement element, string value)
            => element.SetValue(SubPageProperty, value);

        public static string GetSubPage(FrameworkElement element)
            => (string)element.GetValue(SubPageProperty);

        #endregion

        #region Position

        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.RegisterAttached(
                "Position",
                typeof(string),
                typeof(DesignProperties),
                new PropertyMetadata(""));

        public static void SetPosition(FrameworkElement element, string value)
            => element.SetValue(PositionProperty, value);

        public static string GetPosition(FrameworkElement element)
            => (string)element.GetValue(PositionProperty);

        #endregion

        #region PaddingPosition

        public static readonly DependencyProperty PaddingPositionProperty =
            DependencyProperty.RegisterAttached(
                "PaddingPosition",
                typeof(string),
                typeof(DesignProperties),
                new PropertyMetadata(""));

        public static void SetPaddingPosition(FrameworkElement element, string value)
            => element.SetValue(PaddingPositionProperty, value);

        public static string GetPaddingPosition(FrameworkElement element)
            => (string)element.GetValue(PaddingPositionProperty);

        #endregion

        #region PaddingTop

        public static readonly DependencyProperty PaddingTopProperty =
            DependencyProperty.RegisterAttached(
                "PaddingTop",
                typeof(double),
                typeof(DesignProperties),
                new PropertyMetadata(0.0));

        public static void SetPaddingTop(FrameworkElement element, double value)
            => element.SetValue(PaddingTopProperty, value);

        public static double GetPaddingTop(FrameworkElement element)
            => (double)element.GetValue(PaddingTopProperty);

        #endregion

        #region PaddingMiddle

        public static readonly DependencyProperty PaddingMiddleProperty =
            DependencyProperty.RegisterAttached(
                "PaddingMiddle",
                typeof(double),
                typeof(DesignProperties),
                new PropertyMetadata(0.0));

        public static void SetPaddingMiddle(FrameworkElement element, double value)
            => element.SetValue(PaddingMiddleProperty, value);

        public static double GetPaddingMiddle(FrameworkElement element)
            => (double)element.GetValue(PaddingMiddleProperty);

        #endregion

        #region PaddingBottom

        public static readonly DependencyProperty PaddingBottomProperty =
            DependencyProperty.RegisterAttached(
                "PaddingBottom",
                typeof(double),
                typeof(DesignProperties),
                new PropertyMetadata(0.0));

        public static void SetPaddingBottom(FrameworkElement element, double value)
            => element.SetValue(PaddingBottomProperty, value);

        public static double GetPaddingBottom(FrameworkElement element)
            => (double)element.GetValue(PaddingBottomProperty);

        #endregion

    }
}
