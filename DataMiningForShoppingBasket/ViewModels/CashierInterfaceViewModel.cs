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
        private const int MaxOfferedProducts = 5;

        private MyAsyncCommand<object> _exitCommand;
        private MyAsyncCommand<object> _prepareOfferCommand;
        private MyCommand<object> _clearSearchCommand;
        private MyCommand<Products> _productItemDoubleClickCommand;
        private string _searchString;
        private List<Products> _offerProductList;

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        #endregion

        #region IChangeWindowCallerDataContext
        public event ChangeWindowEventHandler ChangeWindowCalled;

        public string WindowLabel => "Кассир";
        #endregion

        #region Properties
        #region Commands
        public MyAsyncCommand<object> ExitCommand =>
            _exitCommand ?? (_exitCommand =
                new MyAsyncCommand<object>(ExitHandlerAsync, obj => _exitCommand?.IsActive == false));
        
        public MyAsyncCommand<object> PrepareOfferCommand =>
            _prepareOfferCommand ?? (_prepareOfferCommand =
                new MyAsyncCommand<object>(PrepareOfferAsync, obj => _prepareOfferCommand?.IsActive == false));

        public MyCommand<object> ClearSearchCommand =>
            _clearSearchCommand ?? (_clearSearchCommand =
                new MyCommand<object>(ClearSearch, _ => true));

        
        public MyCommand<Products> ProductItemDoubleClickCommand =>
            _productItemDoubleClickCommand ?? (_productItemDoubleClickCommand =
                new MyCommand<Products>(AddProductIntoCartAsync, obj => true));
        #endregion

        public ObservableCollection<CashierInterfaceModel> Cart { get; set; }

        public List<Products> OfferProductList
        {
            get => _offerProductList;
            set
            {
                _offerProductList = value;
                RaisePropertyChanged(nameof(OfferProductList));
            }
        }

        public string SearchString
        {
            get => _searchString;
            set
            {
                _searchString = value;
                RaisePropertyChanged(nameof(SearchString));
                RaisePropertyChanged(nameof(ProductsList));
            }
        }

        public List<Products> ProductsList => GetData.Products
            .Where(x => x.ProductName.ToLower().Contains(SearchString.ToLower()))
            .ToList();

        public decimal TotalCost => Cart.Sum(x => x.TotalCost);
        #endregion Properties

        public CashierInterfaceViewModel()
        {
            Cart = new ObservableCollection<CashierInterfaceModel>();
            SearchString = string.Empty;
        }

        private async Task PrepareOfferAsync(object obj)
        {
            try
            {
                OfferProductList = GetData.Products
                    .Except(Cart.Select(x => x.ProductInstance)).Take(MaxOfferedProducts).ToList();
            }
            catch (Exception e)
            {
                MessageWriter.ShowMessage(e.Message);
            }
        }

        private void ClearSearch(object obj)
        {
            SearchString = string.Empty;
        }

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

                if (Cart.Select(x => x.ProductInstance).Contains(product))
                {
                    Cart.First(x => x.ProductInstance == product).Quantity++;
                }
                else
                {
                    var newProduct = new CashierInterfaceModel(product);
                    newProduct.PropertyChanged += (s, e) => RaisePropertyChanged(nameof(TotalCost));
                    Cart.Add(newProduct);
                }

                RaisePropertyChanged(nameof(TotalCost));
            }
            catch (Exception e)
            {
                MessageWriter.ShowMessage(e.Message);
            }
        }

        private bool ProductIsValid(Products product) => product.Cost.HasValue;
    }
}
