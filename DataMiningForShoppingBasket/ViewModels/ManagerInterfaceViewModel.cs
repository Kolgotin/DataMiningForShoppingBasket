using DataMiningForShoppingBasket.Commands;
using DataMiningForShoppingBasket.CommonClasses;
using DataMiningForShoppingBasket.Interfaces;
using DataMiningForShoppingBasket.Views;
using DynamicData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DataMiningForShoppingBasket.ViewModels
{
    public class ManagerInterfaceViewModel : NotifyPropertyChangedImplementation, ILabelHavingDataContext
    {
        private readonly IGetData _getData;
        private List<Products> _productList;
        private List<Discounts> _discountsList;

        #region ILabelHavingDataContext
        public string WindowLabel => "Менеджер";
        #endregion

        #region Properties

        #region Commands
        public MyAsyncCommand<Products> AddOrEditProductCommand { get; }
        public MyAsyncCommand<Discounts> AddOrEditDiscountCommand { get; }
        #endregion

        private INotifier<Products, int> _productsINotifier;
        private INotifier<Discounts, int> _discountsINotifier;

        private readonly ReadOnlyObservableCollection<Products> _products;
        private readonly ReadOnlyObservableCollection<Discounts> _discounts;

        public ReadOnlyObservableCollection<Products> Products => _products;
        public ReadOnlyObservableCollection<Discounts> Discounts => _discounts;
        
        #endregion

        public ManagerInterfaceViewModel()
        {
            _getData = GetData.GetInstance();
            _productsINotifier = DefaultNotifier<Products, int>.GetInstance(x => x.Id);
            _discountsINotifier = DefaultNotifier<Discounts, int>.GetInstance(x => x.Id);

            //todo: вызвать по-нормальному
            InitializeExecuteAsync();

            AddOrEditProductCommand = new MyAsyncCommand<Products>(
                ExecuteAddProductAsync,
                _ => AddOrEditProductCommand?.IsActive == false);
            AddOrEditDiscountCommand = new MyAsyncCommand<Discounts>(
                ExecuteAddDiscountAsync,
                obj => AddOrEditDiscountCommand?.IsActive == false);

            _productsINotifier.Changes
                .Bind(out _products)
                .Subscribe();
            _discountsINotifier.Changes
                .Bind(out _discounts)
                .Subscribe();
        }

        private async Task InitializeExecuteAsync()
        {
            var productList = await _getData.GetListAsync<Products>();
            var discountsList = await _getData.GetListAsync<Discounts>();

            productList.ForEach(_productsINotifier.NotifyAddOrUpdate);
            discountsList.ForEach(_discountsINotifier.NotifyAddOrUpdate);
        }
        
        private async Task ExecuteAddProductAsync(Products product)
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
        }

        private async Task ExecuteAddDiscountAsync(Discounts discount)
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
        }
    }
}
