using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace OctoWhirl.GUI.Ressources.Styles.Custom
{
    public class CardButton : Button
    {
        public static readonly DependencyProperty CardTitleProperty =
            DependencyProperty.Register("CardTitle", typeof(string), typeof(CardButton), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty CardDescriptionProperty =
            DependencyProperty.Register("CardDescription", typeof(string), typeof(CardButton), new PropertyMetadata(string.Empty));

        public static readonly DependencyProperty IconSourceProperty =
            DependencyProperty.Register("IconSource", typeof(ImageSource), typeof(CardButton), new PropertyMetadata(null));

        public string CardTitle
        {
            get => (string)GetValue(CardTitleProperty);
            set => SetValue(CardTitleProperty, value);
        }

        public string CardDescription
        {
            get => (string)GetValue(CardDescriptionProperty);
            set => SetValue(CardDescriptionProperty, value);
        }

        public ImageSource IconSource
        {
            get => (ImageSource)GetValue(IconSourceProperty);
            set => SetValue(IconSourceProperty, value);
        }
    }
}
