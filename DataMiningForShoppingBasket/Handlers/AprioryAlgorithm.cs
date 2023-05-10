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
        private const decimal MaxConfidence = 100m;
        private const decimal MinConfidence = 0m;

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

            var cartProductIds = cart.Select(x => x.Id).ToList();
            var focusProductsList = await _dbManager.GetListAsync<FocusProducts>().ConfigureAwait(false);
            var actualFocusProducts = focusProductsList
                .Where(x => x.StartDate <= DateTime.Now
                            && DateTime.Now <= x.FinishDate && x.Products.WarehouseQuantity > 0)
                .Select(x => x.Products).ToList();
            var actualFocusProductIds = actualFocusProducts
                .Select(x => x.Id)
                .Except(cartProductIds)
                .ToList();
            var focusProductsSaleRows = await _dbManager.GetSalesByProductIds(actualFocusProductIds);
            var cartSaleRows = await _dbManager.GetSalesByProductIds(cartProductIds);
            var intersection = focusProductsSaleRows.Join(cartSaleRows,
                    x => x.SaleId, //todo: //? подумать, нужен ли для SaleRows Id?
                    y => y.SaleId,
                    (x, y) => x).ToList();
            var lonelyCount = focusProductsSaleRows.GroupBy(x => x.ProductId)
                .ToDictionary(x => x.Key, x => x.Count());
            var intersectionCount = intersection.GroupBy(x => x.ProductId)
                .ToDictionary(x => x.Key, x => x.Count());

            return actualFocusProducts.ToDictionary(x => x,
                x => intersectionCount.ContainsKey(x.Id)
                    ? MaxConfidence * intersectionCount[x.Id] / lonelyCount[x.Id]
                    : MinConfidence);
        }
    }
}
