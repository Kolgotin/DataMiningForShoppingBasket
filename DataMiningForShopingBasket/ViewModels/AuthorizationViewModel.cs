using DataMiningForShopingBasket.CommonClasses;
using DataMiningForShopingBasket.Events;
using DataMiningForShopingBasket.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMiningForShopingBasket.ViewModels
{
    class AuthorizationViewModel : INotifyPropertyChanged, IChangeWindowCallerDataContext
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
        #endregion

        public event ChangeWindowEventHandler ChangeWindowCalled;

        #region Commands
        private MyCommand _LoginClickCommand;
        public MyCommand LoginClickCommand =>
            _LoginClickCommand ?? (_LoginClickCommand =
                new MyCommand((obj) => true, (obj) => LoginClick_Handler()));
        #endregion

        public string Login { get; set; }
        public string Password { get; set; }

        private void LoginClick_Handler()
        {
            var userTypeId = CheckProfile();
            var userWindow = GetWindowType(userTypeId);

            ChangeWindowCalled(this, userWindow);
        }

        private int? CheckProfile()
        {
            using (var db = new DataMiningEntities())
            {
                var currentUser = GetData.Users.FirstOrDefault(x => x.UserName.Trim() == Login.Trim());
                if (currentUser is null)
                {
                    throw new MyException("Пользователь не найден");
                }
                if(currentUser.UserPassword.Trim() != Password.Trim())
                {
                    throw new MyException("Неверный пароль");
                }
                return currentUser.UserTypeId;
            }
        }

        private IChangeWindowCaller GetWindowType(int? userTypeId)
        {
            switch (userTypeId)
            {
                case 1:
                    return new UserInterfaceView();
                default:
                    throw new MyException("Тип пользователя не определён");
            }
        }
    }
}
