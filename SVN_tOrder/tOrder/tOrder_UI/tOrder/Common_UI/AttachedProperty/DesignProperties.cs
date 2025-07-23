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
    }
}
