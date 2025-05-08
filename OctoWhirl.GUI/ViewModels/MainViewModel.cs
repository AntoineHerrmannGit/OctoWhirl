using System.Collections.ObjectModel;
using System.Windows.Input;
using MvvmHelpers;
using OctoWhirl.GUI.ViewModels.Technical;
using OctoWhirl.GUI.Views;

namespace OctoWhirl.GUI.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        #region Commands
        public ICommand OpenHistoricalViewCommand { get; }
        public ICommand OpenPricingViewCommand { get; }
        public ICommand CloseTabCommand { get; }
        #endregion Commands

        #region Properties
        public ObservableCollection<TabItemViewModel> Tabs { get; } = new ObservableCollection<TabItemViewModel>();

        private TabItemViewModel _selectedTab;
        public TabItemViewModel SelectedTab
        {
            get => _selectedTab;
            set { _selectedTab = value; OnPropertyChanged(); }
        }

        private string _statusMessage;
        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }
        #endregion Properties

        #region Services
        private readonly IStatusService _statusService;
        #endregion Services

        #region Tabs
        private readonly IViewFactory _viewFactory;
        #endregion Tabs

        public MainViewModel(
            IStatusService statusService,
            IViewFactory viewFactory
        )
        {
            _statusService = statusService;
            _statusMessage = "Ready.";

            // Subscribe to status updates
            _statusService.StatusChanged += (_, msg) => StatusMessage = msg;
            _viewFactory = viewFactory;

            // Setup commands
            OpenHistoricalViewCommand = new RelayCommand(OpenHistoricalView);
            OpenPricingViewCommand = new RelayCommand(OpenPricingView);
            CloseTabCommand = new RelayCommand<TabItemViewModel>(CloseTab);
        }

        private void OpenHistoricalView()
        {
            var view = _viewFactory.Create<HistoricalView>();
            AddTab("Historical", view);
        }

        private void OpenPricingView()
        {
            var view = _viewFactory.Create<PricingView>();
            AddTab("Pricing", view);
        }

        private void AddTab(string baseTitle, object viewContent, bool preventDuplicate = true)
        {
            if (preventDuplicate)
            {
                var existing = Tabs.FirstOrDefault(t => t.Content.GetType() == viewContent.GetType());
                if (existing != null)
                {
                    SelectedTab = existing;
                    return;
                }
            }

            var tab = new TabItemViewModel
            {
                Header = $"{baseTitle}",
                Content = viewContent
            };

            Tabs.Add(tab);
            SelectedTab = tab;
        }

        private void CloseTab(TabItemViewModel tab)
        {
            if (Tabs.Contains(tab))
                Tabs.Remove(tab);
        }
    }
}
