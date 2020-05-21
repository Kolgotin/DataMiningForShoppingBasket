using DataMiningForShopingBasket.Events;
using DataMiningForShopingBasket.Views;
using System;
using System.Collections.Generic;
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

        public string ButtonText { get; set; } = "oooooook";

        #region Commands
        private MyCommand _ButtonClickCommand;
        public MyCommand ButtonClickCommand =>
            _ButtonClickCommand ?? (_ButtonClickCommand =
                new MyCommand((obj) => true, (obj) => ButtonClick_Handler()));
        #endregion

        private void ButtonClick_Handler()
        {
            ChangeWindowCalled(this, new AuthorizationView());
        }
    }
}
