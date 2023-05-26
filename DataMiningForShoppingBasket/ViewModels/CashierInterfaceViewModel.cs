using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using System.Windows.Input;
using DataMiningForShoppingBasket.Commands;
using DataMiningForShoppingBasket.Common;
using DataMiningForShoppingBasket.Handlers;
using DataMiningForShoppingBasket.Interfaces;
using DataMiningForShoppingBasket.Views;

namespace DataMiningForShoppingBasket.ViewModels
{
    public class CashierInterfaceViewModel : NotifyPropertyChangedImplementation,
        ILabelHavingDataContext, IDisposable
    {
        private readonly IDbManager _dbManager;
        private readonly IPrepareOfferHandler _prepareOfferHandler;
        private readonly CompositeDisposable _cleanup = new();

        private List<AdditionalOfferViewModel> _offerProductList;
        private ProductListViewModel _productList;

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
        
        //todo: сделать через DynamicData, подаписать на изменение в ConsumerCart
        public decimal TotalCost => ConsumerCart.Sum(x => x.TotalCost);

        public ProductListViewModel ProductList
        {
            get => _productList;
            private set => SetProperty(ref _productList, value);
        }

        #region Commands

        public IAsyncCommand ShowFocusProductListCommand { get; }
        public ICommand AddProductIntoCartCommand { get; }
        public ICommand CleanCartCommand { get; }
        public IAsyncCommand PrepareOfferCommand { get; }
        public ICommand FinalizeSaleCommand { get; }
        public ICommand DeleteProductFromCartCommand { get; }

        #endregion

        #endregion Properties

        public CashierInterfaceViewModel()
        {
            _dbManager = DbManager.GetInstance();
            _prepareOfferHandler = AprioriAlgorithm3Deep.GetInstance();

            InitializeAsync();

            ConsumerCart = new ObservableCollection<CartRowViewModel>();

            ShowFocusProductListCommand = new MyAsyncCommand(ExecuteShowFocusProductListAsync,
                _ => ShowFocusProductListCommand?.IsActive == false);
            CleanCartCommand = new MyCommand(ExecuteCleanCart);
            PrepareOfferCommand = new MyAsyncCommand(ExecutePrepareOfferAsync,
                _ => PrepareOfferCommand?.IsActive == false);
            FinalizeSaleCommand = new MyAsyncCommand(ExecuteFinalizeSaleAsync);
            AddProductIntoCartCommand = new MyCommand<ProductViewModel>(ExecuteAddProductIntoCartAsync);
            DeleteProductFromCartCommand = new MyCommand(ExecuteDeleteProductFromCart);
        }

        private async void InitializeAsync()
        {
            ProductList = await AsyncInitializedCreator<ProductListViewModel>.ConstructorAsync();
            ProductList.DoubleClickElementCommand = AddProductIntoCartCommand;
            _cleanup.Add(ProductList);
        }

        private async Task ExecuteShowFocusProductListAsync()
        {
            try
            {
                var focusProductListViewModel = await AsyncInitializedCreator<FocusProductListViewModel>.ConstructorAsync();
                _cleanup.Add(focusProductListViewModel);

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
        
        private void ExecuteAddProductIntoCartAsync(ProductViewModel productVm)
        {
            var product = productVm?.Product;

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
            RaisePropertyChanged(nameof(TotalCost));
        }

        private static bool ProductIsValid(Products product)
            => product.Cost.HasValue && product.WarehouseQuantity > 0;

        public void Dispose()
        {
            _cleanup?.Dispose();
        }
    }
}