using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataMiningForShoppingBasket.CommonClasses;
using DataMiningForShoppingBasket.Interfaces;

namespace DataMiningForShoppingBasket.Handlers
{
    public class PrepareOfferSimpleHandler : IPrepareOfferHandler
    {
        private const int MaxOfferedProducts = 5;

        private readonly EqualityProductsComparer _productsComparer;
        private readonly Random _rnd;

        private static PrepareOfferSimpleHandler _instance;

        public static PrepareOfferSimpleHandler Instance =>
            _instance ?? (_instance = new PrepareOfferSimpleHandler());

        private PrepareOfferSimpleHandler()
        {
            _productsComparer = new EqualityProductsComparer();
            _rnd = new Random();
        }

        public async Task<List<Products>> PrepareOffer(IEnumerable<Products> cart)
        {
            return (await GetData.Instance.GetProductsAsync())
                .Where(x => x.WarehouseQuantity > 0)
                .Except(cart, _productsComparer)
                .OrderBy(x=> _rnd.Next())
                .Take(MaxOfferedProducts).ToList();
        }

        private class EqualityProductsComparer : IEqualityComparer<Products>
        {
            public bool Equals(Products x, Products y)
            {
                if (ReferenceEquals(x, y)) return true;
                if (ReferenceEquals(x, null)) return false;
                if (ReferenceEquals(y, null)) return false;
                if (x.GetType() != y.GetType()) return false;
                return x.Id == y.Id;
            }

            public int GetHashCode(Products obj)
            {
                return obj.Id;
            }
        }
    }
}
