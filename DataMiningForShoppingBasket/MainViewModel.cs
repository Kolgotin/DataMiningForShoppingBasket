using DataMiningForShoppingBasket.Commands;
using DataMiningForShoppingBasket.Common;
using DataMiningForShoppingBasket.Interfaces;
using DataMiningForShoppingBasket.Views;
using System;
using System.Threading.Tasks;
using System.Windows;

namespace DataMiningForShoppingBasket
{
    public class MainViewModel : NotifyPropertyChangedImplementation
    {
        public IAsyncCommand ExitCommand { get; }
        public IUserControl CurrentUserControl { get; set; }

        public MainViewModel(IUserControl userControl)
        {
            ExitCommand = new MyAsyncCommand<Window>(ExecuteExitAsync,
                obj => ExitCommand?.IsActive == false);
            ChangeWindow(userControl);
        }

        private void ChangeWindow(IUserControl userControl)
        {
            CurrentUserControl = userControl;
            CurrentUserControl.DataContext = CurrentUserControl.CustomDataContext;
            RaisePropertyChanged(nameof(CurrentUserControl));
        }

        private static Task ExecuteExitAsync(Window window)
        {
            try
            {
                var newWindow = new AuthorizationView();
                window.Close();
                CurrentSession.Clear();
                newWindow.Show();
            }
            catch (Exception e)
            {
                MessageWriter.ShowMessage(e.Message);
                Application.Current.Shutdown();
            }

            return Task.CompletedTask;
        }
    }
}
