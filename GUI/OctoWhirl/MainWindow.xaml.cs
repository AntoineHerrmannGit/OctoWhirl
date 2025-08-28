using OctoWhirl.GUI.Views;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OctoWhirl.GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainContent.Content = new WelcomeView(); // Charge la vue au démarrage
        }

        #region Private Navigation within views
        private void NavButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Content is string viewName)
            {
                NavigateTo(viewName);
            }
        }

        private void NavigateTo(string viewName)
        {
            switch (viewName)
            {
                case "Home":
                    MainContent.Content = new WelcomeView();
                    break;
            }
        }
        #endregion Private Navigation within views
    }
}