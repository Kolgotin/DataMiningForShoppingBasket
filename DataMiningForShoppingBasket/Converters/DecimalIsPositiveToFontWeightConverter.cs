using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace DataMiningForShoppingBasket.Converters
{
    public class DecimalIsPositiveToFontWeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is decimal and > 0 ? FontWeights.Bold : FontWeights.Normal;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            throw new NotImplementedException();
    }
}
