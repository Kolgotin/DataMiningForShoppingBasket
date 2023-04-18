using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataMiningForShoppingBasket.Common;
using DataMiningForShoppingBasket.Interfaces;

namespace DataMiningForShoppingBasket.Handlers
{
    public class AprioriAlgorithm : IPrepareOfferHandler
    {
        private static readonly Lazy<AprioriAlgorithm> Lazy =
            new Lazy<AprioriAlgorithm>(() => new AprioriAlgorithm());

        private readonly IDbManager _dbManager;

        private AprioriAlgorithm()
        {
            _dbManager = DbManager.GetInstance();
        }

        public static AprioriAlgorithm GetInstance() => Lazy.Value;

        public async Task<Dictionary<Products, decimal>> PrepareOffer(IReadOnlyCollection<Products> cart)
        {
            if (!cart.Any())
                return new Dictionary<Products, decimal>();

            var cartIds = cart.Select(x => x.Id).ToList();
            var discountsList = await _dbManager.GetListAsync<Discounts>().ConfigureAwait(false);
            var actualDiscountProducts = discountsList
                .Where(x => x.StartDate <= DateTime.Now && DateTime.Now <= x.FinishDate)
                .Select(x => x.Products).ToList();
            var actualDiscountProductIds = actualDiscountProducts
                .Select(x => x.Id)
                .Except(cartIds)
                .ToList();
            var discountSaleRows = await _dbManager.GetSalesByProductIds(actualDiscountProductIds);
            var cartSaleRows = await _dbManager.GetSalesByProductIds(cartIds);
            var intersection = discountSaleRows.Join(cartSaleRows,
                    x => x.SaleId,
                    y => y.SaleId,
                    (x, y) => x).ToList();
            var lonelyCount = discountSaleRows.GroupBy(x => x.ProductId)
                .ToDictionary(x => x.Key, x => x.Count());
            var intersectionCount = intersection.GroupBy(x => x.ProductId)
                .ToDictionary(x => x.Key, x => x.Count());

            var res = actualDiscountProducts.ToDictionary(x => x,
                x => intersectionCount.ContainsKey(x.Id)
                    ? 100m * intersectionCount[x.Id] / lonelyCount[x.Id]
                    : 0m);
            return res;
        }
    }
}
