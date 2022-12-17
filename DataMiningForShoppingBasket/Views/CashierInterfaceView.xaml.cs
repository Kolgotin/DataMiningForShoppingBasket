using System.Windows.Controls;
using System.Windows.Data;
using DataMiningForShoppingBasket.Interfaces;
using DataMiningForShoppingBasket.ViewModels;

namespace DataMiningForShoppingBasket.Views
{
    /// <summary>
    /// Логика взаимодействия для UserInterfaceView.xaml
    /// </summary>
    public partial class CashierInterfaceView : IUserControl
    {
        public ILabelHavingDataContext CustomDataContext { get; set; } 
            = new CashierInterfaceViewModel();

        public CashierInterfaceView()
        {
            InitializeComponent();
        }

        private void SearchTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            BindingExpression be = SearchTextBox.GetBindingExpression(TextBox.TextProperty);
            be.UpdateSource();
        }
    }
}
