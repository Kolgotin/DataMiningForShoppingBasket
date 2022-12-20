namespace DataMiningForShoppingBasket.Common
{
    public static class CurrentSession
    {
        //todo: проверить является ли это хорошей практикой
        public static Users CurrentUser { get; set; }

        public static void Clear()
        {
            CurrentUser = null;
        }
    }
}
