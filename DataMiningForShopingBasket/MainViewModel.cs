﻿using DataMiningForShopingBasket.CommonClasses;
using System;
using System.ComponentModel;
using DataMiningForShopingBasket.Events;
using DataMiningForShopingBasket.Views;

namespace DataMiningForShopingBasket
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
        
        public IChangeWindowCaller CurrentUserControl { get; set; } 

        public MainViewModel()
        {
            ChangeWindow(new AuthorizationView());
        }

        private void ChangeWindowHandler(object sender, IChangeWindowCaller e)
        {
            try
            {
                ChangeWindow(e);
            }
            catch(Exception ex)
            {
                MessageWriter.ShowMessage(ex.Message);
            }
        }

        private void ChangeWindow(IChangeWindowCaller newWindow)
        {
            CurrentUserControl = newWindow;
            CurrentUserControl.DataContext = CurrentUserControl.CustomDataContext;
            CurrentUserControl.CustomDataContext.ChangeWindowCalled += ChangeWindowHandler;
            RaisePropertyChanged(nameof(CurrentUserControl));
        }
    }
}
