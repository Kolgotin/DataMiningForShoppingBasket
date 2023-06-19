using DataMiningForShoppingBasket.Commands;
using DataMiningForShoppingBasket.Common;
using DataMiningForShoppingBasket.Interfaces;
using DataMiningForShoppingBasket.Views;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace DataMiningForShoppingBasket.ViewModels
{
    public class AuthorizationViewModel : NotifyPropertyChangedImplementation
    {
        private readonly IDbManager _dbManager;
        private readonly Window _window;

        #region Commands
        public MyAsyncCommand<PasswordBox> LoginCommand { get; }
        #endregion Commands

        public string Login { get; set; } = "";
        public bool AuthInProcess => LoginCommand?.IsActive == true;

        public AuthorizationViewModel(Window window)
        {
            _dbManager = DbManager.GetInstance();
            _window = window;

            LoginCommand = new MyAsyncCommand<PasswordBox>(
                ExecuteLoginAsync,
                obj => !AuthInProcess);
        }

        private async Task ExecuteLoginAsync(PasswordBox passwordBox)
        {
            try
            {
#if DEBUG
                CurrentSession.CurrentUser = await DebugCheckProfileAsync(UserType.Cashier);
#else
                var password = passwordBox?.Password;
                CurrentSession.CurrentUser = await CheckProfileAsync(password);
#endif

                var userControl = GetWindowType(CurrentSession.CurrentUser.UserTypeId);
                var newWindow = new MainWindow(userControl);
                _window.Close();
                newWindow.Show();
            }
            catch (Exception ex)
            {
                MessageWriter.ShowMessage(ex.Message);
            }
        }

        private static Task<Users> DebugCheckProfileAsync(UserType userType)
        {
            var currentUser = userType switch
            {
                UserType.Manager => new Users {Id = 1, UserTypeId = 1, UserName = "manager"},
                UserType.Cashier => new Users {Id = 2, UserTypeId = 2, UserName = "cashier"},
                _ => throw new ArgumentOutOfRangeException(nameof(userType), userType, null)
            };

            return Task.FromResult(currentUser);
        }

        private async Task<Users> CheckProfileAsync(string password)
        {
            var currentUser = await _dbManager.GetUserAsync(Login);

            if (currentUser is null)
            {
                throw new MyException("Пользователь не найден");
            } 

            if (currentUser.UserPassword != password)
            {
                throw new MyException("Неверный пароль");
            }

            return currentUser;
        }

        private IUserControl GetWindowType(int? userTypeId)
        {
            return userTypeId switch
            {
                1 => new ManagerInterfaceView(),
                2 => new CashierInterfaceView(),
                _ => throw new MyException("Тип пользователя не определён")
            };
        }
    }
}
