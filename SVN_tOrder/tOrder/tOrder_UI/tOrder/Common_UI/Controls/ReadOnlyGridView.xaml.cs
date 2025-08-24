using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using System.Collections.Generic;
using System.Linq;
using Microsoft.UI.Xaml.Shapes;
using Microsoft.UI.Xaml.Data;
using System;
using Microsoft.UI.Text;

namespace tOrder.UI
{
    public sealed partial class ReadOnlyGridView : UserControl
    {
        public ReadOnlyGridView()
        {
            this.InitializeComponent();
            this.Loaded += OnLoaded;
        }

        public IList<IDictionary<string, string>> ItemsSource
        {
            get => (IList<IDictionary<string, string>>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(IList<IDictionary<string, string>>),
                typeof(ReadOnlyGridView), new PropertyMetadata(null, OnItemsChanged));

        private static void OnItemsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ReadOnlyGridView ctrl)
                ctrl.GenerateContent();
        }

        private void OnLoaded(object sender, RoutedEventArgs e) => GenerateContent();

        private void GenerateContent()
        {
            if (ItemsSource == null || ItemsSource.Count == 0)
                return;

            var columns = ItemsSource[0].Keys.ToList();

            // HEADER
            HeaderGrid.ColumnDefinitions.Clear();
            HeaderGrid.Children.Clear();

            for (int i = 0; i < columns.Count; i++)
            {
                HeaderGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

                var headerText = new TextBlock
                {
                    Text = columns[i],
                    FontWeight = FontWeights.SemiBold,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Center,
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(2)
                };
                Grid.SetColumn(headerText, i);
                HeaderGrid.Children.Add(headerText);
            }

            // ROWS
            ItemsHost.ItemsSource = ItemsSource.Select(rowData =>
            {
                var rowGrid = new Grid();
                for (int i = 0; i < columns.Count; i++)
                    rowGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

                for (int i = 0; i < columns.Count; i++)
                {
                    var text = new TextBlock
                    {
                        Text = rowData[columns[i]],
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Center,
                        TextWrapping = TextWrapping.Wrap,
                        Margin = new Thickness(2)
                    };
                    Grid.SetColumn(text, i);
                    rowGrid.Children.Add(text);
                }

                return rowGrid;
            }).ToList();
        }
    }
}
