using LiveCharts;
using LiveCharts.Wpf;
using MvvmHelpers;
using OctoWhirl.Core.Extensions;
using OctoWhirl.Core.Models.Common;
using OctoWhirl.Core.Models.Enums;
using OctoWhirl.Core.Models.Technicals;
using OctoWhirl.GUI.GUIHelpers;
using OctoWhirl.GUI.ViewModels.Technical;
using OctoWhirl.Services.Data.Loaders;
using OctoWhirl.Services.Models.Requests;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace OctoWhirl.GUI.ViewModels
{
    public class HistoricalViewModel : BaseViewModel
    {
        #region View Properties
        public ObservableCollection<InstrumentItem> Instruments { get; } = new ObservableCollection<InstrumentItem>();

        private string _startDate;
        public string StartDate
        {
            get => _startDate;
            set { _startDate = value; OnPropertyChanged(); }
        }

        private string _endDate;
        public string EndDate
        {
            get => _endDate;
            set { _endDate = value; OnPropertyChanged(); }
        }

        public IEnumerable<ClientSource> AvailableDataSources => Enum.GetValues(typeof(ClientSource)).Cast<ClientSource>();

        private ClientSource _dataSource;
        public ClientSource DataSource
        {
            get => _dataSource;
            set => SetProperty(ref _dataSource, value);
        }

        private SeriesCollection _graphSeries;
        public SeriesCollection GraphSeries
        {
            get => _graphSeries;
            set { _graphSeries = value; OnPropertyChanged(); }
        }

        private string _newInstrumentName;
        public string NewInstrumentName
        {
            get => _newInstrumentName;
            set => SetProperty(ref _newInstrumentName, value);
        }
        #endregion View Properties

        #region Commands
        public ICommand LoadDataCommand { get; }
        public ICommand AddInstrumentCommand { get; }
        #endregion Commands

        #region Services
        private readonly IStatusService _statusService;
        private DataLoader _dataLoader { get; set; }
        #endregion Services

        public HistoricalViewModel(IStatusService statusService, DataLoader dataLoader)
        {
            _statusService = statusService;

            LoadDataCommand = new AsyncRelayCommand(LoadData);
            AddInstrumentCommand = new RelayCommand(AddInstrument, CanAddInstrument);

            DataSource = ClientSource.YahooFinance;
            _dataLoader = dataLoader;

            InitializeInstruments();
        }

        private void InitializeInstruments()
        {
            // À remplacer par ton propre chargement si besoin
            Instruments.Add(new InstrumentItem { Name = "AAPL" });
            Instruments.Add(new InstrumentItem { Name = "GOOG" });
            Instruments.Add(new InstrumentItem { Name = "TSLA" });
        }

        private List<string> GetSelectedInstrumentNames()
        {
            return Instruments.Where(i => i.IsSelected).Select(i => i.Name).ToList();
        }

        #region Adds instruments
        private void AddInstrument()
        {
            if (CanAddInstrument())
            {
                Instruments.Add(new InstrumentItem
                {
                    Name = NewInstrumentName.Trim(),
                    IsSelected = true
                });

                NewInstrumentName = string.Empty;
            }
        }

        private bool CanAddInstrument()
        {
            return !string.IsNullOrWhiteSpace(NewInstrumentName);
        }
        #endregion Adds instruments

        #region Private Data Methods
        private async Task LoadData()
        {
            var selectedInstruments = GetSelectedInstrumentNames();
            try
            {
                _statusService.SetStatus("Loading data...");

                var start = StartDate?.ToDateTime() ?? DateTime.Today.AddMonths(-1);
                var end = EndDate?.ToDateTime() ?? DateTime.Today;

                var series = await LoadHistoricalData(selectedInstruments, start, end).ConfigureAwait(false);

                ThreadingHelper.ExecuteInGUI(() => 
                {
                    GraphSeries = new SeriesCollection(
                        series.Select(kvp => new LineSeries
                        {
                            Title = kvp.Key,
                            Values = new ChartValues<double>(kvp.Value)
                        })
                    );
                });

                _statusService.SetStatus("Data loaded.");
            }
            catch (Exception ex)
            {
                _statusService.SetStatus($"Error : {ex.ToString()}");
            }
        }

        private async Task<Dictionary<string, List<double>>> LoadHistoricalData(List<string> instruments, DateTime startDate, DateTime endDate)
        {
            var request = new GetStocksRequest
            {
                Tickers = instruments.Distinct().ToList(),
                StartDate = startDate,
                EndDate = endDate,
                Interval = ResolutionInterval.Day,
                Source = ClientSource.YahooFinance,
            };

            var candles = await _dataLoader.GetStocks(request).ConfigureAwait(false);
            return FormatCandlesForChart(candles);
        }
        #endregion Private Data Methods

        #region Private Technical Methods
        private Dictionary<string, List<double>> FormatCandlesForChart(List<Candle> candles)
        {
            return candles.GroupBy(c => c.Reference)
                          .ToDictionary(
                              g => g.Key,
                              g => g.OrderBy(c => c.Timestamp).Select(c => c.Close).ToList()
                          );
        }
        #endregion Private Technical Methods
    }
}
