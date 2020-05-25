using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMiningForShopingBasket.CommonClasses
{
    public static class GetData
    {
        static DataMiningEntities db = new DataMiningEntities();

        private static List<Users> _Users;
        public static List<Users> Users => _Users ??
            (_Users = db.Users.ToList());

        private static List<Products> _Products;
        public static List<Products> Products => _Products ??
            (_Products = db.Products.ToList());
    }
}
