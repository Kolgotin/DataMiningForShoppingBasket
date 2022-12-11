using System;
using System.Threading.Tasks;

namespace DataMiningForShoppingBasket.Commands
{
    public class MyAsyncCommand : AsyncCommandBase
    {
        private readonly Func<Task> _exAction;
        private readonly Func<object, bool> _canExAction;

        /// <summary>
        /// Создание класса.
        /// </summary>
        /// <param name="exAction">Функция, возвращающая <see cref="Task"/>, который требуется выполнить асинхронно.</param>
        /// <param name="canExAction">Функция, возвращающая право на выполнение команды.</param>
        public MyAsyncCommand(Func<Task> exAction,
            Func<object, bool> canExAction = null)
        {
            _exAction = exAction;
            _canExAction = canExAction;
        }

        /// <inheritdoc />
        public override bool CanExecute(object parameter)
        {
            return _canExAction?.Invoke(parameter) != false && CanStartExecution();
        }

        /// <inheritdoc />
        protected override NotifyTaskCompletion CreateNotifyTaskCompletion(
            object parameter)
        {
            return new NotifyTaskCompletion(_exAction());
        }
    }
}
