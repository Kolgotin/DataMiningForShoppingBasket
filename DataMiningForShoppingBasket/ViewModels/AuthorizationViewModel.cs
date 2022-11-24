using System;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Controls;
using DataMiningForShoppingBasket.CommonClasses;
using DataMiningForShoppingBasket.Events;
using DataMiningForShoppingBasket.Interfaces;
using DataMiningForShoppingBasket.Views;

namespace DataMiningForShoppingBasket.ViewModels
{
    public class AuthorizationViewModel : INotifyPropertyChanged, IChangeWindowCallerDataContext
    {
        private readonly IGetData _getData;

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        #endregion INotifyPropertyChanged

        #region IChangeWindowCallerDataContext
        public event ChangeWindowEventHandler ChangeWindowCalled;

        public string WindowLabel => "Авторизация";
        #endregion

        #region Commands
        public MyAsyncCommand<PasswordBox> LoginCommand { get; }
        #endregion Commands

        public string Login { get; set; } = "";
        public bool AuthInProcess => LoginCommand?.IsActive == true;

        public AuthorizationViewModel()
        {
            _getData = GetData.Instance;

            LoginCommand = new MyAsyncCommand<PasswordBox>(
                ExecuteLoginAsync,
                obj => !AuthInProcess);
        }

        private async Task ExecuteLoginAsync(PasswordBox passwordBox)
        {
            try
            {
#if DEBUG
                ChangeWindowCalled?.Invoke(this, new CashierInterfaceView());
                return;
#endif
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
