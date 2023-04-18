using System;
using DataMiningForShoppingBasket.Common;

namespace DataMiningForShoppingBasket.ViewModels
{
    //todo: убрать монструозные свойства - использовать автомаппер при сохранении
    public class FocusProductViewModel : NotifyPropertyChangedImplementation
    {
        public FocusProducts FocusProduct { get; }

        public int Id => FocusProduct.Id;
        
        public string Description
        {
            get => FocusProduct.Description;
            set
            {
                if (value == FocusProduct.Description) return;
                FocusProduct.Description = value;
                RaisePropertyChanged(nameof(Description));
            }
        }

        public DateTime StartDate
        {
            get => FocusProduct.StartDate;
            set
            {
                if (value.Equals(FocusProduct.StartDate)) return;
                FocusProduct.StartDate = value;
                RaisePropertyChanged(nameof(StartDate));
            }
        }

        public DateTime FinishDate
        {
            get => FocusProduct.FinishDate;
            set
            {
                if (value.Equals(FocusProduct.FinishDate)) return;
                FocusProduct.FinishDate = value;
                RaisePropertyChanged(nameof(FinishDate));
            }
        }

        public int ProductId
        {
            get => FocusProduct.ProductId;
            set
            {
                if (value == FocusProduct.ProductId) return;
                FocusProduct.ProductId = value;
                RaisePropertyChanged(nameof(ProductId));
            }
        }
        
        public decimal DiscountCost
        {
            get => FocusProduct.DiscountCost;
            set
            {
                if (value == FocusProduct.DiscountCost) return;
                FocusProduct.DiscountCost = value;
                RaisePropertyChanged(nameof(DiscountCost));
            }
        }

        public FocusProductViewModel(FocusProducts focusProduct)
        {
            FocusProduct = focusProduct;
        }
    }
}
