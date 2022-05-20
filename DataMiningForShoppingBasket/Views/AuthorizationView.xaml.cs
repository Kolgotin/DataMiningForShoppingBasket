using DataMiningForShoppingBasket.Interfaces;
using DataMiningForShoppingBasket.ViewModels;

namespace DataMiningForShoppingBasket.Views
{
    /// <summary>
    /// Логика взаимодействия для AuthorizationView.xaml
    /// </summary>
    public partial class AuthorizationView : IChangeWindowCaller
    {
        public IChangeWindowCallerDataContext CustomDataContext { get; set; }
            = new AuthorizationViewModel();

        public AuthorizationView()
        {
            InitializeComponent();
        }
    }
}
