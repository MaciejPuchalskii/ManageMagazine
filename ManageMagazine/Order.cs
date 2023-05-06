 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageMagazine
{
    public class Order
    {
        public int OrderId { get; set; }

        public int CustomerId { get; set; }

        public List<OrderItem> OrderItems { get; set; }

        public int TotalQuantity
        {
            get
            {
                int qty = 0;
                foreach (OrderItem o in OrderItems)
                {
                    qty += o.Quantity;
                }
                return qty;
            }
            set
            {

            }
           
        }
       

        public double Sum 
        {
            get
            {
                double sum = 0;
                foreach(OrderItem o in OrderItems)
                {
                    sum += o.Price;
                }
                return sum;
            } 
            set
            {

            }
        }

        public Order()
        {
            OrderItems = new List<OrderItem>();
        }

        public Order(int id, int customerId, double sum)
        {
            OrderId = id;
            CustomerId = customerId;
            Sum = sum;
            OrderItems = new List<OrderItem>();

        }
    }
}
