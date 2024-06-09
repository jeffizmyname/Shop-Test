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
    /// Logika interakcji dla klasy login.xaml
    /// </summary>
    public partial class login : Window
    {
        public static User? CurrentUser { get; private set; }

        public login()
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
            String login = userLogin.Text;
            String pass = userPass.Text;
            String hash = PasswordHasher.Hash(pass);

            Trace.WriteLine(hash);

            if (!string.IsNullOrEmpty(login) && !string.IsNullOrEmpty(pass))
            {
                string res = Conection.query($"SELECT * FROM users WHERE login = '{login}'");
                Trace.WriteLine(res);
                List<User> user = JsonConvert.DeserializeObject<List<User>>(res);

                if (user != null && user.Count() > 0)
                {
                    if (PasswordHasher.Verify(pass, user[0].haslo))
                    {
                        CurrentUser = user[0];

                        if (user[0].role == "A")
                        {
                            adminPanel panel = new adminPanel();
                            panel.Show();
                        }
                        else
                        {
                            shop shop = new shop();
                            shop.Show();
                        }

                        this.Close();
                        Trace.WriteLine("Zalogowano");
                    }
                    else
                    {
                        // Błąd hasła
                        MessageBox.Show("Nieprawidłowe hasło. Spróbuj ponownie.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    // Błąd loginu
                    MessageBox.Show("Nie znaleziono użytkownika o podanym loginie. Spróbuj ponownie.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                // Błąd wypełnienia pól
                MessageBox.Show("Wszystkie pola są wymagane. Proszę wypełnić wszystkie dane.", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RegisterRedirect_Click(object sender, RoutedEventArgs e)
        {
            register reg = new register();
            reg.Show();
            this.Close();
        }
    }
}
