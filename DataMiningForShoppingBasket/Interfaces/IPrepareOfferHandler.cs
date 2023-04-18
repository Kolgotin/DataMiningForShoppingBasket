using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataMiningForShoppingBasket.Interfaces
{
    public interface IPrepareOfferHandler
    {
        Task<Dictionary<Products, decimal>> PrepareOffer(IReadOnlyCollection<Products> cart);
    }
}
