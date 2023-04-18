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
        private readonly INotifier<FocusProducts, int> _focusProductsINotifier;

        private readonly CompositeDisposable _cleanup;

        private readonly ReadOnlyObservableCollection<ProductViewModel> _productList;
        private readonly ReadOnlyObservableCollection<FocusProductViewModel> _focusProductList;

        #region ILabelHavingDataContext
        public string WindowLabel => "Менеджер";
        #endregion

        #region Properties

        #region Commands
        public MyAsyncCommand<ProductViewModel> AddOrEditProductCommand { get; }
        public MyAsyncCommand<FocusProductViewModel> AddOrEditFocusProductCommand { get; }
        #endregion

        public ReadOnlyObservableCollection<ProductViewModel> ProductList => _productList;
        public ReadOnlyObservableCollection<FocusProductViewModel> FocusProductList => _focusProductList;
        
        #endregion

        public ManagerInterfaceViewModel()
        {
            _dbManager = DbManager.GetInstance();
            _productsINotifier = DefaultNotifier<Products, int>.GetInstance();
            _focusProductsINotifier = DefaultNotifier<FocusProducts, int>.GetInstance();

            //todo: вызвать по-нормальному
            var init = new MyAsyncCommand(InitializeExecuteAsync);
            init.Execute();

            AddOrEditProductCommand = new MyAsyncCommand<ProductViewModel>(
                ExecuteAddProductAsync,
                _ => AddOrEditProductCommand?.IsActive == false);
            AddOrEditFocusProductCommand = new MyAsyncCommand<FocusProductViewModel>(
                ExecuteAddOrEditFocusProductAsync,
                obj => AddOrEditFocusProductCommand?.IsActive == false);

            _cleanup = new CompositeDisposable();
            _cleanup.Add(_productsINotifier.Changes
                .Transform(x=> new ProductViewModel(x))
                .Bind(out _productList)
                .Subscribe());
            _cleanup.Add(_focusProductsINotifier.Changes
                .Transform(x=> new FocusProductViewModel(x))
                .Bind(out _focusProductList)
                .Subscribe());
        }

        private async Task InitializeExecuteAsync()
        {
            var productList = await _dbManager.GetListAsync<Products>();
            var focusProductsList = await _dbManager.GetListAsync<FocusProducts>();

            productList.ForEach(_productsINotifier.NotifyAdd);
            focusProductsList.ForEach(_focusProductsINotifier.NotifyAdd);
        }
        
        private Task ExecuteAddProductAsync(ProductViewModel productVm)
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

        private Task ExecuteAddOrEditFocusProductAsync(FocusProductViewModel focusProductVm)
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
