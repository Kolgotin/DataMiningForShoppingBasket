using DataMiningForShopingBasket.Events;
using DataMiningForShopingBasket.ViewModels;
using System.Windows.Controls;

namespace DataMiningForShopingBasket.Views
{
    /// <summary>
    /// Логика взаимодействия для UserInterfaceView.xaml
    /// </summary>
    public partial class UserInterfaceView : UserControl, IChangeWindowCaller
    {
        public IChangeWindowCallerDataContext CustomDataContext { get; set; } 
            = new UserInterfaceViewModel();

        public UserInterfaceView()
        {
            InitializeComponent();
        }
    }
}
