using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using DataMiningForShoppingBasket.CommonClasses;
using DataMiningForShoppingBasket.Commands;
using DataMiningForShoppingBasket.Interfaces;
using DataMiningForShoppingBasket.Views;

namespace DataMiningForShoppingBasket
{
    public class MainViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        public void RaisePropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        #endregion

        public MyAsyncCommand<Window> ExitCommand { get; }

        public IUserControl CurrentUserControl { get; set; }

        public MainViewModel(IUserControl userControl)
        {
            ExitCommand = new MyAsyncCommand<Window>(ExitHandlerAsync, obj => ExitCommand?.IsActive == false);
            ChangeWindow(userControl);
        }

        private void ChangeWindow(IUserControl userControl)
        {
            CurrentUserControl = userControl;
            CurrentUserControl.DataContext = CurrentUserControl.CustomDataContext;
            RaisePropertyChanged(nameof(CurrentUserControl));
        }

        private Task ExitHandlerAsync(Window window)
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

            return Task.CompletedTask;
        }
    }
}
