using DynamicData;
using System;

namespace DataMiningForShoppingBasket.Interfaces
{
    public interface INotifier<TEntity, TId>
    {
        IObservable<IChangeSet<TEntity, TId>> Changes { get; }
        void NotifyAddOrUpdate(TEntity entity);
        void NotifyDelete(TId entityId);
    }
}