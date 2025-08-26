using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace OctoWhirl.GUI.Converters
{
    public class PackUriConverter : IValueConverter
    {
        public object? Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string relativePath)
            {
                var uri = new Uri($"pack://application:,,,/{relativePath}", UriKind.Absolute);
                return new BitmapImage(uri);
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
