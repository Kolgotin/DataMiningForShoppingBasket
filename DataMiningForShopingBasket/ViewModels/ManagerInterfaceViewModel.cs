using DataMiningForShopingBasket.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMiningForShopingBasket.ViewModels
{
    class ManagerInterfaceViewModel
    {
        UserInterfaceViewModel UsIntDC = new UserInterfaceViewModel();

        private ObservableCollection<ManagerInterfaceModel> GetListMIM()
        {
            var a = new ObservableCollection<ManagerInterfaceModel>();
            a.Add(new ManagerInterfaceModel(1, "zzzz"));
            a.Add(new ManagerInterfaceModel(2, "xxxx"));
            a.Add(new ManagerInterfaceModel(3, "ssss"));
            a.Add(new ManagerInterfaceModel(4, "aaaa"));
            a.Add(new ManagerInterfaceModel(5, "yyyy"));
            return a;
        }
    }
}
