using DataMiningForShoppingBasket.CommonClasses;
using DataMiningForShoppingBasket.Commands;
using DataMiningForShoppingBasket.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

namespace DataMiningForShoppingBasket.ViewModels
{
    class ManagerInterfaceViewModel : INotifyPropertyChanged, IUserWindowDataContext
    {
        private readonly IGetData _getData;
        
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        #endregion

        #region IUserWindowDataContext
        public string WindowLabel => "Менеджер";
        #endregion

        #region Properties

        #region Commands
        public MyAsyncCommand<Products> AddProductCommand { get; }
        public MyAsyncCommand<Discounts> AddDiscountCommand { get; }
        #endregion

        public List<Products> ProductList => _getData.GetProductsAsync().Result;
        public List<Discounts> DiscountsList => _getData.GetDiscountsAsync().Result;

        public Products NewProduct { get; set; }
        public Discounts NewDiscount { get; set; }
        #endregion

        public ManagerInterfaceViewModel()
        {
            _getData = GetData.Instance;
            NewProduct = CreateNewProduct();
            NewDiscount = CreateNewDiscount();

            AddProductCommand = new MyAsyncCommand<Products>(
                ExecuteAddProductAsync,
                obj => AddProductCommand?.IsActive == false);
            AddDiscountCommand = new MyAsyncCommand<Discounts>(
                ExecuteAddDiscountAsync,
                obj => AddDiscountCommand?.IsActive == false);
        }

        private Task ExecuteAddProductAsync(Products obj)
        {
            try
            {

                MessageWriter.ShowMessage("Продукт добавлен");
            }
            catch (Exception e)
            {
                MessageWriter.ShowMessage(e.Message);
            }

            return Task.CompletedTask;
        }

        private async Task ExecuteAddDiscountAsync(Discounts obj)
        {
            try
            {
                await _getData.SaveDiscountsAsync(obj);
                NewDiscount = CreateNewDiscount();
                RaisePropertyChanged(nameof(DiscountsList));
                RaisePropertyChanged(nameof(NewDiscount));
            }
            catch (Exception e)
            {
                MessageWriter.ShowMessage(e.Message);
            }
        }

        private Products CreateNewProduct()
        {
            return new Products
            {
                ProductName = string.Empty,
            };
        }

        private Discounts CreateNewDiscount()
        {
            return new Discounts
            {
                DiscountName = string.Empty,
                DiscountDescription = string.Empty,
                StartDate = DateTime.Today,
                FinishDate = DateTime.Today
            };
        }
    }
}
