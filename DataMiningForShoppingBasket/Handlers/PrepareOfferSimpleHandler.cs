using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataMiningForShoppingBasket.Common;
using DataMiningForShoppingBasket.Interfaces;

namespace DataMiningForShoppingBasket.Handlers
{
    public class PrepareOfferSimpleHandler : IPrepareOfferHandler
    {
        private const int MaxOfferedProducts = 5;

        private readonly EqualityProductsComparer _productsComparer;
        private readonly Random _rnd;
        private readonly IDbManager _dbManager;

        private static readonly Lazy<PrepareOfferSimpleHandler> Lazy =
            new Lazy<PrepareOfferSimpleHandler>(() => new PrepareOfferSimpleHandler());

        private PrepareOfferSimpleHandler()
        {
            _productsComparer = new EqualityProductsComparer();
            _rnd = new Random();
            _dbManager = DbManager.GetInstance();
        }

        public static PrepareOfferSimpleHandler GetInstance() => Lazy.Value;

        public async Task<Dictionary<Products, decimal>> PrepareOffer(IEnumerable<Products> cart)
        {
            var discountsList = await _dbManager.GetListAsync<Discounts>().ConfigureAwait(false);
            var actualDiscountsList = discountsList.Where(x => x.StartDate <= DateTime.Now && DateTime.Now <= x.FinishDate)
                .ToList();
            var productsList = await _dbManager.GetListAsync<Products>().ConfigureAwait(false);
            var dict = productsList
                .Where(x => x.WarehouseQuantity > 0)
                .Except(cart, _productsComparer)
                .Join(actualDiscountsList,
                    x => x.Id,
                    y => y.ProductId,
                    (x, y) => x)
                .Take(MaxOfferedProducts).ToDictionary(x => x, x => 1m *_rnd.Next(100, 200) / _rnd.Next(3, 100));
            return dict;
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
