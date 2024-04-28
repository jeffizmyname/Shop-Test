using Newtonsoft.Json;
using SharpVectors.Converters;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace projekt
{
    /// <summary>
    /// Logika interakcji dla klasy shop.xaml
    /// </summary>
    public partial class shop : Window
    {
        public shop()
        {
            InitializeComponent();
            FillProducts();
        }

        private void FillProducts(String search = null)
        {
            String res;
            if (search == null)
            {
                res = Conection.query("SELECT * FROM products");

            } else
            {
                res = Conection.query($"SELECT * FROM products WHERE productName LIKE '%{search}%'");
            }
            List<Product> products = JsonConvert.DeserializeObject<List<Product>>(res);

            productsGrid.Children.Clear();

            if (products != null)
            {
                for (int i = 0; i < Math.Ceiling((double)(products.Count / 5.0)); i++)
                {
                    productsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(320) });
                }

                for (int i = 0; i < products.Count; i++)
                {
                    AddProductCard(products[i], i / 5, i % 5);
                }
            }


            Trace.WriteLine(res);
            Trace.WriteLine(products.Count / 5.0);
        }

        private void TopBorder_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void AddProductCard(Product product, int row, int col)
        {
            Border border = new Border
            {
                Background = new BrushConverter().ConvertFrom("#17151a") as Brush,
                Margin = new Thickness(10, 5, 10, 5),
                CornerRadius = new CornerRadius(15),
                Height = 300,
            };

            Grid grid = new Grid
            {
                Background = Brushes.Transparent,
            };

            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(200) }); 
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }); 
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) }); 

            Border image = new Border
            {
                CornerRadius = new CornerRadius(15, 15, 0, 0),
                Width = border.Width,
                Height = 200,
                Background = new ImageBrush(new BitmapImage(new Uri(product.productImg)))
                {
                    Stretch = System.Windows.Media.Stretch.UniformToFill    
                }
            };

            TextBlock titleTextBlock = new TextBlock
            {
                Text = product.productName,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(10, 20, 0, 0),
                FontWeight = FontWeights.Bold,
                FontSize = 14,
                Foreground = new BrushConverter().ConvertFrom("#dbb69e") as Brush
            };

            TextBlock priceTextBlock = new TextBlock
            {
                Text = $"{product.productPrice}zł",
                Margin = new Thickness(10, 10, 0, 0),
                FontWeight = FontWeights.Bold,
                FontSize = 16,
                Foreground = new BrushConverter().ConvertFrom("#B0927F") as Brush
            };

            Grid buyPlusPrice = new Grid
            {
                Background = Brushes.Transparent,
                Width = border.Width,
            };

            buyPlusPrice.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) });
            buyPlusPrice.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            Border buttonBorder = new Border
            {
                Background = new BrushConverter().ConvertFrom("#B0927F") as Brush,
                Margin = new Thickness(5, 5, 0, 5),
                CornerRadius = new CornerRadius(10),
                Height = 30,
                Width = 30,
            };

            Button addToCart = new Button
            {
                Background = Brushes.Transparent,
                BorderBrush = Brushes.Transparent,
                Height = 30,
                Width = 30,
                Style = (Style)FindResource("btnHover"),
                HorizontalAlignment= HorizontalAlignment.Right,
            };

            SvgViewbox icon = new SvgViewbox 
            { 
                Width= 10,
                Height= 10,
            };

            icon.Load(new Uri("/Assets/add.svg", UriKind.Relative));

            addToCart.Content = icon;
            buttonBorder.Child = addToCart;
            //buttonBorder.Child = icon;

            buyPlusPrice.Children.Add(priceTextBlock);
            buyPlusPrice.Children.Add(buttonBorder);

            Grid.SetColumn(priceTextBlock, 0);
            Grid.SetColumn(buttonBorder, 1);

            grid.Children.Add(image);
            grid.Children.Add(titleTextBlock);
            grid.Children.Add(buyPlusPrice);

            Grid.SetRow(image, 0);
            Grid.SetRow(titleTextBlock, 1);
            Grid.SetRow(buyPlusPrice, 2);

            border.Child = grid;

            Grid.SetRow(border, row);
            Grid.SetColumn(border, col);

            // Add the grid to the main StackPanel
            productsGrid.Children.Add(border);
        }

        private void searchQ_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = (TextBox)wrapText.Template.FindName("searchQ", wrapText);
            string textBoxValue = textBox.Text;
            Trace.WriteLine(textBoxValue);
            FillProducts(textBoxValue);
        }
    }
}
