namespace DataMiningForShoppingBasket.Interfaces
{
    public interface IUserControl
    {
        object DataContext { get; set; }
        ILabelHavingDataContext CustomDataContext { get; set; }
    }
}
