using System.ComponentModel;

namespace DataMiningForShoppingBasket.Models
{
    public class CashierInterfaceModel : INotifyPropertyChanged
    {
        private const int DefaultQuantity = 1;

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        #endregion

        private decimal _quantity;
        public decimal Quantity
        {
            get => _quantity;
            set
            {
                var oldVal = _quantity;

                if (value > ProductInstance.WarehouseQuantity)
                {
                    value = ProductInstance.WarehouseQuantity;
                }

                _quantity = ProductInstance.FractionalAllowed ? value : decimal.Floor(value);
                RaisePropertyChanged(nameof(Quantity));
                RaisePropertyChanged(nameof(TotalCost));
            }
        }

        public decimal TotalCost => ProductInstance.Cost.HasValue ? Quantity * ProductInstance.Cost.Value : default;
        
        public Products ProductInstance { get; set; }
        
        public CashierInterfaceModel(Products product)
        {
            ProductInstance = product;
            Quantity = DefaultQuantity;
        }
    }
}
