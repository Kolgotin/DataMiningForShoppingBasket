namespace DataMiningForShoppingBasket.Interfaces
{
    public interface IChangeWindowCallerDataContext
    {
        event ChangeWindowEventHandler ChangeWindowCalled;
    }

    public delegate void ChangeWindowEventHandler(object sender, IChangeWindowCaller e);
}
