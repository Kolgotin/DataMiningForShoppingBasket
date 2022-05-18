using System;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using DataMiningForShoppingBasket.CommonClasses;
using DataMiningForShoppingBasket.Events;
using DataMiningForShoppingBasket.Views;

namespace DataMiningForShoppingBasket.ViewModels
{
    class AuthorizationViewModel : INotifyPropertyChanged, IChangeWindowCallerDataContext
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        #endregion INotifyPropertyChanged

        public event ChangeWindowEventHandler ChangeWindowCalled;

        #region Commands
        private MyAsyncCommand<PasswordBox> _loginClickCommandAsync;
        public MyAsyncCommand<PasswordBox> LoginClickCommandAsync =>
            _loginClickCommandAsync ?? (_loginClickCommandAsync =
                new MyAsyncCommand<PasswordBox>(Login_HandlerAsync, obj => !AuthInProcess));
        #endregion Commands

        public string Login { get; set; } = "";
        public bool AuthInProcess => LoginClickCommandAsync?.IsActive == true;

        private async Task Login_HandlerAsync(PasswordBox passwordBox)
        {
            try
            {
                var password = passwordBox?.Password;

                var userTypeId = await CheckProfileAsync(password);
                var userWindow = GetWindowType(userTypeId);

                ChangeWindowCalled?.Invoke(this, userWindow);
            }
            catch (Exception ex)
            {
                MessageWriter.ShowMessage(ex.Message);
            }
        }

        private async Task<int?> CheckProfileAsync(string password)
        {
            var currentUser = GetData.Users.FirstOrDefault(x => x.UserName == Login.Trim());

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

        private IChangeWindowCaller GetWindowType(int? userTypeId)
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
