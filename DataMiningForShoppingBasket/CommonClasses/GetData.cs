using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataMiningForShoppingBasket.CommonClasses
{
    public static class GetData
    {
        static DataMiningEntities dbContext = new DataMiningEntities();

        private static List<Users> _users;
        public static List<Users> Users => _users ??
            (_users = GetUsersAsync());

        private static List<UserTypes> _usersTypes;
        public static List<UserTypes> UsersTypes => _usersTypes ??
            (_usersTypes = dbContext.UserTypes.ToList());

        private static List<Products> _products;
        public static List<Products> Products => _products ??
            (_products = GetProductsAsync());

        public static List<Users> GetUsersAsync()
        {
            var users = dbContext.Users.AsQueryable().ToListAsync().Result;
            return users;
        }

        public static List<Products> GetProductsAsync()
        {
            return dbContext.Products.AsQueryable().ToListAsync().Result;
        }

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
