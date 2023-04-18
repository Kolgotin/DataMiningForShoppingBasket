using DataMiningForShoppingBasket.ViewModels;
using System.Windows;

namespace DataMiningForShoppingBasket.Views
{
    /// <summary>
    /// Interaction logic for FocusProductDialogView.xaml
    /// </summary>
    public partial class FocusProductDialogView : Window
    {
        public FocusProductDialogView(FocusProducts focusProduct = null)
        {
            DataContext = new FocusProductDialogViewModel(focusProduct);
            InitializeComponent();
        }
    }
}
