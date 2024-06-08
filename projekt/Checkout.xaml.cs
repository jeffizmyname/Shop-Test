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
    /// Logika interakcji dla klasy Window1.xaml
    /// </summary>
    public partial class Checkout : Window
    {
        public Checkout()
        {
            InitializeComponent();
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
            payMethode.Children.Clear();
            TextBlock tb = new TextBlock()
            {
                Text= "karta in",
            };
            payMethode.Children.Add(tb);
        }

        private void blik_Click(object sender, RoutedEventArgs e)
        {
            payMethode.Children.Clear();
            TextBlock tb = new TextBlock()
            {
                Text = "blik in",
            };
            payMethode.Children.Add(tb);

        }
    }
}
