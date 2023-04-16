using DynamicData.Binding;

namespace DataMiningForShoppingBasket.ViewModels
{
    public class AdditionalOfferViewModel : AbstractNotifyPropertyChanged
    {
        private decimal _confidence;
        public Products Product { get; }

        public decimal Confidence
        {
            get => _confidence;
            set => SetAndRaise(ref _confidence, value);
        }

        public AdditionalOfferViewModel(Products product, decimal confidence)
        {
            Product = product;
            Confidence = confidence;
        }
    }
}
