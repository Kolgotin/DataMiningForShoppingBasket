using System;

namespace DataMiningForShoppingBasket.CommonClasses
{
    public class MyException : Exception
    {
        public MyException(string message) : base(message)
        {
        }
    }
}
