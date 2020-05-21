using DataMiningForShopingBasket.Events;
using DataMiningForShopingBasket.ViewModels;
using System.Windows.Controls;

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
