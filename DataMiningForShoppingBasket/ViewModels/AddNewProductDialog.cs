using System;
using DataMiningForShoppingBasket.Interfaces;

namespace DataMiningForShoppingBasket.ViewModels
{
    public class AddNewProductDialog : IAddNewEntityDialogService<Products>
    {
        public Products Entity { get; set; }
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
