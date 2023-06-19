using System;
using DataMiningForShoppingBasket.Common;

namespace DataMiningForShoppingBasket.ViewModels
{
    public class FocusProductViewModel : NotifyPropertyChangedImplementation
    {
        public FocusProducts FocusProduct { get; }

        public int Id => FocusProduct.Id;
        public string Description => FocusProduct.Description;
        public DateTime StartDate => FocusProduct.StartDate.ToLocalTime();
        public DateTime FinishDate => FocusProduct.FinishDate.ToLocalTime();
        public int ProductId => FocusProduct.ProductId;
        public decimal DiscountCost => FocusProduct.DiscountCost;
        public bool IsActual => FocusProduct.IsActual();

        public FocusProductViewModel(FocusProducts focusProduct)
        {
            FocusProduct = focusProduct;
        }
    }
}
