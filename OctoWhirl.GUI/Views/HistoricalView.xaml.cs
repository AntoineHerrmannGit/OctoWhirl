using System.Windows.Controls;
using OctoWhirl.GUI.ViewModels;

namespace OctoWhirl.GUI.Views
{
    /// <summary>
    /// Logique d'interaction pour HistoricalView.xaml
    /// </summary>
    public partial class HistoricalView : UserControl
    {
        public HistoricalView(HistoricalViewModel historicalViewModel)
        {
            InitializeComponent();
            DataContext = historicalViewModel;
        }
    }
}
