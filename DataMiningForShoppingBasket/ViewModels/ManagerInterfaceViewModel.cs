using DataMiningForShoppingBasket.Commands;
using DataMiningForShoppingBasket.CommonClasses;
using DataMiningForShoppingBasket.Interfaces;
using DataMiningForShoppingBasket.Views;
using DynamicData;
using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace DataMiningForShoppingBasket.ViewModels
{
    public sealed class ManagerInterfaceViewModel : NotifyPropertyChangedImplementation,
        ILabelHavingDataContext, IDisposable
    {
        private readonly IGetData _getData;
        private readonly INotifier<Products, int> _productsINotifier;
        private readonly INotifier<Discounts, int> _discountsINotifier;

        private readonly CompositeDisposable _cleanup;

        private readonly ReadOnlyObservableCollection<Products> _productList;
        private readonly ReadOnlyObservableCollection<Discounts> _discountList;

        #region ILabelHavingDataContext
        public string WindowLabel => "Менеджер";
        #endregion

        #region Properties

        #region Commands
        public MyAsyncCommand<Products> AddOrEditProductCommand { get; }
        public MyAsyncCommand<Discounts> AddOrEditDiscountCommand { get; }
        #endregion

        public ReadOnlyObservableCollection<Products> ProductList => _productList;
        public ReadOnlyObservableCollection<Discounts> DiscountList => _discountList;
        
        #endregion

        public ManagerInterfaceViewModel()
        {
            _getData = GetData.GetInstance();
            _productsINotifier = DefaultNotifier<Products, int>.GetInstance();
            _discountsINotifier = DefaultNotifier<Discounts, int>.GetInstance();

            //todo: вызвать по-нормальному
            var init = new MyAsyncCommand(InitializeExecuteAsync);
            init.Execute();

            AddOrEditProductCommand = new MyAsyncCommand<Products>(
                ExecuteAddProductAsync,
                _ => AddOrEditProductCommand?.IsActive == false);
            AddOrEditDiscountCommand = new MyAsyncCommand<Discounts>(
                ExecuteAddDiscountAsync,
                obj => AddOrEditDiscountCommand?.IsActive == false);

            _cleanup = new CompositeDisposable();
            _cleanup.Add(_productsINotifier.Changes
                .Bind(out _productList)
                .Subscribe());
            _cleanup.Add(_discountsINotifier.Changes
                .Bind(out _discountList)
                .Subscribe());
        }

        private async Task InitializeExecuteAsync()
        {
            var productList = await _getData.GetListAsync<Products>();
            var discountsList = await _getData.GetListAsync<Discounts>();

            productList.ForEach(_productsINotifier.NotifyAddOrUpdate);
            discountsList.ForEach(_discountsINotifier.NotifyAddOrUpdate);
        }
        
        private Task ExecuteAddProductAsync(Products product)
        {
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

        private Task ExecuteAddDiscountAsync(Discounts discount)
        {
            try
            {
                var view = new DiscountDialogView(discount);
                var res = view.ShowDialog();

                if (res is true)
                {
                    MessageWriter.ShowMessage("Акция сохранена");
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
