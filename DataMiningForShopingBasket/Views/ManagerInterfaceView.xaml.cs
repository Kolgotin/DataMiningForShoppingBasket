using System.Windows.Controls;
using DataMiningForShopingBasket.Events;
using DataMiningForShopingBasket.ViewModels;

namespace DataMiningForShopingBasket.Views
{
    /// <summary>
    /// Логика взаимодействия для ManagerInterfaceView.xaml
    /// </summary>
    public partial class ManagerInterfaceView : UserControl, IChangeWindowCaller
    {
        public IChangeWindowCallerDataContext CustomDataContext { get; set; }
            = new CashierInterfaceViewModel();

        public ManagerInterfaceView()
        {
            InitializeComponent();
        }
    }
}
