using System.Globalization;
using System.Windows;
using System;
using System.Net.Http.Headers;
using System.Windows.Data;

namespace DataMiningForShoppingBasket.Converters
{
    public class BoolToFontWeightConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is true ? FontWeights.Bold : FontWeights.Normal;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) =>
            value is FontWeight f && f == FontWeights.Bold;
    }
}