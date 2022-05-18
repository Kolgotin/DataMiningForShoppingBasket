using System;
using System.Threading.Tasks;
using DynamicData.Binding;

namespace DataMiningForShopingBasket.Events
{
    public class NotifyTaskCompletion : AbstractNotifyPropertyChanged
    {
        private readonly Task _task;

        /// <summary>
        /// Конструктор объекта.
        /// </summary>
        public NotifyTaskCompletion(Task task)
        {
            _task = task;
            if (task != null && !task.IsCompleted)
            {
                TaskCompletion = WatchTaskAsync(task);
            }
        }

        /// <summary>
        /// Обновление состояние объекта на основе состояния переданной <see cref="Task"/>.
        /// </summary>
        /// <param name="task"><see cref="Task"/>, состояние которой требуется использовать.</param>

        /// <summary>
        /// Метод, создающий <see cref="Task"/>, отслеживающий статус указанного
        /// объекта <see cref="Task"/> и соответствующим образом изменяющий
        /// состояние объекта в процессе, а также по окончании его выполнения.
        /// </summary>
        /// <returns></returns>
        private async Task WatchTaskAsync(Task task)
        {
            try
            {
                await task;
            }
            catch
            {
                //ignored
            }

            // ReSharper disable ExplicitCallerInfoArgument
            OnPropertyChanged(nameof(Status));
            OnPropertyChanged(nameof(IsCompleted));
            OnPropertyChanged(nameof(IsNotCompleted));
            // ReSharper restore ExplicitCallerInfoArgument

            if (task.IsCanceled)
            {
                // ReSharper disable once ExplicitCallerInfoArgument
                OnPropertyChanged(nameof(IsCanceled));
            }
            else if (task.IsFaulted)
            {
                // ReSharper disable ExplicitCallerInfoArgument
                OnPropertyChanged(nameof(IsFaulted));
                OnPropertyChanged(nameof(Exception));
                OnPropertyChanged(nameof(InnerException));
                OnPropertyChanged(nameof(ErrorMessage));
                // ReSharper restore ExplicitCallerInfoArgument
            }
            else
            {
                // ReSharper disable once ExplicitCallerInfoArgument
                OnPropertyChanged(nameof(IsSuccessfullyCompleted));
            }
        }

        /// <summary>
        /// <see cref="Task"/>, запуск выполнения которой
        /// происходит с отслеживанием её состояния.
        /// </summary>
        public Task TaskCompletion { get; }

        /// <summary>
        /// Статус выполнения <see cref="Task"/>.
        /// </summary>
        public TaskStatus Status => _task.Status;

        /// <summary>
        /// Признак завершения выполнения <see cref="Task"/>.
        /// </summary>
        public bool IsCompleted => _task.IsCompleted;

        /// <summary>
        /// Признак продолжения выполнения <see cref="Task"/>.
        /// </summary>
        public bool IsNotCompleted => !_task.IsCompleted;

        /// <summary>
        /// Признак успешного завершения <see cref="Task"/>.
        /// </summary>
        public bool IsSuccessfullyCompleted => _task.Status == TaskStatus.RanToCompletion;

        /// <summary>
        /// Признак отмены <see cref="Task"/>.
        /// </summary>
        public bool IsCanceled => _task.IsCanceled;

        /// <summary>
        /// Признак завершения <see cref="Task"/> с ошибкой.
        /// </summary>
        public bool IsFaulted => _task.IsFaulted;

        /// <summary>
        /// Информация об ошибке, произошедшей при выполнении асинхронной операции.
        /// </summary>
        public AggregateException Exception => _task.Exception;

        /// <summary>
        /// Получение исключения, произошедшего внутри потока, выполнявшего <see cref="Task"/>.
        /// </summary>
        public Exception InnerException => Exception?.InnerException;

        /// <summary>
        /// Получение текста сообщения об ошибке.
        /// </summary>
        public string ErrorMessage => InnerException?.Message;
    }
}
