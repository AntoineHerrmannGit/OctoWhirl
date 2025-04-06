using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace OctoWhirl.Services.Clients.FinnHubClient
{
    public class FinnHubClient
    {
        private readonly string _apiKey;
        private readonly HttpClient _httpClient;

        public FinnHubClient(string apiKey)
                                        {
            _apiKey = apiKey;
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://finnhub.io/api/v1/")
            };
        }

        // 1. Obtenir les données de l'action (quotes historiques OHLCV)
        public async Task<string> GetStockAsync(string symbol, DateTime startDate, DateTime endDate)
        {
            long from = startDate.ToUnixTimestamp();
            long to = endDate.ToUnixTimestamp();

            string url = $"stock/candle?symbol={symbol}&resolution=D&from={from}&to={to}&token={_apiKey}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        // 2. Obtenir les données d’une option
        public async Task<string> GetOptionAsync(string optionSymbol, DateTime startDate, DateTime endDate)
        {
            long from = startDate.ToUnixTimestamp();
            long to = endDate.ToUnixTimestamp();

            string url = $"options/candle?symbol={optionSymbol}&resolution=D&from={from}&to={to}&token={_apiKey}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }

        // 3. Obtenir la liste des options disponibles pour un sous-jacent à une date
        public async Task<string> GetListedOptionsAsync(string symbol, DateTime date)
        {
            string formattedDate = date.ToString("yyyy-MM-dd");
            string url = $"stock/option-chain?symbol={symbol}&date={formattedDate}&token={_apiKey}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadAsStringAsync();
        }
    }   
}