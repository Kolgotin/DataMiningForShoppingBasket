using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using DynamicData.Binding;

namespace DataMiningForShoppingBasket.Events
{
    public abstract class  AsyncCommandBase : AbstractNotifyPropertyChanged, IAsyncCommand
    {
        private bool _isActive;
        private NotifyTaskCompletion _execution;

        /// <summary>
        /// Создание класса.
        /// </summary>
        protected AsyncCommandBase()
        {
            Execution = null;
        }

        /// <inheritdoc />
        public abstract ICommand CancelCommand { get; }

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

        /// <inheritdoc />
        public event EventHandler IsActiveChanged;

        /// <inheritdoc cref="ICommand.CanExecute(object)"/>
        public abstract bool CanExecute(object parameter);

        /// <inheritdoc cref="ICommand.Execute(object)"/>
        async void ICommand.Execute(object parameter)
        {
            await InnerExecuteAsync(parameter);
            if (Execution?.IsFaulted == true)
                throw Execution.Exception;
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

        protected abstract void NotifyCommandStarting();

        protected abstract void NotifyCommandFinished();

        /// <summary>
        /// Выполнение действия команды.
        /// </summary>
        /// <param name="parameter">Параметр команды.</param>
        protected async Task InnerExecuteAsync(object parameter)
        {
            NotifyCommandStarting();
            Execution = CreateNotifyTaskCompletion(parameter);
            if (Execution.TaskCompletion != null)
            {
                RaiseCanExecuteChanged(true);
                await Execution.TaskCompletion;
                RaiseCanExecuteChanged(false);
            }
            NotifyCommandFinished();
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
            IsActiveChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
