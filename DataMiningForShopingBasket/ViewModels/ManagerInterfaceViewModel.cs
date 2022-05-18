using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataMiningForShopingBasket.Events;
using DataMiningForShopingBasket.Models;

namespace DataMiningForShopingBasket.ViewModels
{
    class ManagerInterfaceViewModel : INotifyPropertyChanged, IChangeWindowCallerDataContext
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        #endregion

        public event ChangeWindowEventHandler ChangeWindowCalled;


        ObservableCollection<ManagerInterfaceModel> _ProductsList;
        public ObservableCollection<ManagerInterfaceModel> ProductsList
        {
            get => _ProductsList ?? (_ProductsList = GetListMIM());
            set
            {
                _ProductsList = value;
                RaisePropertyChanged(nameof(ProductsList));
            }
        }

        public ObservableCollection<ManagerInterfaceModel> DiscountsList { get; set; }

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
