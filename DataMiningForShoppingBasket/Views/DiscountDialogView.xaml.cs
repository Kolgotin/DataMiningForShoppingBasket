using DataMiningForShoppingBasket.ViewModels;
using System.Windows;

namespace DataMiningForShoppingBasket.Views
{
    /// <summary>
    /// Interaction logic for DiscountDialogView.xaml
    /// </summary>
    public partial class DiscountDialogView : Window
    {
        public DiscountDialogView(Discounts discount = null)
        {
            DataContext = new DiscountDialogViewModel(discount);
            InitializeComponent();
        }
    }
}
