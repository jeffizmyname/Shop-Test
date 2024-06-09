using Org.BouncyCastle.Tls;
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
    /// Logika interakcji dla klasy adminPanel.xaml
    /// </summary>
    public partial class adminPanel : Window
    {
        public adminPanel()
        {
            InitializeComponent();
            Stats();
        }

        private void TopBorder_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void LogoutBtn_Click(object sender, RoutedEventArgs e)
        {
            login login = new login();
            login.Show();
            this.Close();
        }

        private void Stats()
        {
            String zamowieniaIle = Conection.query("SELECT COUNT(*) orders FROM orders");
            String uzytkownicyIle = Conection.query("SELECT COUNT(*) users FROM users");
            String wykonaneZamowieniaIle;
            String sredniaOcenauzytkownikow;
        }
    }
}
