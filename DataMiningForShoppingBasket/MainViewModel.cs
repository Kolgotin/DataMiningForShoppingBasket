using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using DataMiningForShoppingBasket.CommonClasses;
using DataMiningForShoppingBasket.Events;
using DataMiningForShoppingBasket.Interfaces;
using DataMiningForShoppingBasket.Views;

namespace DataMiningForShoppingBasket
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private MyAsyncCommand<Window> _exitCommand;

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        #endregion

        public MyAsyncCommand<Window> ExitCommand =>
            _exitCommand ?? (_exitCommand =
                new MyAsyncCommand<Window>(ExitHandlerAsync, obj => _exitCommand?.IsActive == false));

        public IUserControl CurrentUserControl { get; set; }

        public MainViewModel(IUserControl userControl)
        {
            ChangeWindow(userControl);
        }

        private void ChangeWindow(IUserControl userControl)
        {
            CurrentUserControl = userControl;
            CurrentUserControl.DataContext = CurrentUserControl.CustomDataContext;
            RaisePropertyChanged(nameof(CurrentUserControl));
        }

        private async Task ExitHandlerAsync(Window window)
        {
            try
            {
                var newWindow = new AuthorizationView();
                window.Close();
                newWindow.Show();
            }
            catch (Exception e)
            {
                MessageWriter.ShowMessage(e.Message);
            }
        }
    }
}
