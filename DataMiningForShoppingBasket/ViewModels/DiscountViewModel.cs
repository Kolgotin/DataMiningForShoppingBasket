using System;
using DataMiningForShoppingBasket.Common;

namespace DataMiningForShoppingBasket.ViewModels
{
    //todo: убрать монструозные свойства - использовать автомаппер при сохранении
    public class DiscountViewModel : NotifyPropertyChangedImplementation
    {
        public Discounts Discount { get; }

        public int Id => Discount.Id;

        public string DiscountName
        {
            get => Discount.DiscountName;
            set
            {
                if (value == Discount.DiscountName) return;
                Discount.DiscountName = value;
                RaisePropertyChanged(nameof(DiscountName));
            }
        }

        public string DiscountDescription
        {
            get => Discount.DiscountDescription;
            set
            {
                if (value == Discount.DiscountDescription) return;
                Discount.DiscountDescription = value;
                RaisePropertyChanged(nameof(DiscountDescription));
            }
        }

        public DateTime StartDate
        {
            get => Discount.StartDate;
            set
            {
                if (value.Equals(Discount.StartDate)) return;
                Discount.StartDate = value;
                RaisePropertyChanged(nameof(StartDate));
            }
        }

        public DateTime FinishDate
        {
            get => Discount.FinishDate;
            set
            {
                if (value.Equals(Discount.FinishDate)) return;
                Discount.FinishDate = value;
                RaisePropertyChanged(nameof(FinishDate));
            }
        }

        public int ProductId
        {
            get => Discount.ProductId;
            set
            {
                if (value == Discount.ProductId) return;
                Discount.ProductId = value;
                RaisePropertyChanged(nameof(ProductId));
            }
        }

        public int Quantity
        {
            get => Discount.Quantity;
            set
            {
                if (value == Discount.Quantity) return;
                Discount.Quantity = value;
                RaisePropertyChanged(nameof(Quantity));
            }
        }

        public decimal DiscountCost
        {
            get => Discount.DiscountCost;
            set
            {
                if (value == Discount.DiscountCost) return;
                Discount.DiscountCost = value;
                RaisePropertyChanged(nameof(DiscountCost));
            }
        }

        public DiscountViewModel(Discounts discount)
        {
            Discount = discount;
        }
    }
}
