using OctoWhirl.GUI.Ressources.Styles.Custom;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OctoWhirl.GUI.Views
{
    /// <summary>
    /// Logique d'interaction pour WelcomeView.xaml
    /// </summary>
    public partial class WelcomeView : UserControl
    {
        public WelcomeView()
        {
            InitializeComponent();
        }

        private void Card_Click(object sender, RoutedEventArgs e)
        {
            if (sender is CardButton element && element.CardTitle is string cardTitle)
            {
                UserControl? targetView = cardTitle switch
                {
                    "Trading" => new TradingView(),
                    "Risk Analysis" => new RiskView(),
                    "Market Overview" => new MarketView(),
                    "BackTest" => new StrategyView(),
                    "Scripting" => new ScriptingView(),
                    "Settings" => new SettingsView(),
                    _ => null
                };

                if (targetView != null)
                {
                    var mainWindow = Window.GetWindow(this) as MainWindow;
                    if (mainWindow != null)
                    {
                        mainWindow.MainContent.Content = targetView;
                    }
                }
            }
        }

    }
}
