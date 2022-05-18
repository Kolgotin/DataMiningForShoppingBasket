using System.Windows.Controls;
using DataMiningForShopingBasket.Events;
using DataMiningForShopingBasket.ViewModels;

namespace DataMiningForShopingBasket.Views
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationView.xaml
    /// </summary>
    public partial class AuthorizationView : UserControl, IChangeWindowCaller
    {
        public IChangeWindowCallerDataContext CustomDataContext { get; set; }
            = new AuthorizationViewModel();

        public AuthorizationView()
        {
            InitializeComponent();
        }
    }
}
