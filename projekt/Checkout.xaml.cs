using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.Json;
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
    /// Logika interakcji dla klasy Window1.xaml
    /// </summary>
    public partial class Checkout : Window
    {

        Order? userOrder = ShoppingCart.UserOrder;
        User? currentUser = login.CurrentUser;
        public Checkout()
        {
            InitializeComponent();
            blikGrid.Visibility= Visibility.Visible;
            kartaGrid.Visibility= Visibility.Collapsed;
            if(currentUser != null)
            {
                List<Product> cart = currentUser.ShoppingCart;
                userOrder.Initialize(currentUser.id, cart);
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

        private void karta_Click(object sender, RoutedEventArgs e)
        {
            kartaGrid.Visibility= Visibility.Visible;
            blikGrid.Visibility= Visibility.Collapsed;
        }

        private void blik_Click(object sender, RoutedEventArgs e)
        {
            blikGrid.Visibility= Visibility.Visible;
            kartaGrid.Visibility= Visibility.Collapsed;
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            string kraj = Kraj.Text;
            string miastio = Miasto.Text;
            string ulica = Ulica.Text;
            string nr = Nr.Text;
            string addr = kraj + " " + miastio + " " + ulica + " " + nr;
            List<int> productIds = new List<int>();
            foreach (var product in currentUser.ShoppingCart)
            {
                productIds.Add(product.id);
            }

            // Convert the list of IDs to a JSON array
            string prods = JsonSerializer.Serialize(productIds);

            if(blikGrid.Visibility == Visibility.Visible) {

            } else
            {

            }

            Trace.WriteLine($"INSERT INTO orders VALUES(NULL, '{userOrder.userID}', '{userOrder.FastDelivery}', '{prods}', '{addr}')");
        }
    }
}
