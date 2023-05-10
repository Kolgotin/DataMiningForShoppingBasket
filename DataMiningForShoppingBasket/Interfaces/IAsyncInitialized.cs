using System.Threading.Tasks;

namespace DataMiningForShoppingBasket.Interfaces
{
    public interface IAsyncInitialized
    {
        Task InitializeAsync();
    }
}