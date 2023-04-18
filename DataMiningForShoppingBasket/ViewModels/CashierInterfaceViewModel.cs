﻿using DataMiningForShoppingBasket.Commands;
using DataMiningForShoppingBasket.Common;
using DataMiningForShoppingBasket.Handlers;
using DataMiningForShoppingBasket.Interfaces;
using DynamicData;
using DynamicData.Binding;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace DataMiningForShoppingBasket.ViewModels
{
    public class CashierInterfaceViewModel : NotifyPropertyChangedImplementation,
        ILabelHavingDataContext, IDisposable
    {
        private readonly IDbManager _dbManager;
        private readonly IPrepareOfferHandler _prepareOfferHandler;
        private readonly INotifier<Products, int> _productsINotifier;
        private readonly CompositeDisposable _cleanup;

        private string _searchString;
        private List<AdditionalOfferViewModel> _offerProductList;
        private readonly ReadOnlyObservableCollection<Products> _productsList;

        #region ILabelHavingDataContext

        public string WindowLabel => "Кассир";

        #endregion

        #region Properties

        public ObservableCollection<CartRowViewModel> ConsumerCart { get; set; }

        public CartRowViewModel SelectedCartRowItem { get; set; }

        public List<AdditionalOfferViewModel> OfferProductList
        {
            get => _offerProductList;
            set => SetProperty(ref _offerProductList, value);
        }

        public string SearchString
        {
            get => _searchString;
            set => SetProperty(ref _searchString, value);
        }

        //todo: добавить фильтрацию по наличию на складе
        public ReadOnlyObservableCollection<Products> ProductsList => _productsList;

        public decimal TotalCost => ConsumerCart.Sum(x => x.TotalCost);

        #region Commands

        public MyCommand<Products> AddProductIntoCartCommand { get; }
        public MyAsyncCommand PrepareOfferCommand { get; }
        public MyAsyncCommand FinalizeSaleCommand { get; }

        public MyCommand ClearSearchCommand { get; }
        public MyCommand DeleteProductFromCartCommand { get; }

        #endregion

        #endregion Properties

        public CashierInterfaceViewModel()
        {
            _dbManager = DbManager.GetInstance();
            _productsINotifier = DefaultNotifier<Products, int>.GetInstance();
            _prepareOfferHandler = AprioriAlgorithm.GetInstance();

            //todo: вызвать по-нормальному
            var init = new MyAsyncCommand(InitializeExecuteAsync);
            init.Execute();

            ConsumerCart = new ObservableCollection<CartRowViewModel>();
            SearchString = string.Empty;

            PrepareOfferCommand = new MyAsyncCommand(ExecutePrepareOfferAsync,
                _ => PrepareOfferCommand?.IsActive == false);
            FinalizeSaleCommand = new MyAsyncCommand(ExecuteFinalizeSaleAsync);
            ClearSearchCommand = new MyCommand(ExecuteClearSearch);
            AddProductIntoCartCommand = new MyCommand<Products>(ExecuteAddProductIntoCartAsync);
            DeleteProductFromCartCommand = new MyCommand(ExecuteDeleteProductFromCart);

            var productsChangeDisposable = _productsINotifier.Changes
                .Filter(
                    this.WhenValueChanged(x => x.SearchString)
                        .Select(SearchFilter))
                .Bind(out _productsList)
                .Subscribe();

            _cleanup = new CompositeDisposable();
            _cleanup.Add(productsChangeDisposable);
        }

        private async Task InitializeExecuteAsync()
        {
            var productList = await _dbManager.GetListAsync<Products>();
            productList.ForEach(_productsINotifier.NotifyAdd);
        }

        private async Task ExecutePrepareOfferAsync()
        {
            try
            {
                var productsInCart = ConsumerCart.Select(x => x.Product).ToList();
                var productsList = await _prepareOfferHandler.PrepareOffer(productsInCart);
                OfferProductList = productsList
                    .Select(x => new AdditionalOfferViewModel(x.Key, x.Value))
                    .OrderByDescending(x => x.Confidence)
                    .ToList();
            }
            catch (Exception e)
            {
                MessageWriter.ShowMessage(e.Message);
            }
        }

        private async Task ExecuteFinalizeSaleAsync()
        {
            try
            {
                var receipt = new SaleReceipts
                {
                    SaleDateTime = DateTime.Now,
                    CashierId = CurrentSession.CurrentUser.Id,
                    ClientId = null,
                    SaleRows = ConsumerCart.Select(x =>
                        new SaleRows()
                        {
                            ProductId = x.Product.Id,
                            Quantity = x.Quantity,
                            TotalCost = x.TotalCost
                        }).ToList()
                };

                await _dbManager.SaveSale(receipt);
                ConsumerCart.Clear();
                OfferProductList = null;
                RaisePropertyChanged(nameof(TotalCost));
            }
            catch (Exception e)
            {
                MessageWriter.ShowMessage(e.Message);
            }
        }

        private void ExecuteClearSearch()
        {
            SearchString = string.Empty;
        }

        private void ExecuteAddProductIntoCartAsync(Products product)
        {
            if (product is null)
                return;

            try
            {
                if (!ProductIsValid(product))
                    return;

                if (ConsumerCart.Select(x => x.Product).Contains(product))
                {
                    ConsumerCart.Single(x => x.Product == product).Quantity++;
                }
                else
                {
                    var newProduct = new CartRowViewModel(product);
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

        private void ExecuteDeleteProductFromCart()
        {
            if (SelectedCartRowItem is null)
            {
                return;
            }

            _ = ConsumerCart.Remove(SelectedCartRowItem);
        }

        private static bool ProductIsValid(Products product)
            => product.Cost.HasValue && product.WarehouseQuantity > 0;

        private static Func<Products, bool> SearchFilter(string searchStr)
            => x => x.ProductName.ToLower().Contains(searchStr.ToLower());

        public void Dispose()
        {
            _cleanup?.Dispose();
        }
    }
}