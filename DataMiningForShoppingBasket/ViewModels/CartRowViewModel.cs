using DataMiningForShoppingBasket.Common;

namespace DataMiningForShoppingBasket.ViewModels
{
    public class CartRowViewModel : NotifyPropertyChangedImplementation
    {
        private const int DefaultQuantity = 1;
        
        private decimal _quantity;

        public decimal Quantity
        {
            get => _quantity;
            set
            {
                if (value > Product.WarehouseQuantity)
                {
                    value = Product.WarehouseQuantity;
                }

                _quantity = Product.FractionalAllowed ? value : decimal.Floor(value);
                RaisePropertyChanged(nameof(Quantity));
                RaisePropertyChanged(nameof(TotalCost));
            }
        }

        public decimal TotalCost => Product.Cost.HasValue ? Quantity * Product.Cost.Value : default;
        
        public Products Product { get; set; }
        
        public CartRowViewModel(Products product)
        {
            Product = product;
            Quantity = DefaultQuantity;
        }
    }
}
