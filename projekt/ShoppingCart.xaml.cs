using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace projekt
{
    /// <summary>
    /// Logika interakcji dla klasy ShoppingCart.xaml
    /// </summary>
    public partial class ShoppingCart : Window
    {
        List<Product> products = new List<Product>();
        public ShoppingCart()
        {
            InitializeComponent();
            if(login.CurrentUser != null && login.CurrentUser.getShoppingCart() != null)
            {
                products = login.CurrentUser.getShoppingCart();
                AddItems();
            }
        }

        private void AddItems()
        {
            Cart.Children.Clear();
            if (login.CurrentUser != null && products.Count == 0)
            {
                TextBlock info = new TextBlock()
                {
                    FontSize = 30,
                    Foreground = new BrushConverter().ConvertFrom("#B0927F") as Brush,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 20, 0, 0),
                    Text = "koszyk jest Pusty"
                };
                Cart.Children.Add(info);
            } else
            {
                for(int i = 0; i < products.Count; i++)
                {
                    Cart.RowDefinitions.Add(new RowDefinition { Height = new GridLength(200) });
                }


                for (int i = 0; i < products.Count; i++)
                {

                    Border card = new Border()
                    {
                        Background = new BrushConverter().ConvertFrom("#17151a") as Brush,
                        CornerRadius = new CornerRadius(15)
                    };

                    if(i != products.Count)
                    {
                        card.Margin = new Thickness(0, 0, 0, 20);
                    }

                    Grid cardGrid = new Grid();
                    cardGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(262) });
                    cardGrid.ColumnDefinitions.Add(new ColumnDefinition());

                    card.Child = cardGrid;

                    Border image = new Border
                    {
                        CornerRadius = new CornerRadius(15, 0, 0, 15),
                        Height = card.Height,
                        Background = new ImageBrush(new BitmapImage(new Uri(products[i].productImg)))
                        {
                            Stretch = System.Windows.Media.Stretch.UniformToFill
                        }
                    };

                    cardGrid.Children.Add(image);

                    Grid.SetColumn(image, 0);

                    Grid.SetRow(card, i);

                    Cart.Children.Add(card);   
                }

            }
        }

        private void TopBorder_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            shop shop = new shop();
            shop.Show();
            this.Close();
        }

        
    }
}
