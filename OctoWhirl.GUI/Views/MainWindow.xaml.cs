using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using OctoWhirl.GUI.ViewModels;

namespace OctoWhirl.GUI.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainViewModel mainViewModel)
        {
            InitializeComponent();
            DataContext = mainViewModel;
        }
    }
}