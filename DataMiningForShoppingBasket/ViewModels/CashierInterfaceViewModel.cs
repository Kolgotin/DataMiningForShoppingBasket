using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using DataMiningForShoppingBasket.CommonClasses;
using DataMiningForShoppingBasket.Events;
using DataMiningForShoppingBasket.Handlers;
using DataMiningForShoppingBasket.Interfaces;
using DataMiningForShoppingBasket.Models;
using DataMiningForShoppingBasket.Views;

namespace DataMiningForShoppingBasket.ViewModels
{
    public class CashierInterfaceViewModel : INotifyPropertyChanged, IChangeWindowCallerDataContext
    {
        private readonly IGetData _getData;
        private readonly IPrepareOfferHandler _prepareOfferHandler;

        private MyAsyncCommand<object> _exitCommand;
        private MyAsyncCommand<object> _prepareOfferCommand;
        private MyCommand<object> _clearSearchCommand;
        private MyCommand<Products> _productItemDoubleClickCommand; 
        private MyCommand<object> _deleteProductFromCartCommand;
        private string _searchString;
        private List<Products> _offerProductList;
        private List<Products> _productsList;

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

        public MyCommand<object> DeleteProductFromCartCommand =>
            _deleteProductFromCartCommand ?? (_deleteProductFromCartCommand =
                new MyCommand<object>(DeleteProductFromCart, obj => true));
        #endregion

        public ObservableCollection<CashierInterfaceModel> ConsumerCart { get; set; }

        public CashierInterfaceModel SelectedCartItem { get; set; }

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

        public List<Products> ProductsList => _productsList
            .Where(x => x.ProductName.ToLower().Contains(SearchString.ToLower()))
            .ToList();

        public decimal TotalCost => ConsumerCart.Sum(x => x.TotalCost);
        #endregion Properties
        
        public CashierInterfaceViewModel()
        {
            _productsList = new List<Products>();
            _getData = GetData.Instance;
            _ = RefreshActualProductsAsync();
            ConsumerCart = new ObservableCollection<CashierInterfaceModel>();
            SearchString = string.Empty;
            _prepareOfferHandler = PrepareOfferSimpleHandler.Instance;
        }

        private async Task PrepareOfferAsync(object obj)
        {
            try
            {
                var productsInCart = ConsumerCart.Select(x => x.ProductInstance);
                OfferProductList = await _prepareOfferHandler.PrepareOffer(productsInCart);
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

                if (ConsumerCart.Select(x => x.ProductInstance).Contains(product))
                {
                    ConsumerCart.First(x => x.ProductInstance == product).Quantity++;
                }
                else
                {
                    var newProduct = new CashierInterfaceModel(product);
                    newProduct.PropertyChanged += (s, e) => RaisePropertyChanged(nameof(TotalCost));
                    ConsumerCart.Add(newProduct);
                }

                RaisePropertyChanged(nameof(TotalCost));
            }
            catch (Exception e)
            {
                MessageWriter.ShowMessage(e.Message);
            }
        }

        private void DeleteProductFromCart(object item)
        {
            if (SelectedCartItem is null)
            {
                return;
            }

            _ = ConsumerCart.Remove(SelectedCartItem);
        }

        private bool ProductIsValid(Products product) => product.Cost.HasValue && product.WarehouseQuantity > 0;

        private async Task RefreshActualProductsAsync()
        {
            try
            {
                if (_getData is null)
                {
                    return;
                }

                _productsList = await _getData.GetProductsAsync();
                RaisePropertyChanged(nameof(ProductsList));
            }
            catch (Exception e)
            {
                MessageWriter.ShowMessage(e.Message);
            }
        }
    }
}
