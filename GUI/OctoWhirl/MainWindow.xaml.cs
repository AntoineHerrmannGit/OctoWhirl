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
        // Take into account already loaded views and remembers when lost focus
        private Dictionary<string, UserControl> _activeViews;

        public MainWindow()
        {
            InitializeComponent();
            _activeViews = new Dictionary<string, UserControl>() { { "Home", new WelcomeView() } };
            MainContent.Content = _activeViews["Home"];
        }

        #region Private Navigation within views
        private void NavButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Content is string viewName)
            {
                NavigateTo(viewName);
            }
        }

        public void NavigateTo(string viewName)
        {
            switch (viewName)
            {
                case "Home":
                    if (_activeViews.ContainsKey(viewName))
                        MainContent.Content = _activeViews["Home"];
                    else
                        MainContent.Content = new WelcomeView();
                    break;
                case "Risk Analysis":
                    if (_activeViews.ContainsKey(viewName))
                        MainContent.Content = _activeViews["Risk"];
                    else
                        MainContent.Content = new RiskView();
                    break;
                case "Trading":
                    if (_activeViews.ContainsKey(viewName))
                        MainContent.Content = _activeViews["Trading"];
                    else
                        MainContent.Content = new TradingView();
                    break;
                case "Settings":
                    if (_activeViews.ContainsKey(viewName))
                        MainContent.Content = _activeViews["Settings"];
                    else
                        MainContent.Content = new SettingsView();
                    break;
            }
        }
        #endregion Private Navigation within views
    }
}