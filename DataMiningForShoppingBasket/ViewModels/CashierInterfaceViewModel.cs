using System.Collections.ObjectModel;
using System.ComponentModel;
using DataMiningForShoppingBasket.Events;
using DataMiningForShoppingBasket.Models;
using DataMiningForShoppingBasket.Views;

namespace DataMiningForShoppingBasket.ViewModels
{
    public class CashierInterfaceViewModel : INotifyPropertyChanged, IChangeWindowCallerDataContext
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

        ObservableCollection<CashierInterfaceModel> _productsList;
        public ObservableCollection<CashierInterfaceModel> ProductsList
        {
            get => _productsList ?? (_productsList = GetListUIM());
            set
            {
                _productsList = value;
                RaisePropertyChanged(nameof(ProductsList));
            }
        }
        
        private ObservableCollection<CashierInterfaceModel> GetListUIM()
        {
            return new ObservableCollection<CashierInterfaceModel>
            {
                new CashierInterfaceModel(1, "zzzz"),
                new CashierInterfaceModel(2, "xxxx"),
                new CashierInterfaceModel(3, "ssss"),
                new CashierInterfaceModel(4, "aaaa"),
                new CashierInterfaceModel(5, "yyyy"),
            };
        }

        private void ButtonClick_Handler()
        {
            ChangeWindowCalled(this, new AuthorizationView());
        }
    }
}
