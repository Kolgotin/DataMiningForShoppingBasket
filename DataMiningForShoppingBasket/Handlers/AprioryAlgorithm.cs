using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataMiningForShoppingBasket.Common;
using DataMiningForShoppingBasket.Interfaces;

namespace DataMiningForShoppingBasket.Handlers;

public class AprioriAlgorithm : IPrepareOfferHandler
{
    private const decimal MaxConfidence = 100m;
    private const decimal MinConfidence = 0m;

    private static readonly Lazy<AprioriAlgorithm> Lazy = new(() => new AprioriAlgorithm());

    private readonly IDbManager _dbManager;

    private AprioriAlgorithm()
    {
        _dbManager = DbManager.GetInstance();
    }

    public static AprioriAlgorithm GetInstance() => Lazy.Value;

    public async Task<List<(Products, decimal)>> PrepareOfferAsync(IReadOnlyCollection<Products> cart)
    {
        if (!cart.Any())
            return new List<(Products, decimal)>();

        var cartProductIds = cart.Select(x => x.Id).ToList();
        var focusProductsList = await _dbManager.GetListAsync<FocusProducts>().ConfigureAwait(false);
        var actualFocusProducts = focusProductsList
            .Where(x => x.IsActual() && x.Products.WarehouseQuantity > 0)
            .Select(x => x.Products).ToList();
        var actualFocusProductIds = actualFocusProducts
            .Select(x => x.Id)
            .Except(cartProductIds)
            .ToList();
        var cartSaleRows = await _dbManager.GetSalesByProductIds(cartProductIds);
        var focusProductsSaleRows = await _dbManager.GetSalesByProductIds(actualFocusProductIds);
        var intersection = focusProductsSaleRows.Join(cartSaleRows,
            x => x.SaleId, //todo: //? подумать, нужен ли для SaleRows Id?
            y => y.SaleId,
            (x, y) => x).ToList();
        var lonelyCount = focusProductsSaleRows.GroupBy(x => x.ProductId)
            .ToDictionary(x => x.Key, x => x.Count());
        var intersectionCount = intersection.GroupBy(x => x.ProductId)
            .ToDictionary(x => x.Key, x => x.Count());

        return actualFocusProducts.Select(x => (x,
                intersectionCount.ContainsKey(x.Id)
                    ? MaxConfidence * intersectionCount[x.Id] / lonelyCount[x.Id]
                    : MinConfidence))
            .ToList();
    }
}
