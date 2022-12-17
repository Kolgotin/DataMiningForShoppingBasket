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
        private readonly IGetData _getData;

        private static readonly Lazy<PrepareOfferSimpleHandler> Lazy =
            new Lazy<PrepareOfferSimpleHandler>(() => new PrepareOfferSimpleHandler());

        private PrepareOfferSimpleHandler()
        {
            _productsComparer = new EqualityProductsComparer();
            _rnd = new Random();
            _getData = GetData.GetInstance();
        }

        public static PrepareOfferSimpleHandler GetInstance() => Lazy.Value;

        public async Task<List<Products>> PrepareOffer(IEnumerable<Products> cart)
        {
            return (await _getData.GetListAsync<Products>().ConfigureAwait(false))
                .Where(x => x.WarehouseQuantity > 0)
                .Except(cart, _productsComparer)
                .OrderBy(x=> _rnd.Next())
                .Take(MaxOfferedProducts).ToList();
        }

        private class EqualityProductsComparer : IEqualityComparer<Products>
        {
            public bool Equals(Products x, Products y)
            {
                if (x is null || y is null) return false;
                if (ReferenceEquals(x, y)) return true;
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
