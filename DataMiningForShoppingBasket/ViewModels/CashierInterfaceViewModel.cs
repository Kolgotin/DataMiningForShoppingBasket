using DataMiningForShoppingBasket.Commands;
using DataMiningForShoppingBasket.Common;
using DataMiningForShoppingBasket.Handlers;
using DataMiningForShoppingBasket.Interfaces;
using DataMiningForShoppingBasket.Views;
using DynamicData;
using DynamicData.Binding;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DataMiningForShoppingBasket.ViewModels
{
    public class CashierInterfaceViewModel : NotifyPropertyChangedImplementation,
        ILabelHavingDataContext, IDisposable
    {
        private readonly IDbManager _dbManager;
        private readonly IPrepareOfferHandler _prepareOfferHandler;
        private readonly INotifier<Products, int> _productsINotifier;
        private readonly CompositeDisposable _cleanup = new();

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

        //todo: сделать через DynamicData, подаписать на изменение в ConsumerCart
        public decimal TotalCost => ConsumerCart.Sum(x => x.TotalCost);

        #region Commands

        public IAsyncCommand ShowFocusProductListCommand { get; }
        public ICommand AddProductIntoCartCommand { get; }
        public ICommand CleanCartCommand { get; }
        public IAsyncCommand PrepareOfferCommand { get; }
        public ICommand FinalizeSaleCommand { get; }
        public ICommand ClearSearchCommand { get; }
        public ICommand DeleteProductFromCartCommand { get; }

        #endregion

        #endregion Properties

        public CashierInterfaceViewModel()
        {
            _dbManager = DbManager.GetInstance();
            _productsINotifier = DefaultNotifier<Products, int>.GetInstance();
            _prepareOfferHandler = AprioriAlgorithm3Deep.GetInstance();

            //todo: вызвать по-нормальному
            var init = new MyAsyncCommand(InitializeExecuteAsync);
            init.Execute();

            ConsumerCart = new ObservableCollection<CartRowViewModel>();
            SearchString = string.Empty;

            ShowFocusProductListCommand = new MyAsyncCommand(ExecuteShowFocusProductListAsync,
                _=> ShowFocusProductListCommand?.IsActive == false);
            CleanCartCommand = new MyCommand(ExecuteCleanCart);
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

            _cleanup.Add(productsChangeDisposable);
        }

        private async Task InitializeExecuteAsync()
        {
            var productList = await _dbManager.GetListAsync<Products>();
            productList.ForEach(_productsINotifier.NotifyAdd);
        }

        private static async Task ExecuteShowFocusProductListAsync()
        {
            try
            {
                var focusProductListViewModel = await AsyncInitializedCreator<FocusProductListViewModel>.ConstructorAsync();
                var view = new FocusProductListDialogView
                {
                    DataContext = focusProductListViewModel
                };
                view.ShowDialog();
            }
            catch (Exception e)
            {
                MessageWriter.ShowMessage(e.Message);
            }
        }

        private void ExecuteCleanCart()
        {
            ConsumerCart?.Clear();
            OfferProductList = null;
            RaisePropertyChanged(nameof(TotalCost));
        }

        private async Task ExecutePrepareOfferAsync()
        {
            try
            {
                var productsInCart = ConsumerCart.Select(x => x.Product).ToList();
                var productsList = await _prepareOfferHandler.PrepareOfferAsync(productsInCart);
                OfferProductList = productsList
                    .Select(x => new AdditionalOfferViewModel(x.Item1, x.Item2))
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
                    SaleDateTime = DateTime.UtcNow,
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
                ExecuteCleanCart();
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