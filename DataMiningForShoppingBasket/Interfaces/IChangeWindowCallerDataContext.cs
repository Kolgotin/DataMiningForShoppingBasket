namespace DataMiningForShoppingBasket.Interfaces
{
    public interface IChangeWindowCallerDataContext
    {
        event ChangeWindowEventHandler ChangeWindowCalled;
        string WindowLabel { get; }
    }

    public delegate void ChangeWindowEventHandler(object sender, IChangeWindowCaller e);
}
