using DataMiningForShoppingBasket.ViewModels;

namespace DataMiningForShoppingBasket.Models
{
    public class CashierInterfaceModel : NotifyPropertyChangedImplementation
    {
        private const int DefaultQuantity = 1;
        
        private decimal _quantity;
        public decimal Quantity
        {
            get => _quantity;
            set
            {
                if (value > ProductInstance.WarehouseQuantity)
                {
                    value = ProductInstance.WarehouseQuantity;
                }

                _quantity = ProductInstance.FractionalAllowed ? value : decimal.Floor(value);
                RaisePropertyChanged(nameof(Quantity));
                RaisePropertyChanged(nameof(TotalCost));
            }
        }

        public decimal TotalCost => ProductInstance.ProductCost.HasValue ? Quantity * ProductInstance.ProductCost.Value : default;
        
        public Products ProductInstance { get; set; }
        
        public CashierInterfaceModel(Products product)
        {
            ProductInstance = product;
            Quantity = DefaultQuantity;
        }
    }
}
