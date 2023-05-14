using System;

namespace DataMiningForShoppingBasket.Common
{
    public static class MethodExtensions
    {
        public static bool IsActual(this FocusProducts focusProduct)
        {
            return focusProduct.StartDate.ToUniversalTime() <= DateTime.UtcNow
                   && DateTime.UtcNow <= focusProduct.FinishDate.ToUniversalTime();
        }
    }
}
