using System.Collections.Generic;
using System.Linq;

namespace DataMiningForShoppingBasket.CommonClasses
{
    public static class GetData
    {
        static DataMiningEntities dbContext = new DataMiningEntities();

        private static List<Users> _users;
        public static List<Users> Users => _users ??
            (_users = GetUsersAsync());

        private static List<Products> _products;
        public static List<Products> Products => _products ??
            (_products = GetProductsAsync());

        public static List<Users> GetUsersAsync() => dbContext.Users.AsQueryable().ToList();

        public static List<Products> GetProductsAsync() => dbContext.Products.AsQueryable().ToList();

        private static List<T> GetList<T>() where T : class
        {
            var res = dbContext.Set<T>().ToList();
            /*dbContext.Set(dbContext.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
             * .FirstOrDefault(y => y.PropertyType.GetTypeInfo().GenericTypeArguments[0].Name == nameType)
             * .PropertyType.GetTypeInfo().GenericTypeArguments[0]).ToListAsync().Result.ToList();
                   */
            return res;
        }
    }
}
