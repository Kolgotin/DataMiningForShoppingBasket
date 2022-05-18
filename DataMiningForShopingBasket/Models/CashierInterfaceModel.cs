namespace DataMiningForShopingBasket.Models
{
    public class CashierInterfaceModel
    {
        public int Count { get; set; }
        public Products ProductInstance { get; set; }

        public CashierInterfaceModel()
        {
            ProductInstance = new Products();
        }

        public CashierInterfaceModel(int i , string s) : this()
        {
            ProductInstance.id = i;
            ProductInstance.ProductName = s;
        }
    }
}
