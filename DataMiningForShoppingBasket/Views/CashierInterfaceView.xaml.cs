using DataMiningForShoppingBasket.Interfaces;
using DataMiningForShoppingBasket.ViewModels;

namespace DataMiningForShoppingBasket.Views
{
    /// <summary>
    /// Логика взаимодействия для UserInterfaceView.xaml
    /// </summary>
    public partial class CashierInterfaceView : IChangeWindowCaller
    {
        public IChangeWindowCallerDataContext CustomDataContext { get; set; } 
            = new CashierInterfaceViewModel();

        public CashierInterfaceView()
        {
            InitializeComponent();
        }
    }
}
