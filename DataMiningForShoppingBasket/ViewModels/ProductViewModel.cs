using DataMiningForShoppingBasket.Common;

namespace DataMiningForShoppingBasket.ViewModels
{
    public class ProductViewModel : NotifyPropertyChangedImplementation
    {
        public Products Product { get; }

        public int Id => Product.Id;
        public string ProductName => Product.ProductName;
        public decimal? Cost => Product.Cost;
        public decimal WarehouseQuantity => Product.WarehouseQuantity;

        public ProductViewModel(Products product)
        {
            Product = product;
        }
    }
}
