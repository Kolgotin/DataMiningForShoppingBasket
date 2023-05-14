using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataMiningForShoppingBasket.Common;
using DataMiningForShoppingBasket.Interfaces;

namespace DataMiningForShoppingBasket.Handlers;

public class AprioriAlgorithm3Deep : IPrepareOfferHandler
{
    private const decimal MaxConfidence = 100m;
    private const decimal MinConfidence = 0m;

    private static readonly Lazy<AprioriAlgorithm3Deep> Lazy = new(() => new AprioriAlgorithm3Deep());

    private readonly IDbManager _dbManager;

    public AprioriAlgorithm3Deep()
    {
        _dbManager = DbManager.GetInstance();
    }

    public static AprioriAlgorithm3Deep GetInstance() => Lazy.Value;

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

        var focusProductsSaleRows = await _dbManager.GetSalesByProductIds(actualFocusProductIds);

        var focusProductSalesDict = focusProductsSaleRows.GroupBy(x => x.ProductId)
            .ToDictionary(x => x.Key, x => x.Select(y => y.SaleId).ToList());

        var cartSaleRows = await _dbManager.GetSalesByProductIds(cartProductIds);
        var cartProductsSalesDictionary = cartSaleRows.GroupBy(x => x.ProductId)
            .ToDictionary(x => 
                x.Key, x => x.Select(y => y.SaleId).ToList());

        var prevItemSetsSaleIds = cartProductsSalesDictionary
            .Select(x => (Key: new IntHashSet {x.Key}, x.Value))
            .ToList();

        var itemSetsSalesIntersections = new List<(IntHashSet, List<int>)>();

        foreach (var productId in cartProductsSalesDictionary.Keys.ToList())
        {
            var temp = prevItemSetsSaleIds
                .Where(x => !x.Key.Contains(productId))
                .Select(x => (new IntHashSet(x.Key) {productId}
                    , x.Value.Intersect(cartProductsSalesDictionary[productId]).ToList()))
                .Where(x => x.Item2.Any())
                .ToList();
            var newTemp = temp
                .Where(x => !itemSetsSalesIntersections.Any(y=> y.Item1.Equals(x.Item1)))
                .ToList();

            itemSetsSalesIntersections.AddRange(newTemp);
        }

        var salesId = itemSetsSalesIntersections.SelectMany(x => x.Item2).ToList();
        var intersectionCount = actualFocusProductIds.ToDictionary(x => x,
            x => salesId.Intersect(focusProductSalesDict[x]).Count());

        return actualFocusProducts.Select(x => (x,
            intersectionCount.ContainsKey(x.Id)
                ? MaxConfidence * intersectionCount[x.Id] / focusProductSalesDict[x.Id].Count
                : MinConfidence))
            .ToList();
    }

    private class IntHashSet: HashSet<int>, IEquatable<IntHashSet>, IEqualityComparer<IntHashSet>
    {
        public IntHashSet()
            : base(EqualityComparer<int>.Default)
        {
        }

        public IntHashSet(IEnumerable<int> collection)
            : base(collection, EqualityComparer<int>.Default)
        {
        }

        public bool Equals(IntHashSet other)
            => Equals(this, other);

        public bool Equals(IntHashSet x, IntHashSet y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (x is null) return false;
            if (y is null) return false;
            if (x.Count != y.Count) return false;

            return !x.Except(y).Any();
        }

        public int GetHashCode(IntHashSet obj)
            => obj.Aggregate(obj.Count, (current, x) => (current * 397) ^ x);
    }
}