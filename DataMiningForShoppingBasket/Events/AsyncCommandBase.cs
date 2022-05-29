using System;
using System.Threading.Tasks;
using System.Windows.Input;
using DataMiningForShoppingBasket.Interfaces;
using DynamicData.Binding;

namespace DataMiningForShoppingBasket.Events
{
    public abstract class  AsyncCommandBase : AbstractNotifyPropertyChanged, IAsyncCommand
    {
        private bool _isActive;
        private NotifyTaskCompletion _execution;

        protected AsyncCommandBase()
        {
            Execution = null;
        }

        /// <summary>
        /// Объект, наблюдающий за выполнением асинхронной операции
        /// и обновляющий свой статус в соответствии с процессом выполнения.
        /// </summary>
        public NotifyTaskCompletion Execution
        {
            get => _execution;
            private set => SetAndRaise(ref _execution, value);
        }

        /// <inheritdoc />
        public bool IsActive
        {
            get => _isActive;
            set => throw new InvalidOperationException();
        }


        /// <inheritdoc cref="ICommand.CanExecute(object)"/>
        public abstract bool CanExecute(object parameter);

        /// <inheritdoc cref="ICommand.Execute(object)"/>
        async void ICommand.Execute(object parameter)
        {
            await InnerExecuteAsync(parameter);
            
            if (Execution?.IsFaulted == true)
            {
                throw Execution.Exception;
            }
        }

        /// <inheritdoc cref="ICommand.CanExecuteChanged"/>
        event EventHandler ICommand.CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        /// <summary>
        /// Создание класса слежения за процессом и результатом выполнения команды.
        /// </summary>
        /// <param name="parameter">Параметр, передаваемый при вызове команды.</param>
        /// <returns>Класс слежения за процессом и результатом выполнения команды.</returns>
        protected abstract NotifyTaskCompletion CreateNotifyTaskCompletion(object parameter);
        
        /// <summary>
        /// Выполнение действия команды.
        /// </summary>
        /// <param name="parameter">Параметр команды.</param>
        protected async Task InnerExecuteAsync(object parameter)
        {
            Execution = CreateNotifyTaskCompletion(parameter);
            if (Execution.TaskCompletion != null)
            {
                RaiseCanExecuteChanged(true);
                await Execution.TaskCompletion;
                RaiseCanExecuteChanged(false);
            }
        }

        /// <summary>
        /// Проверка возможности выполения команды.
        /// </summary>
        /// <returns>Призак разрешения выполнения команды.</returns>
        protected bool CanStartExecution()
        {
            return Execution == null || Execution.IsCompleted;
        }

        /// <summary>
        /// Принудительный вызов события изменения состояния команды.
        /// </summary>
        private void RaiseCanExecuteChanged(bool value)
        {
            _isActive = value;
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
