using SearchService.Models;
using SearchService.ServiceContracts;

namespace SearchService.Services
{
    public class AuctionsService : IAuctionsService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public AuctionsService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<List<Item>> GetAuctions(string date)
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
