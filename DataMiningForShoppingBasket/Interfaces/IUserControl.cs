namespace DataMiningForShoppingBasket.Interfaces
{
    public interface IUserControl
    {
        object DataContext { get; set; }
        IUserWindowDataContext CustomDataContext { get; set; }
    }
}
