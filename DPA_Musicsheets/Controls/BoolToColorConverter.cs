using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DPA_Musicsheets.Controls
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ((bool)value ? Color.FromArgb(0xFF, 0x00, 0xFF, 0x00) : Color.FromArgb(0xFF, 0xFF, 0x00, 0x00));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
