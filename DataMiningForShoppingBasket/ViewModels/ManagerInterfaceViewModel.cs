﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using DataMiningForShoppingBasket.CommonClasses;
using DataMiningForShoppingBasket.Interfaces;
using DataMiningForShoppingBasket.Models;

namespace DataMiningForShoppingBasket.ViewModels
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

        #region IChangeWindowCallerDataContext
        public event ChangeWindowEventHandler ChangeWindowCalled;

        public string WindowLabel => "Менеджер";
        #endregion

        private ObservableCollection<CashierInterfaceModel> _cart = new ObservableCollection<CashierInterfaceModel>();
        public ObservableCollection<CashierInterfaceModel> Cart
        {
            get => _cart;
            set
            {
                _cart = value;
                RaisePropertyChanged(nameof(Cart));
            }
        }

        public List<Products> ProductsList => GetData.Products;

        public ObservableCollection<ManagerInterfaceModel> DiscountsList { get; set; }
        
    }
}
