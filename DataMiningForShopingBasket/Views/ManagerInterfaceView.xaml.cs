using DataMiningForShopingBasket.Events;
using DataMiningForShopingBasket.ViewModels;
using System.Windows.Controls;

namespace DataMiningForShopingBasket.Views
{
    /// <summary>
    /// Логика взаимодействия для ManagerInterfaceView.xaml
    /// </summary>
    public partial class ManagerInterfaceView : UserControl, IChangeWindowCaller
    {
        public IChangeWindowCallerDataContext CustomDataContext { get; set; }
            = new UserInterfaceViewModel();

        public ManagerInterfaceView()
        {
            InitializeComponent();
        }
    }
}
