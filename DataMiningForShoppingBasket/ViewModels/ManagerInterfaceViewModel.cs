using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using DataMiningForShoppingBasket.Commands;
using DataMiningForShoppingBasket.Common;
using DataMiningForShoppingBasket.Interfaces;
using DataMiningForShoppingBasket.Views;
using DynamicData;

namespace DataMiningForShoppingBasket.ViewModels
{
    public sealed class ManagerInterfaceViewModel : NotifyPropertyChangedImplementation,
        ILabelHavingDataContext, IDisposable
    {
        private readonly IDbManager _dbManager;
        private readonly INotifier<Products, int> _productsINotifier;

        private readonly CompositeDisposable _cleanup;

        private readonly ReadOnlyObservableCollection<ProductViewModel> _productList;
        private FocusProductListViewModel _focusProductList;

        #region ILabelHavingDataContext
        public string WindowLabel => "Менеджер";
        #endregion

        #region Properties

        #region Commands
        public IAsyncCommand AddOrEditProductCommand { get; }
        public IAsyncCommand AddOrEditFocusProductCommand { get; }
        #endregion

        public ReadOnlyObservableCollection<ProductViewModel> ProductList => _productList;

        public FocusProductListViewModel FocusProductList
        {
            get => _focusProductList;
            private set => SetProperty(ref _focusProductList, value);
        }

        #endregion

        public ManagerInterfaceViewModel()
        {
            _dbManager = DbManager.GetInstance();
            _productsINotifier = DefaultNotifier<Products, int>.GetInstance();

            AddOrEditProductCommand = new MyAsyncCommand<ProductViewModel>(
                ExecuteAddProductAsync,
                _ => AddOrEditProductCommand?.IsActive == false);
            AddOrEditFocusProductCommand = new MyAsyncCommand<FocusProductViewModel>(
                ExecuteAddOrEditFocusProductAsync,
                _ => AddOrEditFocusProductCommand?.IsActive == false);

            //todo: вызвать по-нормальному
            var init = new MyAsyncCommand(InitializeExecuteAsync);
            init.Execute();

            _cleanup = new CompositeDisposable
            {
                _productsINotifier.Changes
                    .Transform(x => new ProductViewModel(x))
                    .Bind(out _productList)
                    .Subscribe()
            };
        }
        
        private async Task InitializeExecuteAsync()
        {
            var productList = await _dbManager.GetListAsync<Products>();
            productList.ForEach(_productsINotifier.NotifyAdd);
            FocusProductList = await AsyncInitializedCreator<FocusProductListViewModel>.ConstructorAsync();
            FocusProductList.DoubleClickElementCommand = AddOrEditFocusProductCommand;
            _cleanup.Add(FocusProductList);
        }
        
        private static Task ExecuteAddProductAsync(ProductViewModel productVm)
        {
            var product = productVm?.Product;
            try
            {
                var view = new ProductDialogView(product);
                var res = view.ShowDialog();

                if (res is true)
                {
                    MessageWriter.ShowMessage("Продукт сохранен");
                }
            }
            catch (Exception e)
            {
                MessageWriter.ShowMessage(e.Message);
            }

            return Task.CompletedTask;
        }

        private static Task ExecuteAddOrEditFocusProductAsync(FocusProductViewModel focusProductVm)
        {
            var focusProduct = focusProductVm?.FocusProduct;
            try
            {
                var view = new FocusProductDialogView(focusProduct);
                var res = view.ShowDialog();

                if (res is true)
                {
                    MessageWriter.ShowMessage("Фокусный продукт сохранен");
                }
            }
            catch (Exception e)
            {
                MessageWriter.ShowMessage(e.Message);
            }

            return Task.CompletedTask; 
        }

        public void Dispose()
        {
            _cleanup?.Dispose();
        }
    }
}
