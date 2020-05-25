using DataMiningForShopingBasket.Events;
using DataMiningForShopingBasket.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

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
        
        public IChangeWindowCaller CurrentUC { get; set; } 

        public MainViewModel()
        {
            //ChangeWindow(new AuthorizationView()); 
            ChangeWindow(new UserInterfaceView()); 
        }

        private void ChangeWindowHandler(object sender, IChangeWindowCaller e)
        {
            ChangeWindow(e);
        }

        private void ChangeWindow(IChangeWindowCaller newWindow)
        {
            CurrentUC = newWindow;
            CurrentUC.DataContext = CurrentUC.CustomDataContext;
            CurrentUC.CustomDataContext.ChangeWindowCalled += ChangeWindowHandler;
            RaisePropertyChanged("CurrentUC");
        }
    }
}
