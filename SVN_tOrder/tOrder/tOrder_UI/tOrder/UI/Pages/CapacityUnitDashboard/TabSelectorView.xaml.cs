//===================================================================
// $Workfile:: TabSelectorView.xaml.cs                               $
// $Author:: Alexandra_Seligova                                      $
// $Revision:: 1                                                    $
// $Date:: 2025-06-19 18:09:00 +0200 (èt, 19 èvn 2025)              $
//===================================================================
// Description:  - tOrder
//     TabSelectorView – UI control for tab selection in CapacityUnitDashboard.
//     Provides tab highlighting, switching, and event notification.
//===================================================================

namespace tOrder.UI;

//-----------------------------------------------------------
#region Using directives
//-----------------------------------------------------------
using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using Microsoft.UI.Text;
using Windows.UI.Text;

#endregion //Using directives

//===================================================================
// class TabSelectorView
//===================================================================

public sealed partial class TabSelectorView : UserControl
{
    //-----------------------------------------------------------
    #region Fields & Properties
    //-----------------------------------------------------------

    /// <summary>
    /// Raised when the user changes tab selection (zero-based index).
    /// </summary>
    public event Action<int>? TabChanged;

    /// <summary>
    /// Currently selected tab index.
    /// </summary>
    public int SelectedIndex
    {
        get => (int)GetValue(SelectedIndexProperty);
        set => SetValue(SelectedIndexProperty, value);
    }

    public static readonly DependencyProperty SelectedIndexProperty =
        DependencyProperty.Register(nameof(SelectedIndex), typeof(int), typeof(TabSelectorView),
            new PropertyMetadata(0, OnSelectedIndexChanged));

    #endregion //Fields & Properties

    //-----------------------------------------------------------
    #region Constructor
    //-----------------------------------------------------------

    public TabSelectorView()
    {
        this.InitializeComponent();
        UpdateTabVisuals(SelectedIndex);
    }

    #endregion //Constructor

    //-----------------------------------------------------------
    #region Event Handlers
    //-----------------------------------------------------------

    private void Tab_Click(object sender, RoutedEventArgs e)
    {
        int selectedIndex = int.Parse(((Button)sender).Tag.ToString());
        SelectedIndex = selectedIndex;  // Spustí celou aktualizaci správnì
    }



    private static void OnSelectedIndexChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is TabSelectorView view && e.NewValue is int newIndex)
        {
            view.UpdateTabVisuals(newIndex);
            view.TabChanged?.Invoke(newIndex);
        }
    }

    #endregion //Event Handlers

    //-----------------------------------------------------------
    #region Visual Updates
    //-----------------------------------------------------------

    /// <summary>
    /// Updates the visual state of all tab buttons and indicator bars.
    /// </summary>
    /// <param name="activeIndex">Index of the currently active tab.</param>
    private void UpdateTabVisuals(int activeIndex)
    {
        for (int i = 0; i < 8; i++)
        {
            if (TabRoot.Children[i] is StackPanel stack
                && stack.Children[0] is Button button
                && stack.Children[1] is Border border)
            {
                if (i == activeIndex)
                {
                    button.Foreground = (SolidColorBrush)Resources["TabActiveBrush"];
                    button.FontWeight = FontWeights.Bold;
                    border.Background = (SolidColorBrush)Resources["TabActiveBrush"];
                }
                else
                {
                    button.Foreground = (SolidColorBrush)Resources["TabInactiveBrush"];
                    button.FontWeight = FontWeights.Bold;
                    border.Background = new SolidColorBrush(Microsoft.UI.Colors.Transparent);
                }
            }
        }
    }

    #endregion //Visual Updates
}
//===================================================================
