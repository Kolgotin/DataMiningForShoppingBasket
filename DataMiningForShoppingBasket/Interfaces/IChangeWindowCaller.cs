namespace DataMiningForShoppingBasket.Interfaces
{
    public interface IChangeWindowCaller
    {
        object DataContext { get; set; }
        IChangeWindowCallerDataContext CustomDataContext { get; set; }
    }
}
