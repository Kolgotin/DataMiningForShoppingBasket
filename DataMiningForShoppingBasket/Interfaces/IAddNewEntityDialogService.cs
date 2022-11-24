namespace DataMiningForShoppingBasket.Interfaces
{
    public interface IAddNewEntityDialogService<T>
    {
        T Entity { get; set; }

        bool Save();
        void CloseWithoutSaving();
    }
}