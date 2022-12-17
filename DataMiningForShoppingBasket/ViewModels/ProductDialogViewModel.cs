﻿using DataMiningForShoppingBasket.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DataMiningForShoppingBasket.Commands;
using DataMiningForShoppingBasket.CommonClasses;
using System.Linq;

namespace DataMiningForShoppingBasket.ViewModels
{
    public class ProductDialogViewModel : NotifyPropertyChangedImplementation
    {
        private readonly Products _product;
        private readonly IGetData _getData;
        private readonly INotifier<Products, int> _productsINotifier;

        public ProductDialogViewModel(Products product = null)
        {
            _getData = GetData.GetInstance();
            _productsINotifier = DefaultNotifier<Products, int>.GetInstance(x => x.Id);

            ProductTypes = _getData.GetListAsync<ProductTypes>().Result
                .OrderBy(x => x.ProductTypeName).ToList();
            SaveCommand = new MyAsyncCommand<Window>(SaveExecuteAsync);

            if (product == null)
            {
                _product = new Products();
                return;
            }

            _product = product;
            ProductName = product.ProductName;
            ProductTypeId = product.ProductTypeId;
            ProductCost = product.ProductCost;
            FractionalAllowed = product.FractionalAllowed;
            WarehouseQuantity = product.WarehouseQuantity;
        }

        #region Properties
        
        public ICommand SaveCommand { get; }

        public string ProductName { get; set; }
        public int? ProductTypeId { get; set; }
        public decimal? ProductCost { get; set; }
        public bool FractionalAllowed { get; set; }
        public decimal WarehouseQuantity { get; set; }

        public IReadOnlyCollection<ProductTypes> ProductTypes { get; }

        #endregion

        #region Methods

        private async Task SaveExecuteAsync(Window window)
        {
            _product.ProductName = ProductName;
            _product.ProductTypeId = ProductTypeId;
            _product.ProductCost = ProductCost;
            _product.FractionalAllowed = FractionalAllowed;
            _product.WarehouseQuantity = WarehouseQuantity;
            await _getData.SaveEntityAsync(_product);
            _productsINotifier.NotifyAddOrUpdate(_product);

            window.DialogResult = true;
            window.Close();
        }

        #endregion
    }
}