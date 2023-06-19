using System.Windows.Controls;

namespace DataMiningForShoppingBasket.Views;

/// <summary>
/// Interaction logic for ProductListView.xaml
/// </summary>
public partial class ProductListView
{
    public ProductListView()
    {
        InitializeComponent();
    }

    private void SearchTextBox_OnTextChanged(object sender, TextChangedEventArgs e)
    {
        var bindingExpression = SearchTextBox.GetBindingExpression(TextBox.TextProperty);
        bindingExpression?.UpdateSource();
    }
}