﻿using System;
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
using System.Xml.Linq;

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

            string prods = JsonSerializer.Serialize(productIds);

            if (string.IsNullOrEmpty(kraj) || string.IsNullOrEmpty(miastio) || string.IsNullOrEmpty(ulica) || string.IsNullOrEmpty(nr))
            {
                MessageBox.Show("Wszystkie pola adresu muszą być wypełnione.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (blikGrid.Visibility == Visibility.Visible)
            {
                if (string.IsNullOrEmpty(Blik.Text))
                {
                    MessageBox.Show("Kod BLIK jest wymagany.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            else
            {
                if (string.IsNullOrEmpty(KartNumer.Text) || string.IsNullOrEmpty(Cardholder.Text) || string.IsNullOrEmpty(ccv.Text))
                {
                    MessageBox.Show("Wszystkie pola danych karty są wymagane.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }

            Trace.WriteLine($"INSERT INTO orders VALUES('', '{userOrder.userID}', '{userOrder.FastDelivery}', '{prods}', '{addr}')");
            string res = Conection.query($"INSERT INTO orders VALUES('', '{userOrder.userID}', '{userOrder.FastDelivery}', '{prods}', '{addr}')");
            Trace.WriteLine(res);
            Final end = new Final();
            end.Show();
            this.Close();
        }
    }
}
