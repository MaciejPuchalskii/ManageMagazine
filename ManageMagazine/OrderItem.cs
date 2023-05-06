using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ManageMagazine
{
    public class OrderItem
    {
        public static int OrderItemId { get; set; }

        public static int NextID = 1;

        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }

        public Product Product { get; set; }

        public string DisplayInfo => $"{Product.Name}" + $" : {Product.Manufacturer}" + $" : {Quantity}" + $" : {Product.SalePrice}" + "$";


        public OrderItem(int orderID, int quantity, Product product)
        {
            OrderItemId = NextID;
            NextID++;
            OrderID = orderID;
            ProductID = product.Id;
            Quantity = quantity;
            Price = product.SalePrice * quantity;
            Product = product;
        }


    }
}