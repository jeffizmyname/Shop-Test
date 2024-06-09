using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Logika interakcji dla klasy Final.xaml
    /// </summary>
    public partial class Final : Window
    {
        int rate = 1;
        User? currentUser = login.CurrentUser;
        public Final()
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

        static int ExtractNumber(string input)
        {
            Regex regex = new Regex(@":\s*(\d+)$");

            Match match = regex.Match(input);
            if (match.Success)
            {
                return int.Parse(match.Groups[1].Value);
            }
            throw new FormatException("The input string does not contain a valid number.");
        }

        private void rate_Click(object sender, RoutedEventArgs e)
        {
            aRate.Foreground = new BrushConverter().ConvertFrom("#B0927F") as Brush;
            bRate.Foreground = new BrushConverter().ConvertFrom("#B0927F") as Brush;
            cRate.Foreground = new BrushConverter().ConvertFrom("#B0927F") as Brush;
            dRate.Foreground = new BrushConverter().ConvertFrom("#B0927F") as Brush;
            eRate.Foreground = new BrushConverter().ConvertFrom("#B0927F") as Brush;

            if (sender is UIElement element)
            {
                Trace.WriteLine(ExtractNumber(sender.ToString()));

                rate = ExtractNumber(sender.ToString());

                if (element is Control control)
                {
                    control.Foreground = new BrushConverter().ConvertFrom("#00FF00") as Brush;
                }
            }


        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            Trace.WriteLine(rate.ToString());
            Conection.query($"INSERT INTO ratings VALUES('', '{currentUser.id}','{rate}')");
            shop shop = new shop();
            shop.Show();
            this.Close();
        }
    }
}
