﻿using DataMiningForShoppingBasket.Interfaces;
using DataMiningForShoppingBasket.ViewModels;

namespace DataMiningForShoppingBasket.Views
{
    /// <summary>
    /// Логика взаимодействия для ManagerInterfaceView.xaml
    /// </summary>
    public partial class ManagerInterfaceView : IUserControl
    {
        public ILabelHavingDataContext CustomDataContext { get; set; }
            = new ManagerInterfaceViewModel();

        public ManagerInterfaceView()
        {
            InitializeComponent();
        }
    }
}
