namespace DataMiningForShoppingBasket.Interfaces
{
    public interface IHavingId<TId>
    {
        TId Id { get; set; }
    }
}