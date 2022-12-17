using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.Remoting.Contexts;
using System.Threading.Tasks;
using DataMiningForShoppingBasket.Interfaces;

namespace DataMiningForShoppingBasket.CommonClasses
{
    public class GetData : IGetData
    {
        private readonly DataMiningEntities _dbContext;

        private static readonly Lazy<GetData> Lazy =
            new Lazy<GetData>(() => new GetData());

        private GetData()
        {
            _dbContext = new DataMiningEntities();
        }

        public static GetData GetInstance() => Lazy.Value;
        
        public async Task<Users> GetUserAsync(string loginStr)
        {
            //todo: шифровать UserPassword
            var login = loginStr.Trim();
            var user = await _dbContext.Users.AsQueryable()
                .FirstOrDefaultAsync(x => x.UserName == login)
                .ConfigureAwait(false);
            return user;
        }
        
        public async Task<int> SaveDiscountsAsync(Discounts discount)
        {
            _ =_dbContext.Discounts.Add(discount);
            await _dbContext.SaveChangesAsync();
            return discount.Id;
        }

        public async Task<List<T>> GetListAsync<T>() where T : class
            => await _dbContext.Set<T>().ToListAsync().ConfigureAwait(false);

        public async Task<T> SaveEntityAsync<T>(T entity) where T : class
        {

            _dbContext.Set<T>().AddOrUpdate(entity);
            await _dbContext.SaveChangesAsync();
            return entity;
        }
    }
}
