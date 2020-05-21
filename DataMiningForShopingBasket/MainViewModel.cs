using DataMiningForShopingBasket.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace DataMiningForShopingBasket
{
    public class MainViewModel
    {
        public object CurrentUC { get; set; } = new AuthorizationView();
    }
}
