using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DataMiningForShoppingBasket.Commands;
using DataMiningForShoppingBasket.Common;
using DataMiningForShoppingBasket.Interfaces;
using DataMiningForShoppingBasket.Views;
using DynamicData;
using DynamicData.Binding;

namespace DataMiningForShoppingBasket.ViewModels
{
    public sealed class ManagerInterfaceViewModel : NotifyPropertyChangedImplementation,
        ILabelHavingDataContext, IDisposable
    {
        private readonly CompositeDisposable _cleanup = new();

        private FocusProductListViewModel _focusProductList;
        private ProductListViewModel _productList;

        #region ILabelHavingDataContext
        public string WindowLabel => "Менеджер";
        #endregion

        #region Properties

        #region Commands
        public IAsyncCommand AddOrEditProductCommand { get; }
        public IAsyncCommand AddOrEditFocusProductCommand { get; }
        #endregion

        public FocusProductListViewModel FocusProductList
        {
            get => _focusProductList;
            private set => SetProperty(ref _focusProductList, value);
        }

        public ProductListViewModel ProductList
        {
            get => _productList;
            private set => SetProperty(ref _productList, value);
        }

        #endregion

        public ManagerInterfaceViewModel()
        {
            InitializeAsync();
            AddOrEditFocusProductCommand = new MyAsyncCommand<FocusProductViewModel>(
                ExecuteAddOrEditFocusProductAsync,
                _ => AddOrEditFocusProductCommand?.IsActive == false);
            AddOrEditProductCommand = new MyAsyncCommand<ProductViewModel>(
                ExecuteAddOrEditProductAsync,
                _ => AddOrEditProductCommand?.IsActive == false);
        }

        private async void InitializeAsync()
        {
            FocusProductList = await AsyncInitializedCreator<FocusProductListViewModel>.ConstructorAsync();
            FocusProductList.DoubleClickElementCommand = AddOrEditFocusProductCommand;
            _cleanup.Add(FocusProductList);

            ProductList = await AsyncInitializedCreator<ProductListViewModel>.ConstructorAsync();
            ProductList.DoubleClickElementCommand = AddOrEditProductCommand;
            _cleanup.Add(ProductList);
        }

        private static Task ExecuteAddOrEditProductAsync(ProductViewModel productVm)
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
