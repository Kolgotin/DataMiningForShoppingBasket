namespace DataMiningForShoppingBasket.Events
{
    public interface IChangeWindowCaller
    {
        object DataContext { get; set; }
        IChangeWindowCallerDataContext CustomDataContext { get; set; }
    }

    public interface IChangeWindowCallerDataContext
    {
        event ChangeWindowEventHandler ChangeWindowCalled;
    }

    public delegate void ChangeWindowEventHandler(object sender, IChangeWindowCaller e);
}
