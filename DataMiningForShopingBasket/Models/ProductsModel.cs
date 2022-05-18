using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMiningForShopingBasket.Models
{
    class ProductsModel : Products
    {
        public int Count { get; set; }

        public ProductsModel(int i, string s)
        {
            id = i;
            ProductName = s;
        }
    }
}
