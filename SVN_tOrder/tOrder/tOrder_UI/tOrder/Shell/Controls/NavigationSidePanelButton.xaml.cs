//===================================================================
// $Workfile:: NavigationSidePanelButton.xaml.cs                   $
// $Author:: Alexandra_Seligova                                   $
// $Revision:: 4                                                  $
// $Date:: 2025-06-11 15:44:33 +0200 (st, 11 čvn 2025)           $
//===================================================================
// Description: SPC - tOrder
//     Reusable control for single navigation button in side panel
//===================================================================

namespace tOrder.Shell;

//-----------------------------------------------------------
#region Using directives
//-----------------------------------------------------------

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using tOrder.Common;

#endregion //Using directives

/// <summary>
/// Reusable button with icon and text for left-side navigation.
/// </summary>
public sealed partial class NavigationSidePanelButton : UserControl
{
    public NavigationSidePanelButton()
    {
        this.InitializeComponent();
    }

    public static readonly DependencyProperty IconGlyphProperty =
        DependencyProperty.Register(nameof(IconGlyph), typeof(string), typeof(NavigationSidePanelButton), new PropertyMetadata("\uE80F"));

    public string IconGlyph
    {
        get => (string)GetValue(IconGlyphProperty);
        set => SetValue(IconGlyphProperty, value);
    }

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register(nameof(Text), typeof(string), typeof(NavigationSidePanelButton), new PropertyMetadata(string.Empty));

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly DependencyProperty PageKeyProperty =
    DependencyProperty.Register(nameof(PageKey), typeof(string), typeof(NavigationSidePanelButton), new PropertyMetadata(string.Empty));

    public string PageKey
    {
        get => (string)GetValue(PageKeyProperty);
        set => SetValue(PageKeyProperty, value);
    }
    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is string key && !string.IsNullOrWhiteSpace(key))
        {
            var nav = App.GetService<INavigationService>();
            // nav.Navigate(key);
        }
    }

}