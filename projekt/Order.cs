using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace projekt
{
    public class Order
    {
        public int userID { get; set; }
        public bool FastDelivery { get; set; } = false;
        public string? deliveryAddres { get; set; }
        public List<Product> products { get; set; }

        public Order() { }
        public void Initialize(int userID, List<Product> products) { 
            this.userID = userID; 
            this.products = products;
        }
    }
}
