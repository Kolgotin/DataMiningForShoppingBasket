using System.Collections.Generic;

namespace DataMiningForShoppingBasket.Common
{
    public class EqualityProductsComparer : IEqualityComparer<Products>
    {
        public bool Equals(Products x, Products y)
        {
            if (x is null || y is null) return false;
            if (ReferenceEquals(x, y)) return true;
            if (x.GetType() != y.GetType()) return false;
            return x.Id == y.Id;
        }

        public int GetHashCode(Products obj)
        {
            return obj.Id;
        }
    }
}
