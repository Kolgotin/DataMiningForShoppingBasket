﻿using DataMiningForShoppingBasket.Commands;
using DataMiningForShoppingBasket.Common;
using DataMiningForShoppingBasket.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DataMiningForShoppingBasket.ViewModels
{
    public class DiscountDialogViewModel : NotifyPropertyChangedImplementation
    {
        private readonly Discounts _discount;
        private readonly IDbManager _dbManager;

        public DiscountDialogViewModel(Discounts discount = null)
        {
            _dbManager = DbManager.GetInstance();

            ProductList = _dbManager.GetListAsync<Products>().Result
                .OrderBy(x=>x.ProductName).ToList();
            SaveCommand = new MyAsyncCommand<Window>(SaveExecuteAsync);

            if (discount == null)
            {
                _discount = new Discounts();
                StartDate = DateTime.Today;
                FinishDate = DateTime.Today;
                return;
            }

            _discount = discount;
            DiscountName = _discount.DiscountName;
            DiscountDescription = _discount.DiscountDescription;
            StartDate = _discount.StartDate;
            FinishDate= _discount.FinishDate;
            ProductId= _discount.ProductId;
            Quantity = _discount.Quantity;
            DiscountCost = _discount.DiscountCost;
        }

        #region Properties

        public ICommand SaveCommand { get; }

        public string DiscountName { get; set; }
        public string DiscountDescription { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal DiscountCost { get; set; }

        public IReadOnlyCollection<Products> ProductList { get; }

        #endregion

        private async Task SaveExecuteAsync(Window window)
        {
            _discount.DiscountName = DiscountName;
            _discount.DiscountDescription = DiscountDescription ?? string.Empty;
            _discount.StartDate = StartDate;
            _discount.FinishDate = FinishDate;
            _discount.ProductId = ProductId;
            _discount.Quantity = Quantity;
            _discount.DiscountCost = DiscountCost;
            await _dbManager.SaveAndNotifyHavingIdEntityAsync<Discounts, int>(_discount);

            window.DialogResult = true;
            window.Close();
        }
    }
}
