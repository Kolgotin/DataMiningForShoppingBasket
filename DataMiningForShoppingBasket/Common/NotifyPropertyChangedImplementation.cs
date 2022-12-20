using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DataMiningForShoppingBasket.Common
{
    public abstract class NotifyPropertyChangedImplementation : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        public void RaisePropertyChanged(string prop)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        protected void SetProperty<T>(ref T backingField, T newValue, [CallerMemberName] string propertyName = null)
        {
            var comparer = EqualityComparer<T>.Default;
            if (comparer.Equals(backingField, newValue))
            {
                return;
            }

            backingField = newValue;
            RaisePropertyChanged(propertyName);
        }
    }
}
