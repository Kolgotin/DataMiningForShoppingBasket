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

        public async Task<List<(Products, decimal)>> PrepareOfferAsync(IReadOnlyCollection<Products> cart)
        {
            var focusProductsList = await _dbManager.GetListAsync<FocusProducts>().ConfigureAwait(false);
            var actualFocusProductsList = focusProductsList
                .Where(x => x.IsActual())
                .ToList();
            var productsList = await _dbManager.GetListAsync<Products>().ConfigureAwait(false);
            return productsList
                .Where(x => x.WarehouseQuantity > 0)
                .Except(cart, _productsComparer)
                .Join(actualFocusProductsList,
                    x => x.Id,
                    y => y.ProductId,
                    (x, y) => x)
                .Take(MaxOfferedProducts)
                .Select(x => (x, 1m *_rnd.Next(100, 200) / _rnd.Next(3, 100)))
                .ToList();
        }
    }
}
