using System.Windows.Controls;
using DataMiningForShoppingBasket.Events;
using DataMiningForShoppingBasket.ViewModels;

namespace DataMiningForShoppingBasket.Views
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
