using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManageMagazine
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Manufacturer { get; set; }
        public double PurchasePrice { get; set; }
        public double SalePrice { get; set; }
        public int Quantity { get; set; }
      
    }
}
