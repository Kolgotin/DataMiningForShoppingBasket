using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataMiningForShoppingBasket.Interfaces
{
    public interface IPrepareOfferHandler
    {
        Task<List<(Products, decimal)>> PrepareOfferAsync(IReadOnlyCollection<Products> cart);
    }
}
