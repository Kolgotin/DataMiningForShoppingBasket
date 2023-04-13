using DataMiningForShoppingBasket.Common;

namespace DataMiningForShoppingBasket.ViewModels
{
    //todo: убрать монструозные свойства - использовать автомаппер при сохранении
    public class ProductViewModel : NotifyPropertyChangedImplementation
    {
        public Products Product { get; }

        public int Id => Product.Id;

        public string ProductName
        {
            get => Product.ProductName;
            set
            {
                if (value == Product.ProductName) return;
                Product.ProductName = value;
                RaisePropertyChanged(nameof(ProductName));
            }
        }

        public int? ProductTypeId
        {
            get => Product.ProductTypeId;
            set
            {
                if (value == Product.ProductTypeId) return;
                Product.ProductTypeId = value;
                RaisePropertyChanged(nameof(ProductTypeId));
            }
        }

        public decimal? Cost
        {
            get => Product.Cost;
            set
            {
                if (value == Product.Cost) return;
                Product.Cost = value;
                RaisePropertyChanged(nameof(Cost));
            }
        }

        public bool FractionalAllowed
        {
            get => Product.FractionalAllowed;
            set
            {
                if (value == Product.FractionalAllowed) return;
                Product.FractionalAllowed = value;
                RaisePropertyChanged(nameof(FractionalAllowed));
            }
        }

        public decimal WarehouseQuantity
        {
            get => Product.WarehouseQuantity;
            set
            {
                if (value == Product.WarehouseQuantity) return;
                Product.WarehouseQuantity = value;
                RaisePropertyChanged(nameof(WarehouseQuantity));
            }
        }

        public ProductViewModel(Products product)
        {
            Product = product;
        }
    }
}
