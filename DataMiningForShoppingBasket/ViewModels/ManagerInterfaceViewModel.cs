using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using DataMiningForShoppingBasket.CommonClasses;
using DataMiningForShoppingBasket.Events;
using DataMiningForShoppingBasket.Interfaces;
using DataMiningForShoppingBasket.Models;

namespace DataMiningForShoppingBasket.ViewModels
{
    class ManagerInterfaceViewModel : INotifyPropertyChanged, IChangeWindowCallerDataContext
    {
        private readonly IGetData _getData;

        private MyAsyncCommand<Products> _addProductCommand;
        private MyAsyncCommand<Discounts> _addDiscountCommand;
        private MyAsyncCommand<Window> _closeCommand;

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        #endregion

        #region IChangeWindowCallerDataContext
        public event ChangeWindowEventHandler ChangeWindowCalled;

        public string WindowLabel => "Менеджер";
        #endregion

        #region Properties

        #region Commands
        public MyAsyncCommand<Products> AddProductCommand =>
            _addProductCommand ?? (_addProductCommand =
                new MyAsyncCommand<Products>(AddProductAsync, obj => _addProductCommand?.IsActive == false));

        public MyAsyncCommand<Discounts> AddDiscountCommand =>
            _addDiscountCommand ?? (_addDiscountCommand =
                new MyAsyncCommand<Discounts>(AddDiscountAsync, obj => _addDiscountCommand?.IsActive == false));

        public MyAsyncCommand<Window> CloseCommand =>
            _closeCommand ?? (_closeCommand =
                new MyAsyncCommand<Window>(CloseAsync, obj => _closeCommand?.IsActive == false));
        #endregion

        public List<Products> ProductsList => _getData.GetProductsAsync().Result;

        public List<Discounts> DiscountsList => _getData.GetDiscountsAsync().Result;

        public Products NewProduct { get; set; }

        public Discounts NewDiscount { get; set; }
        #endregion

        public ManagerInterfaceViewModel()
        {
            _getData = GetData.Instance;
            NewProduct = CreateNewProduct();
            NewDiscount = CreateNewDiscount();
        }

        private async Task AddProductAsync(Products obj)
        {
            try
            {
                MessageWriter.ShowMessage("Продукт добавлен");
            }
            catch (Exception e)
            {
                MessageWriter.ShowMessage(e.Message);
            }
        }

        private async Task AddDiscountAsync(Discounts obj)
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

        private async Task CloseAsync(Window window)
        {
            try
            {
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
