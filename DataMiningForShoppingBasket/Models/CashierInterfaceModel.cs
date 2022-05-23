using System.ComponentModel;
using System.Windows;

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
                _quantity = ProductInstance?.FractionalAllowed == true ? value : decimal.Floor(value);
                RaisePropertyChanged(nameof(Quantity));
                RaisePropertyChanged(nameof(TotalCost));
            }
        }

        public decimal TotalCost => Quantity * ProductInstance.Cost.Value;
        
        public Products ProductInstance { get; set; }
        
        public CashierInterfaceModel(Products product)
        {
            ProductInstance = product;
            Quantity = DefaultQuantity;
        }
    }
}
