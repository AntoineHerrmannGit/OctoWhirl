using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System.Windows;

namespace OctoWhirl.GUI.Converters
{
    public class ThemeIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // value = nom de l'icône (ex: "home.png")
            // parameter = optionnel, peut contenir le sous-dossier (ex: "Icons")

            if (value is string iconName)
            {
                // Récupère le thème courant depuis les ressources
                string theme = Application.Current.Resources["CurrentTheme"] as string ?? "DarkTheme";

                // Chemin complet
                string folder = parameter as string ?? "Images";
                string path = $"pack://application:,,,/Ressources/{folder}/{theme}/{iconName}";

                return new BitmapImage(new Uri(path, UriKind.Absolute));
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
