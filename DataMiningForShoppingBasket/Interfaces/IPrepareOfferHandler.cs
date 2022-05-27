using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataMiningForShoppingBasket.Interfaces
{
    public interface IPrepareOfferHandler
    {
        Task<List<Products>> PrepareOffer(IEnumerable<Products> cart);
    }
}
