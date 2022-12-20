using DataMiningForShoppingBasket.Commands;
using DataMiningForShoppingBasket.Common;
using DataMiningForShoppingBasket.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DataMiningForShoppingBasket.ViewModels
{
    public class ProductDialogViewModel : NotifyPropertyChangedImplementation
    {
        private readonly Products _product;
        private readonly IGetData _getData;

        public ProductDialogViewModel(Products product = null)
        {
            _getData = GetData.GetInstance();

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

        private async Task SaveExecuteAsync(Window window)
        {
            _product.ProductName = ProductName;
            _product.ProductTypeId = ProductTypeId;
            _product.ProductCost = ProductCost;
            _product.FractionalAllowed = FractionalAllowed;
            _product.WarehouseQuantity = WarehouseQuantity;
            await _getData.SaveAndNotifyHavingIdEntityAsync<Products, int>(_product);

            window.DialogResult = true;
            window.Close();
        }
    }
}