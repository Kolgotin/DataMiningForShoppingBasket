using DataMiningForShoppingBasket.Commands;
using DataMiningForShoppingBasket.CommonClasses;
using DataMiningForShoppingBasket.Interfaces;
using DataMiningForShoppingBasket.Views;
using System;
using System.Collections.Generic;
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

        public List<Products> ProductList
        {
            get => _productList;
            private set => SetProperty(ref _productList, value);
        }

        public List<Discounts> DiscountsList
        {
            get => _discountsList;
            private set => SetProperty(ref _discountsList, value);
        }

        #endregion

        public ManagerInterfaceViewModel()
        {
            _getData = GetData.GetInstance();

            //todo: вызвать по-нормальному
            InitializeExecuteAsync();

            AddOrEditProductCommand = new MyAsyncCommand<Products>(
                ExecuteAddProductAsync,
                _ => AddOrEditProductCommand?.IsActive == false);
            AddOrEditDiscountCommand = new MyAsyncCommand<Discounts>(
                ExecuteAddDiscountAsync,
                obj => AddOrEditDiscountCommand?.IsActive == false);
        }

        private async Task InitializeExecuteAsync()
        {
            ProductList = await _getData.GetListAsync<Products>();
            DiscountsList = await _getData.GetListAsync<Discounts>();
        }

        private async Task ExecuteAddProductAsync(Products product)
        {
            try
            {
                var view = new ProductDialogView(product);
                var res = view.ShowDialog();

                if (res is true)
                {
                    ProductList = await _getData.GetListAsync<Products>();
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
                    DiscountsList = await _getData.GetListAsync<Discounts>();
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
