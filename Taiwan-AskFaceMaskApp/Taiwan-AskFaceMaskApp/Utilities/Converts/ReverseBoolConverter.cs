using System;
using System.Globalization;
using Xamarin.Forms;

namespace JamestsaiTW.Utilities.Converters
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class ReverseBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is bool)
            {
                return !(bool)value;
            }
            else
            {
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value != null && value is bool)
            {
                return !(bool)value;
            }
            else
            {
                return false;
            }
        }
    }
}
