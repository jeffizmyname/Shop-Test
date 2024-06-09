using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projekt
{
    public class User
    {
        public int id { get; set; }
        public String? Imie { get; set; }
        public String? Nazwisko { get; set; }
        public String? Login { get; set; }
        public String? haslo { get; set; }
        public String? role { get; set; }

        public List<Product> ShoppingCart = new List<Product>();

        public void addProduct(Product product)
        {
            ShoppingCart.Add(product);
        }

        public void removeProduct(Product product)
        {
            ShoppingCart.Remove(product);
        }
        public List<Product> getShoppingCart() { return ShoppingCart; }

    }
}
