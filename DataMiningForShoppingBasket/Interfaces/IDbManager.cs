using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataMiningForShoppingBasket.Interfaces
{
    public interface IDbManager
    {
        Task<Users> GetUserAsync(string login);
        Task<List<SaleRows>> GetSalesByProductIds(IReadOnlyCollection<int> productIds);
        Task<List<T>> GetListAsync<T>() where T : class;
        Task<T> SaveEntityAsync<T>(T entity) where T : class;
        Task<TEntity> SaveAndNotifyHavingIdEntityAsync<TEntity, TId>(TEntity entity)
            where TEntity : class, IHavingId<TId>, new();
        Task SaveSale(SaleReceipts receipt);
    }
}
