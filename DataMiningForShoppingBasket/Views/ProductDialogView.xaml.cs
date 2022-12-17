using DataMiningForShoppingBasket.ViewModels;
using System.Windows;

namespace DataMiningForShoppingBasket.Views
{
    /// <summary>
    /// Interaction logic for ProductDialogView.xaml
    /// </summary>
    public partial class ProductDialogView : Window
    {
        public ProductDialogView(Products product = null)
        {
            DataContext = new ProductDialogViewModel(product);
            InitializeComponent();
        }
    }
}
