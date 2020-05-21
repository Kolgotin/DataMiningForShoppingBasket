﻿using DataMiningForShopingBasket.Events;
using DataMiningForShopingBasket.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMiningForShopingBasket.ViewModels
{
    class AuthorizationViewModel : INotifyPropertyChanged, IChangeWindowCallerDataContext
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
        private MyCommand _LoginClickCommand;
        public MyCommand LoginClickCommand =>
            _LoginClickCommand ?? (_LoginClickCommand =
                new MyCommand((obj) => true, (obj) => LoginClick_Handler()));
        #endregion

        public string Login { get; set; } = "qqqaaaaaaa";
        public string Password { get; set; }

        private void LoginClick_Handler()
        {
            ChangeWindowCalled(this, new UserInterfaceView());
        }
    }
}
