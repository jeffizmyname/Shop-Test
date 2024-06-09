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
    /// Logika interakcji dla klasy register.xaml
    /// </summary>
    public partial class register : Window
    {
        public register()
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

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            string imie = userName.Text;
            string nazwisko = userSurname.Text;
            string login = userLogin.Text;
            string pesel = userPesel.Text;
            string haslo = userPass.Text;
            String hash = PasswordHasher.Hash(haslo);

            if (!string.IsNullOrEmpty(imie) && !string.IsNullOrEmpty(nazwisko) && !string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(pesel) && !string.IsNullOrEmpty(haslo))
            {
                if (Conection.query($"SELECT * FROM users WHERE login = '{login}'") == "[]")
                {
                    if (Conection.query($"SELECT * FROM users WHERE pesel = '{pesel}'") == "[]")
                    {
                        Conection.query($"INSERT INTO users VALUES ('', '{imie}','{nazwisko}','{login}','{hash}','{pesel}', 'U')");
                        login log = new login();
                        log.Show();
                        this.Close();
                    }
                    else
                    {
                        // Błąd PESEL
                        MessageBox.Show("PESEL już istnieje. Proszę wprowadzić inny PESEL.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    // Błąd loginu
                    MessageBox.Show("Login już istnieje. Proszę wybrać inny login.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                // Błąd wypełnienia pól
                MessageBox.Show("Wszystkie pola są wymagane. Proszę wypełnić wszystkie dane.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoginRedirect_Click(object sender, RoutedEventArgs e)
        {
            login log = new login();
            log.Show();
            this.Close();
        }
    }
}
