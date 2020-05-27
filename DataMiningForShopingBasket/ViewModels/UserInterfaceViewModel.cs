using DataMiningForShopingBasket.Events;
using DataMiningForShopingBasket.Models;
using DataMiningForShopingBasket.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMiningForShopingBasket.ViewModels
{
    class UserInterfaceViewModel : INotifyPropertyChanged, IChangeWindowCallerDataContext
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        #endregion

        public event ChangeWindowEventHandler ChangeWindowCalled;

        #region Commands
        private MyCommand _ButtonClickCommand;
        public MyCommand ButtonClickCommand =>
            _ButtonClickCommand ?? (_ButtonClickCommand =
                new MyCommand((obj) => true, (obj) => ButtonClick_Handler()));
        #endregion

        ObservableCollection<UserInterfaceModel> _ProductsList;
        public ObservableCollection<UserInterfaceModel> ProductsList
        {
            get => _ProductsList ?? (_ProductsList = GetListUIM());
            set
            {
                _ProductsList = value;
                RaisePropertyChanged("ProductsList");
            }
        }

        public UserInterfaceViewModel()
        {
        }

        private ObservableCollection<UserInterfaceModel> GetListUIM()
        {
            var a = new ObservableCollection<UserInterfaceModel>();
            a.Add(new UserInterfaceModel(1, "zzzz"));
            a.Add(new UserInterfaceModel(2, "xxxx"));
            a.Add(new UserInterfaceModel(3, "ssss"));
            a.Add(new UserInterfaceModel(4, "aaaa"));
            a.Add(new UserInterfaceModel(5, "yyyy"));
            return a;
        }

        private void ButtonClick_Handler()
        {
            ChangeWindowCalled(this, new AuthorizationView());
        }
    }
}
