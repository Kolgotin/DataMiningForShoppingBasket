using System;
using System.Windows.Input;

namespace DataMiningForShopingBasket.Events
{
    interface IAsyncCommand : ICommand
    {
        bool IsActive { get; set; }

        event EventHandler IsActiveChanged;

        /// <summary>
        /// Команда отмены для выполняемой команды.
        /// </summary>
        ICommand CancelCommand { get; }
    }
}
