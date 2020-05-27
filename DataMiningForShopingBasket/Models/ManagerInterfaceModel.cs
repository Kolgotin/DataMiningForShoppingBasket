using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMiningForShopingBasket.Models
{
    class ManagerInterfaceModel : Products
    {
        public ManagerInterfaceModel(int i, string s)
        {
            id = i;
            ProductName = s;
        }
    }
}
