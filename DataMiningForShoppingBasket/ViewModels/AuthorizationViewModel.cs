﻿using DataMiningForShoppingBasket.Commands;
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
        private readonly IGetData _getData;
        private readonly Window _window;

        #region Commands
        public MyAsyncCommand<PasswordBox> LoginCommand { get; }
        #endregion Commands

        public string Login { get; set; } = "";
        public bool AuthInProcess => LoginCommand?.IsActive == true;

        public AuthorizationViewModel(Window window)
        {
            _getData = GetData.GetInstance();
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
                DebugCheckProfile(UserType.Cashier);
#endif

#if !DEBUG
                var password = passwordBox?.Password;
                await CheckProfileAsync(password);
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

        private static void DebugCheckProfile(UserType userType)
        {
            switch (userType)
            {
                case UserType.Manager:
                    CurrentSession.CurrentUser =
                    new Users
                        {
                            Id = 1,
                            UserTypeId = 1,
                            UserName = "manager"
                    };
                    break;
                case UserType.Cashier:
                    CurrentSession.CurrentUser =
                    new Users
                    {
                        Id = 2,
                        UserTypeId = 2,
                        UserName = "cashier"
                    };
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(userType), userType, null);
            }
        }

        private async Task CheckProfileAsync(string password)
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

            CurrentSession.CurrentUser = currentUser;
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
