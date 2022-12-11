using DataMiningForShoppingBasket.CommonClasses;
using DataMiningForShoppingBasket.Commands;
using DataMiningForShoppingBasket.Interfaces;
using DataMiningForShoppingBasket.Views;
using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DataMiningForShoppingBasket.ViewModels
{
    public class AuthorizationViewModel : INotifyPropertyChanged
    {
        private readonly IGetData _getData;
        private readonly Window _window;

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        #endregion INotifyPropertyChanged

        #region Commands
        public MyAsyncCommand<PasswordBox> LoginCommand { get; }
        #endregion Commands

        public string Login { get; set; } = "";
        public bool AuthInProcess => LoginCommand?.IsActive == true;

        public AuthorizationViewModel(Window window)
        {
            _getData = GetData.Instance;
            _window = window;

            LoginCommand = new MyAsyncCommand<PasswordBox>(
                ExecuteLoginAsync,
                obj => !AuthInProcess);
        }

        private async Task ExecuteLoginAsync(PasswordBox passwordBox)
        {
            try
            {
                IUserControl userControl;
#if DEBUG
                userControl = new ManagerInterfaceView();
#endif

#if !DEBUG
                var password = passwordBox?.Password;

                var userTypeId = await CheckProfileAsync(password);
                userControl = GetWindowType(userTypeId);
#endif

                var newWindow = new MainWindow(userControl);
                _window.Close();
                newWindow.Show();
            }
            catch (Exception ex)
            {
                MessageWriter.ShowMessage(ex.Message);
            }
        }

        private async Task<int?> CheckProfileAsync(string password)
        {
            var currentUser = await _getData.GetUserAsync(Login);

            if (currentUser is null)
            {
                throw new MyException("Пользователь не найден");
            } 

            if (currentUser.UserPassword != password)
            {
                throw new MyException("Неверный пароль");
            }

            return currentUser.UserTypeId;
        }

        private IUserControl GetWindowType(int? userTypeId)
        {
            switch (userTypeId)
            {
                case 1:
                    return new ManagerInterfaceView();
                case 2:
                    return new CashierInterfaceView();
                default:
                    throw new MyException("Тип пользователя не определён");
            }
        }
    }
}
