using System.Threading.Tasks;
using DataMiningForShoppingBasket.Interfaces;

namespace DataMiningForShoppingBasket.Common
{
    public class AsyncInitializedCreator<T> where T : IAsyncInitialized, new()
    {
        public static async Task<T> ConstructorAsync()
        {
            var instance = new T();
            await instance.InitializeAsync();
            return instance;
        }
    }
}