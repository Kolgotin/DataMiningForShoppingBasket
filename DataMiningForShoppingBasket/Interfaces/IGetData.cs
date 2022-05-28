using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataMiningForShoppingBasket.Interfaces
{
    public interface IGetData
    {
        Task<List<Users>> GetUsersAsync();
        Task<Users> GetUserAsync(string login);
        Task<List<Products>> GetProductsAsync();
        Task<List<Discounts>> GetDiscountsAsync();
        Task<int> SaveDiscountsAsync(Discounts discount);
    }
}
