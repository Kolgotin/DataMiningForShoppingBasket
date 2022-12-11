using System;
using DataMiningForShoppingBasket.Interfaces;

namespace DataMiningForShoppingBasket.ViewModels
{
    public class AddNewDiscountDialog : IAddNewEntityDialogService<Discounts>
    {
        public Discounts Entity { get; set; }

        public bool Save()
        {
            throw new NotImplementedException();
        }

        public void CloseWithoutSaving()
        {
            throw new NotImplementedException();
        }
    }
}
