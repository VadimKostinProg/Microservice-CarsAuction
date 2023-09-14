using SearchService.Models;
using SearchService.ServiceContracts;
using System.Text.Json;

namespace SearchService.Services
{
    public class SyncServiceCommunicator : ISyncServiceCommunicator
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly ILogger<SyncServiceCommunicator> _logger;

        public SyncServiceCommunicator(HttpClient httpClient, IConfiguration config, 
            ILogger<SyncServiceCommunicator> logger)
        {
            _httpClient = httpClient;
            _config = config;
            _logger = logger;
        }

        public async Task<List<Item>> GetNewestAuctionsAsync(string date)
        {
            string url = _config["AuctionServiceUrl"] + "/api/auctions?date=" + date;

            try
            {
                var list = await _httpClient.GetFromJsonAsync<List<Item>>(url);

                return list;
            }
            catch(Exception ex)
            {
                _logger.LogInformation("--> Error in sync communication: " + ex.Message);

                return new List<Item>();
            }
        }
    }
}
