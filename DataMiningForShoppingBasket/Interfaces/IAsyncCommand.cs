using System.Windows.Input;

namespace DataMiningForShoppingBasket.Interfaces
{
    public interface IAsyncCommand : ICommand
    {
        bool IsActive { get; }
    }
}
