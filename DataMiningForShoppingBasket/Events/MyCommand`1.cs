using System;
using System.Windows.Input;

namespace DataMiningForShoppingBasket.Events
{
    public class MyCommand<T> : ICommand
    {
        private readonly Predicate<T> _canExecute;
        private readonly Action<T> _execute;

        public MyCommand(Action<T> execute)
            : this(execute, _ => true)
        {
        }

        public MyCommand(Action<T> execute, Predicate<T> canExecute)
        {
            _canExecute = canExecute;
            _execute = execute;
        }

        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute((T)parameter);
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }
    }
}
