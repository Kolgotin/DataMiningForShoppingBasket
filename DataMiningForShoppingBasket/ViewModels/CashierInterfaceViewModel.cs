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

        //todo: добавить фильтрацию по наличию на складе
        public List<Products> ProductsList => _productsList
            .Where(x => x.ProductName.ToLower().Contains(SearchString.ToLower()))
            .ToList();

        public decimal TotalCost => ConsumerCart.Sum(x => x.TotalCost);

        #region Commands
        public MyAsyncCommand ExitCommand { get; }
        public MyAsyncCommand<object> PrepareOfferCommand { get; }

        public MyCommand<object> ClearSearchCommand { get; }
        public MyCommand<Products> AddProductIntoCartCommand { get; }
        public MyCommand<object> DeleteProductFromCartCommand { get; }
        #endregion

        #endregion Properties

        public CashierInterfaceViewModel()
        {
            ExitCommand = new MyAsyncCommand(ExecuteExitAsync, 
                _ => ExitCommand?.IsActive == false);
            PrepareOfferCommand  = new MyAsyncCommand<object>(PrepareOfferAsync,
                _ => PrepareOfferCommand?.IsActive == false);
            ClearSearchCommand  = new MyCommand<object>(ClearSearch);
            AddProductIntoCartCommand = new MyCommand<Products>(AddProductIntoCartAsync);
            DeleteProductFromCartCommand  = new MyCommand<object>(DeleteProductFromCart);

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

        private Task ExecuteExitAsync()
        {
            try
            {
                ChangeWindowCalled?.Invoke(this, new AuthorizationView());
            }
            catch (Exception e)
            {
                MessageWriter.ShowMessage(e.Message);
            }

            return Task.CompletedTask;
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

        private static bool ProductIsValid(Products product) => product.ProductCost.HasValue && product.WarehouseQuantity > 0;

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
