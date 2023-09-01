using SearchService.Models;
using SearchService.ServiceContracts;

namespace SearchService.Services
{
    public class SyncServiceCommunicator : ISyncServiceCommunicator
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public SyncServiceCommunicator(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<List<Item>> GetNewestAuctionsAsync(string date)
        {
            string url = _config["AuctionServiceUrl"] + "/api/auctions";

            if (!string.IsNullOrEmpty(date))
            {
                url = url + $"?date={date}";
            }

            return await _httpClient.GetFromJsonAsync<List<Item>>(url);
        }
    }
}
