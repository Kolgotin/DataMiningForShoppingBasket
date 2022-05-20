using System.ComponentModel;

namespace DataMiningForShoppingBasket.Models
{
    public class CashierInterfaceModel : INotifyPropertyChanged
    {
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
                _quantity = value;
                RaisePropertyChanged(nameof(TotalCost));
            }
        }

        public decimal TotalCost => Quantity * ProductInstance.Cost.Value;
        
        public Products ProductInstance { get; set; }

        public CashierInterfaceModel()
        {
            ProductInstance = new Products();
        }

        public CashierInterfaceModel(int i, string s) : this()
        {
            ProductInstance.Id = i;
            ProductInstance.ProductName = s;
        }

        public CashierInterfaceModel(Products product)
        {
            ProductInstance = product;
        }
    }
}
