// tOrder.Common.BindingProxy.cs
using Microsoft.UI.Xaml;

namespace tOrder.Common;
public class BindingProxy : DependencyObject
{
    public object Data
    {
        get => GetValue(DataProperty);
        set => SetValue(DataProperty, value);
    }

    public static readonly DependencyProperty DataProperty =
        DependencyProperty.Register(nameof(Data), typeof(object), typeof(BindingProxy), new PropertyMetadata(null));
}
