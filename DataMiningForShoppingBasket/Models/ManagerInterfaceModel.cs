namespace DataMiningForShoppingBasket.Models
{
    class ManagerInterfaceModel : Discounts
    {
        public Products Product { get; set; }

        public ManagerInterfaceModel(int i, string s)
        {
            Id = i;
            Product.ProductName = s;
        }
    }
}
