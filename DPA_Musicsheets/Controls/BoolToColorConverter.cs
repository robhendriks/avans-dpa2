using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DPA_Musicsheets.Controls
{
    public class BoolToColorConverter : IValueConverter
    {
        public static readonly Color yes = (Color)ColorConverter.ConvertFromString("#26A65B");
        public static readonly Color no = (Color)ColorConverter.ConvertFromString("#D91E18");

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value ? yes : no);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
