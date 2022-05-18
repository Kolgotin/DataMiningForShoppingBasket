using System.Windows.Controls;
using DataMiningForShoppingBasket.Events;
using DataMiningForShoppingBasket.ViewModels;

namespace DataMiningForShoppingBasket.Views
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
