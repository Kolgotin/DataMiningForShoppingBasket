using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using DataMiningForShoppingBasket.Interfaces;

namespace DataMiningForShoppingBasket.CommonClasses
{
    public class GetData : IGetData
    {
        private readonly DataMiningEntities _dbContext;

        private static GetData _instance;
        public static GetData Instance => _instance ?? (_instance = new GetData());

        private GetData()
        {
            _dbContext = new DataMiningEntities();
        }

        public async Task<List<Users>> GetUsersAsync()
            => await _dbContext.Users.AsQueryable().ToListAsync().ConfigureAwait(false);

        public async Task<Users> GetUserAsync(string loginStr)
        {
            var login = loginStr.Trim();
            var user = await _dbContext.Users.AsQueryable()
                .FirstOrDefaultAsync(x => x.UserName == login)
                .ConfigureAwait(false);
            return user;
        }

        public async Task<List<Products>> GetProductsAsync()
            => await _dbContext.Products.AsQueryable().ToListAsync().ConfigureAwait(false);

        private async Task<List<T>> GetList<T>() where T : class
        {
            var res = await _dbContext.Set<T>().ToListAsync();
            /*dbContext.Set(dbContext.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
             * .FirstOrDefault(y => y.PropertyType.GetTypeInfo().GenericTypeArguments[0].Name == nameType)
             * .PropertyType.GetTypeInfo().GenericTypeArguments[0]).ToListAsync().Result.ToList();
                   */
            return res;
        }
    }
}
