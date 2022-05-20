using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using DataMiningForShoppingBasket.CommonClasses;
using DataMiningForShoppingBasket.Events;
using DataMiningForShoppingBasket.Interfaces;
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
        
        #region IChangeWindowCallerDataContext
        public event ChangeWindowEventHandler ChangeWindowCalled;
        #endregion

        #region Commands
        private MyAsyncCommand<object> _exitCommand;
        public MyAsyncCommand<object> ExitCommand =>
            _exitCommand ?? (_exitCommand =
                new MyAsyncCommand<object>(ExitHandlerAsync, obj => _exitCommand?.IsActive == false));

        private MyCommand<Products> _productItemDoubleClickCommand;
        public MyCommand<Products> ProductItemDoubleClickCommand =>
            _productItemDoubleClickCommand ?? (_productItemDoubleClickCommand =
                new MyCommand<Products>(AddProductIntoCartAsync, obj => true));
        
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

        public decimal TotalCost => Cart.Sum(x => x.TotalCost);

        public List<Products> ProductsList => GetData.Products;
        
        private async Task ExitHandlerAsync(object obj)
        {
            try
            {
                ChangeWindowCalled?.Invoke(this, new AuthorizationView());
            }
            catch (Exception e)
            {
                MessageWriter.ShowMessage(e.Message);
            }
        }

        private void AddProductIntoCartAsync(Products product)
        {
            try
            {
                if (!ProductIsValid(product))
                {
                    return;
                }

                var newProduct = new CashierInterfaceModel(product);
                newProduct.PropertyChanged += (s,e) => RaisePropertyChanged(nameof(TotalCost));
                Cart.Add(newProduct);
            }
            catch (Exception e)
            {
                MessageWriter.ShowMessage(e.Message);
            }
        }
        
        private bool ProductIsValid(Products product) => product.Cost.HasValue;

        private ObservableCollection<CashierInterfaceModel> GetFakeList()
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
    }
}
