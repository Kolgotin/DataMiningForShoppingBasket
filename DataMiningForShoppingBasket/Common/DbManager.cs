using DataMiningForShoppingBasket.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Threading.Tasks;

namespace DataMiningForShoppingBasket.Common
{
    public class DbManager : IDbManager
    {
        private readonly DataMiningEntities _dbContext;

        private static readonly Lazy<DbManager> Lazy =
            new Lazy<DbManager>(() => new DbManager());

        private DbManager()
        {
            _dbContext = new DataMiningEntities();
        }

        public static DbManager GetInstance() => Lazy.Value;
        
        public async Task<Users> GetUserAsync(string loginStr)
        {
            //todo: шифровать UserPassword
            var login = loginStr.Trim();
            var user = await _dbContext.Users.AsQueryable()
                .FirstOrDefaultAsync(x => x.UserName == login)
                .ConfigureAwait(false);
            return user;
        }
        
        public async Task<List<T>> GetListAsync<T>() where T : class
            => await _dbContext.Set<T>().ToListAsync().ConfigureAwait(false);

        public async Task<T> SaveEntityAsync<T>(T entity)
            where T : class
        {
            _dbContext.Set<T>().AddOrUpdate(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }

        public async Task<TEntity> SaveAndNotifyHavingIdEntityAsync<TEntity, TId>(TEntity entity)
            where TEntity : class, IHavingId<TId>, new()
        {
            var id = entity.Id;
            _dbContext.Set<TEntity>().AddOrUpdate(entity);
            await _dbContext.SaveChangesAsync();
            var notifier = DefaultNotifier<TEntity, TId>.GetInstance();
            if (entity.Id.Equals(id))
            {
                notifier.NotifyUpdate(entity);
            }
            else
            {
                notifier.NotifyAdd(entity);
            }
            return entity;
        }

        public async Task SaveSale(SaleReceipts receipt)
        {
            using (var dbContextTransaction = _dbContext.Database.BeginTransaction())
            {
                _dbContext.SaleReceipts.Add(receipt);
                await _dbContext.SaveChangesAsync();
                dbContextTransaction.Commit();
            }

            var context = ((IObjectContextAdapter)_dbContext).ObjectContext;
            var notifier = DefaultNotifier<Products, int>.GetInstance();
            var updatedProducts = receipt.SaleRows.Select(x => x.Products).ToList();
            foreach (var product in updatedProducts)
            {
                await context.RefreshAsync(System.Data.Entity.Core.Objects.RefreshMode.StoreWins, product);
                notifier.NotifyUpdate(product);
            }
        }
    }
}
