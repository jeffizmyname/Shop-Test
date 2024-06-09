using Google.Protobuf.WellKnownTypes;
using Newtonsoft.Json;
using Org.BouncyCastle.Tls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Nodes;
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
using System.Xml.Linq;

namespace projekt
{
    /// <summary>
    /// Logika interakcji dla klasy adminPanel.xaml
    /// </summary>
    public partial class adminPanel : Window
    {
        public ObservableCollection<User> Users { get; set; }
        public ObservableCollection<Order> Orders { get; set; }
        string field = "id";
        public adminPanel()
        {
            InitializeComponent();
            Stats();
            FillOrders();
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

        private void random_Filed(object sender, RoutedEventArgs e)
        {
            string res = Conection.query($"SELECT * FROM users ORDER BY RAND() LIMIT 1");
            Trace.WriteLine(res);

            Users = JsonConvert.DeserializeObject<ObservableCollection<User>>(res);

            if (Users != null && Users.Count() > 0)
            {
                randomrec.Children.Clear();
                for (int i = 0; i < Users.Count(); i++)
                {
                    randomrec.RowDefinitions.Add(new RowDefinition { Height = new GridLength(80) });
                }

                int j = 0;
                foreach (User user in Users)
                {
                    Grid userGrid = new Grid();

                    userGrid.ColumnDefinitions.Add(new ColumnDefinition { });
                    userGrid.ColumnDefinitions.Add(new ColumnDefinition { });
                    userGrid.ColumnDefinitions.Add(new ColumnDefinition { });
                    userGrid.ColumnDefinitions.Add(new ColumnDefinition { });
                    userGrid.ColumnDefinitions.Add(new ColumnDefinition { });

                    Border card = new Border()
                    {
                        Background = new BrushConverter().ConvertFrom("#36303b") as Brush,
                        CornerRadius = new CornerRadius(15),
                        Padding = new Thickness(5),
                        Margin = new Thickness(10)
                    };

                    TextBlock name = generate_Text(user.Imie);
                    TextBlock surname = generate_Text(user.Nazwisko);
                    TextBlock id = generate_Text((user.id).ToString());
                    TextBlock login = generate_Text(user.Login);
                    TextBlock pesel = generate_Text(user.Pesel);


                    Grid.SetRow(card, j);
                    Grid.SetColumn(id, 0);
                    Grid.SetColumn(name, 1);
                    Grid.SetColumn(surname, 2);
                    Grid.SetColumn(login, 3);
                    Grid.SetColumn(pesel, 4);


                    userGrid.Children.Add(name);
                    userGrid.Children.Add(surname);
                    userGrid.Children.Add(id);
                    userGrid.Children.Add(login);
                    userGrid.Children.Add(pesel);

                    card.Child = userGrid;
                    randomrec.Children.Add(card);
                    j++;
                }
            }
        }

        private void add_Clicked(object sender, RoutedEventArgs e)
        {
            if(name.Text != "" && desc.Text != ""  && price.Text != "") 
            {
                string res = Conection.query($"INSERT INTO products VALUES('', '{name.Text}', '{desc.Text}', '{price.Text}', 'https://via.placeholder.com/262x300')");
                MessageBox.Show("Dodano produkt", "brawo", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.Yes);
            } else
            {
                MessageBox.Show("podałeś złe dane", "brawo", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.Yes);
            }
        }

        private void Filter_Clicked(object sender, RoutedEventArgs e)
        {
            id.Foreground = new BrushConverter().ConvertFrom("#B0927F") as Brush;
            imie.Foreground = new BrushConverter().ConvertFrom("#B0927F") as Brush;
            nazwisko.Foreground = new BrushConverter().ConvertFrom("#B0927F") as Brush;
            login.Foreground = new BrushConverter().ConvertFrom("#B0927F") as Brush;
            pesel.Foreground = new BrushConverter().ConvertFrom("#B0927F") as Brush;

            if (sender is UIElement element)
            {
                Trace.WriteLine(element);

                field = ExtractName(element.ToString());

                if (element is Control control)
                {
                    control.Foreground = new BrushConverter().ConvertFrom("#00FF00") as Brush;
                }


            }
        }

        private TextBlock generate_Text(string text)
        {
             TextBlock textBlock = new TextBlock()
            {
                Text = text,
                FontSize = 20,
                Foreground = new BrushConverter().ConvertFrom("#B0927F") as Brush,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(0, 10, 0, 10)
            };
            return textBlock;
        }

        private void FillOrders()
        {
            string res = Conection.query($"SELECT * FROM orders");
            Orders = JsonConvert.DeserializeObject<ObservableCollection<Order>>(res);
            List<string> productIDsList = ExtractProductIDsFromJson(res);

            Trace.WriteLine(res);

            if (Orders != null && Orders.Count() > 0)
            {
                ordersList.Children.Clear();
                for (int i = 0; i < Orders.Count(); i++)
                {
                    ordersList.RowDefinitions.Add(new RowDefinition { Height = new GridLength(80) });
                }

                int j = 0;
                foreach (Order order in Orders)
                {
                    Grid orderGrid = new Grid();

                    orderGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50)});
                    orderGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50) });
                    orderGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(200) });
                    orderGrid.ColumnDefinitions.Add(new ColumnDefinition { });
                    orderGrid.ColumnDefinitions.Add(new ColumnDefinition { });
                    orderGrid.ColumnDefinitions.Add(new ColumnDefinition { });
                    orderGrid.ColumnDefinitions.Add(new ColumnDefinition { });

                    Button button = new Button
                    {
                        Background = Brushes.Transparent,
                        BorderBrush = Brushes.Transparent,
                        Foreground = new SolidColorBrush(Color.FromRgb(176, 146, 127)),
                        FontSize = 25,
                        Content = "zmien Status",
                        Name = $"zmienStatus{j+1}"
                    };
                    button.Click += (sender, e) => Change_status(sender, e, order.status);
                    button.Style = (Style)FindResource("btnHover");

                    Border card = new Border()
                    {
                        Background = new BrushConverter().ConvertFrom("#36303b") as Brush,
                        CornerRadius = new CornerRadius(15),
                        Padding = new Thickness(5),
                        Margin = new Thickness(10)
                    };

                    TextBlock id = generate_Text((j + 1).ToString());
                    TextBlock userId = generate_Text((order.userID).ToString());
                    TextBlock fast = generate_Text(order.FastDelivery ? "true" : "");
                    TextBlock products = generate_Text(productIDsList[j].Substring(1));
                    TextBlock shipAddr = generate_Text(order.shipAddres);
                    TextBlock status = generate_Text(order.status);

                    Grid.SetRow(card, j);
                    Grid.SetColumn(id, 0);
                    Grid.SetColumn(userId, 1);
                    Grid.SetColumn(fast, 2);
                    Grid.SetColumn(products, 3);
                    Grid.SetColumn(shipAddr, 4);
                    Grid.SetColumn(status, 5);
                    Grid.SetColumn(button, 6);

                    orderGrid.Children.Add(id);
                    orderGrid.Children.Add(userId);
                    orderGrid.Children.Add(fast);
                    orderGrid.Children.Add(products);
                    orderGrid.Children.Add(shipAddr);
                    orderGrid.Children.Add(status);
                    orderGrid.Children.Add(button);

                    card.Child = orderGrid;
                    ordersList.Children.Add(card);
                    j++;

                }
            }
        }

        private void Change_status(object sender, RoutedEventArgs e, string status)
        {
            Button btn = (Button)sender;
            string booooo = status == "in prep" ? "done" : "in prep";
            Trace.WriteLine((btn.Name).Substring(11));
            string res = Conection.query($"UPDATE orders SET status = '{booooo}' WHERE id = {(btn.Name).Substring(11)}");
            Trace.WriteLine($"UPDATE orders SET status = '{booooo}' WHERE id = {(btn.Name).Substring(11)}");
            FillOrders();
        }

        private void Run_query(object sender, RoutedEventArgs e)
        {
            string res = Conection.query($"SELECT * FROM users WHERE {field} LIKE '%{value.Text}%'");
            Trace.WriteLine(res);

            Users = JsonConvert.DeserializeObject<ObservableCollection<User>>(res);

            if (Users != null && Users.Count() > 0)
            {
                results.Children.Clear();
                for (int i = 0; i < Users.Count(); i++)
                {
                    results.RowDefinitions.Add(new RowDefinition { Height = new GridLength(80) });
                }

                int j = 0;
                foreach (User user in Users)
                {
                    Grid userGrid = new Grid();

                    userGrid.ColumnDefinitions.Add(new ColumnDefinition { });
                    userGrid.ColumnDefinitions.Add(new ColumnDefinition { });
                    userGrid.ColumnDefinitions.Add(new ColumnDefinition { });
                    userGrid.ColumnDefinitions.Add(new ColumnDefinition { });
                    userGrid.ColumnDefinitions.Add(new ColumnDefinition { });

                    Border card = new Border()
                    {
                        Background = new BrushConverter().ConvertFrom("#36303b") as Brush,
                        CornerRadius = new CornerRadius(15),
                        Padding = new Thickness(5),
                        Margin = new Thickness(10)
                    };

                    TextBlock name = generate_Text(user.Imie);
                    TextBlock surname = generate_Text(user.Nazwisko);
                    TextBlock id = generate_Text((user.id).ToString());
                    TextBlock login = generate_Text(user.Login);
                    TextBlock pesel = generate_Text(user.Pesel);


                    Grid.SetRow(card, j);
                    Grid.SetColumn(id, 0);
                    Grid.SetColumn(name, 1);
                    Grid.SetColumn(surname, 2);
                    Grid.SetColumn(login, 3);
                    Grid.SetColumn(pesel, 4);


                    userGrid.Children.Add(name);
                    userGrid.Children.Add(surname);
                    userGrid.Children.Add(id);
                    userGrid.Children.Add(login);
                    userGrid.Children.Add(pesel);

                    card.Child = userGrid;
                    results.Children.Add(card);
                    j++;
                }
            }

        }
        static List<string> ExtractProductIDsFromJson(string json)
        {
            List<string> productIDsList = new List<string>();

            int startIndex = json.IndexOf("\"productIDs\":\"[");
            while (startIndex != -1)
            {
                int endIndex = json.IndexOf("]\"", startIndex);
                if (endIndex != -1)
                {
                    string productIDs = json.Substring(startIndex + 14, endIndex - startIndex - 14); // 14 is the length of "\"productIDs\":\"["
                    productIDsList.Add(productIDs);
                    startIndex = json.IndexOf("\"productIDs\":\"[", endIndex);
                }
                else
                {
                    break;
                }
            }

            return productIDsList;
        }

        static string ExtractName(string input)
        {
            int colonIndex = input.IndexOf(':');
            if (colonIndex != -1 && colonIndex + 1 < input.Length)
            {
                string name = input.Substring(colonIndex + 1).Trim();
                return name;
            }
            throw new FormatException("The input string does not contain a valid name.");
        }

        static double ExtractNumber(string input)
        {
            Regex regex = new Regex(@":\s*(-?\d+(\.\d+)?)");

            Match match = regex.Match(input);
            if (match.Success)
            {
                return double.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
            }
            throw new FormatException("The input string does not contain a valid number.");
        }

        static int ExtractNumberName(string input)
        {
            Regex regex = new Regex(@":\s*(\d+)$");

            Match match = regex.Match(input);
            if (match.Success)
            {
                return int.Parse(match.Groups[1].Value);
            }
            throw new FormatException("The input string does not contain a valid number.");
        }

        

        private void AddTextBlockToBorder(string borderName, string titleText, string text)
        {
            Border border = (Border)FindName(borderName);
            if (border != null)
            {
                Grid grid = new Grid();
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40) });
                grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(40) });


                TextBlock title = new TextBlock()
                {
                    Text = titleText,
                    Foreground = new BrushConverter().ConvertFrom("#B0927F") as Brush,
                    FontSize = 25,
                    FontWeight = FontWeights.Bold,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left
                };
                TextBlock textBlock = new TextBlock
                {
                    Text = text,
                    Foreground = new BrushConverter().ConvertFrom("#B0927F") as Brush,
                    FontSize = 20,
                    VerticalAlignment = VerticalAlignment.Top,
                    HorizontalAlignment = HorizontalAlignment.Left
                };

                Grid.SetRow(title, 0);
                Grid.SetRow(textBlock, 1);

                grid.Children.Add(title);
                grid.Children.Add(textBlock);

                border.Child = grid;

            }
        }

        private void Stats()
        {
            String zamowieniaIle = Conection.query("SELECT COUNT(*) orders FROM orders");
            String uzytkownicyIle = Conection.query("SELECT COUNT(*) users FROM users");
            String wykonaneZamowieniaIle = Conection.query("SELECT COUNT(*) finished FROM orders WHERE status = 'done'");
            String sredniaOcenauzytkownikow = Conection.query("SELECT AVG(rating) rating FROM ratings");

            Trace.WriteLine(
                ExtractNumber(zamowieniaIle) + " " + 
                ExtractNumber(uzytkownicyIle) + " " + 
                ExtractNumber(wykonaneZamowieniaIle) + " " + 
                ExtractNumber(sredniaOcenauzytkownikow));

            AddTextBlockToBorder("stat1", "Liczba zamówień" , ExtractNumber(zamowieniaIle).ToString());
            AddTextBlockToBorder("stat2", "Liczba użytkowników" , ExtractNumber(uzytkownicyIle).ToString());
            AddTextBlockToBorder("stat3", "wykonane zamówienia", ExtractNumber(wykonaneZamowieniaIle).ToString());
            AddTextBlockToBorder("stat4", "Srednia ocena usług", ExtractNumber(sredniaOcenauzytkownikow).ToString());

        }
    }
}
