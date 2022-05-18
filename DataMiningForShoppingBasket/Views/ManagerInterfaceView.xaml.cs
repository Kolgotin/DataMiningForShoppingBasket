using System.Windows.Controls;
using DataMiningForShoppingBasket.Events;
using DataMiningForShoppingBasket.ViewModels;

namespace DataMiningForShoppingBasket.Views
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
