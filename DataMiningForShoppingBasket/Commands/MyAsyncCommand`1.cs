using System;
using System.Threading.Tasks;

namespace DataMiningForShoppingBasket.Commands
{
    public class MyAsyncCommand<TParam> : AsyncCommandBase
    {
        private readonly Func<TParam, Task> _exAction;
        private readonly Func<TParam, bool> _canExAction;

        /// <summary>
        /// Создание класса.
        /// </summary>
        /// <param name="exAction">Функция, возвращающая <see cref="Task"/>, который требуется выполнить асинхронно.</param>
        /// <param name="canExAction">Функция, возвращающая право на выполнение команды.</param>
        public MyAsyncCommand(Func<TParam, Task> exAction,
            Func<TParam, bool> canExAction = null)
        {
            _exAction = exAction;
            _canExAction = canExAction;
        }

        /// <inheritdoc />
        public override bool CanExecute(object parameter)
        {
            return _canExAction?.Invoke((TParam)parameter) != false && CanStartExecution();
        }
        
        /// <inheritdoc />
        protected override NotifyTaskCompletion CreateNotifyTaskCompletion(
            object parameter)
        {
            return new NotifyTaskCompletion(_exAction((TParam)parameter));
        }
    }
}
