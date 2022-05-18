namespace DataMiningForShopingBasket.Models
{
    class ManagerInterfaceModel : Discounts
    {
        public Products Product { get; set; }

        public ManagerInterfaceModel(int i, string s)
        {
            id = i;
            Product.ProductName = s;
        }
    }
}
