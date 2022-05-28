using System.Windows.Input;

namespace DataMiningForShoppingBasket.Interfaces
{
    interface IAsyncCommand : ICommand
    {
        bool IsActive { get; set; }
    }
}
