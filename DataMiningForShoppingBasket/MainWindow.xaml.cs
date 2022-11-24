using DataMiningForShoppingBasket.Interfaces;
using System.Windows;

namespace DataMiningForShoppingBasket
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(IUserControl userControl)
        {
            InitializeComponent();
            DataContext = new MainViewModel(userControl);
        }
    }
}
