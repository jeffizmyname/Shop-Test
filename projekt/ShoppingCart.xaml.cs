using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// 
    public partial class ShoppingCart : Window
    {
        public static Order? UserOrder { get; private set; } = new();
        double TotalPrice = 0.00;
        List<Product> products = new List<Product>();
        public ShoppingCart()
        {
            InitializeComponent();
            if(login.CurrentUser != null && login.CurrentUser.getShoppingCart() != null)
            {
                UserOrder.FastDelivery = false;
                products = login.CurrentUser.getShoppingCart();
                AddItems();
            }
        }

        private void AddItems()
        {
            //resset
            TotalPrice = 0.00;
            Cart.Children.Clear();
            Info.Children.Remove(Info.FindName("All") as UIElement);
            Info.Children.Remove(Info.FindName("Shipments") as UIElement);
            Info.Children.Remove(Info.FindName("paymentPriceBlock") as UIElement);

            
            if (login.CurrentUser != null && products.Count == 0) // sprawdza czy produkty som
            {//wypisujeze nie ma
                TextBlock info = new TextBlock()
                {
                    FontSize = 30,
                    Foreground = new BrushConverter().ConvertFrom("#B0927F") as Brush,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 20, 0, 0),
                    Text = "koszyk jest Pusty"
                };
                Cart.Children.Add(info);
            } else // wypisuje produkty
            {
                for(int i = 0; i < products.Count; i++)
                {
                    Cart.RowDefinitions.Add(new RowDefinition { Height = new GridLength(200) });
                    TotalPrice += products[i].productPrice;
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
                    cardGrid.RowDefinitions.Add(new RowDefinition());
                    cardGrid.RowDefinitions.Add(new RowDefinition());

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

                    TextBlock titleTextBlock = new TextBlock
                    {
                        Text = products[i].productName,
                        TextWrapping = TextWrapping.Wrap,
                        Margin = new Thickness(10, 20, 0, 0),
                        FontWeight = FontWeights.Bold,
                        FontSize = 14,
                        Foreground = new BrushConverter().ConvertFrom("#dbb69e") as Brush
                    };

                    TextBlock priceTextBlock = new TextBlock
                    {
                        Text = $"{products[i].productPrice}zł",
                        Margin = new Thickness(10, 10, 0, 0),
                        FontWeight = FontWeights.Bold,
                        FontSize = 16,
                        Foreground = new BrushConverter().ConvertFrom("#B0927F") as Brush
                    };

                    cardGrid.Children.Add(image);
                    cardGrid.Children.Add(titleTextBlock);
                    cardGrid.Children.Add(priceTextBlock);

                    Grid.SetColumn(image, 0);
                    Grid.SetColumn(titleTextBlock, 1);
                    Grid.SetColumn(priceTextBlock, 1);
                    Grid.SetRow(titleTextBlock, 0);
                    Grid.SetRow(priceTextBlock, 1);
                    Grid.SetRowSpan(image, 2);


                    Grid.SetRow(card, i);

                    Cart.Children.Add(card);   
                }

                //wypisanie ceny

                TextBlock paymentPriceBlock = new TextBlock
                {
                    Name= "paymentPriceBlock",
                    Text = $"cena {TotalPrice.ToString("N2")} zł",
                    Margin = new Thickness(10, 20, 0, 0),
                    FontWeight = FontWeights.Bold,
                    FontSize = 20,
                    Foreground = new BrushConverter().ConvertFrom("#dbb69e") as Brush
                };

                TextBlock Shipments = new TextBlock
                {
                    Name = "Shipments",
                    Text = "+ 8,99 zł dostawa",
                    Margin = new Thickness(10, 5, 0, 0),
                    FontSize = 16,
                    Foreground = new BrushConverter().ConvertFrom("#B0927F") as Brush
                };


                TextBlock All = new TextBlock
                {
                    Name= "All",
                    Text = $"razem \n{(TotalPrice+8.99).ToString("N2")} zł",
                    Margin = new Thickness(10, 10, 0, 0),
                    FontWeight = FontWeights.Bold,
                    FontSize = 35,
                    Foreground = new BrushConverter().ConvertFrom("#dbb69e") as Brush
                };

                Grid.SetRow(paymentPriceBlock, 0);
                Grid.SetRow(Shipments, 1);
                Grid.SetRow(All, 2);

                Info.Children.Add(paymentPriceBlock);
                Info.Children.Add(Shipments);
                Info.Children.Add(All);



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
        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            if(products.Count > 0)
            {
                Checkout co = new Checkout();
                co.Show();
                this.Close();
            } else
            {
                MessageBox.Show("Koszyk jest pusty. Proszę dodać produkty do koszyka.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Dostawa_Click(object sender, RoutedEventArgs e)
        {
            UserOrder.FastDelivery = !UserOrder.FastDelivery;
            Dostawa.Foreground = UserOrder.FastDelivery ?
                new BrushConverter().ConvertFrom("#00ff00") as Brush :
                new BrushConverter().ConvertFrom("#ff0000") as Brush;
        }

        private void rabat_TextChanged(object sender, TextChangedEventArgs e)
        {
            double discountValue = 0.00;
            string rabatKod = rabat.Text;
            string res = Conection.query($"SELECT * FROM discounts WHERE code = '{rabatKod}'");
            if(res != "[]" && res != null)
            {
                List<Dictionary<string, object>> discounts = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(res);

                foreach (var discount in discounts)
                {
                    string code = (string)discount["code"];
                    discountValue = Convert.ToDouble(discount["discount"]);

                    Trace.WriteLine($"Code: {code}, Discount: {discountValue}%");
                }

                TextBlock Discount = new TextBlock
                {
                    Name = "Discount",
                    Text = $"- {discountValue}% rabatu",
                    Margin = new Thickness(10, 5, 0, 0),
                    FontSize = 16,
                    Foreground = new BrushConverter().ConvertFrom("#B0927F") as Brush
                };


                Grid.SetRow(Discount, 3);
                Info.Children.Add(Discount);
            }

        }
    }
}
