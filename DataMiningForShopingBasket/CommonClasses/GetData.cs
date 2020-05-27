using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DataMiningForShopingBasket.CommonClasses
{
    public static class GetData
    {
        static DataMiningEntities dbContext = new DataMiningEntities();

        private static List<Users> _Users;
        public static List<Users> Users => _Users ??
            (_Users = dbContext.Users.ToList());

        private static List<UserTypes> _UsersTypes;
        public static List<UserTypes> UsersTypes => _UsersTypes ??
            (_UsersTypes = dbContext.UserTypes.ToList());

        private static List<Products> _Products;
        public static List<Products> Products => _Products ??
            (_Products = dbContext.Products.ToList());

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
