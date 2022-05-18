using System.Windows.Controls;
using DataMiningForShopingBasket.Events;
using DataMiningForShopingBasket.ViewModels;

namespace DataMiningForShopingBasket.Views
{
    /// <summary>
    /// Логика взаимодействия для UserInterfaceView.xaml
    /// </summary>
    public partial class CashierInterfaceView : UserControl, IChangeWindowCaller
    {
        public IChangeWindowCallerDataContext CustomDataContext { get; set; } 
            = new CashierInterfaceViewModel();

        public CashierInterfaceView()
        {
            InitializeComponent();
        }
    }
}
