using SearchService.Models;

namespace SearchService.ServiceContracts
{
    /// <summary>
    /// Service for sync communication between Search and Auction services.
    /// </summary>
    public interface ISyncServiceCommunicator
    {
        /// <summary>
        /// Methof for reading all auctions updated after the passed date.
        /// </summary>
        /// <param name="date">Start date of updating of the auctions.</param>
        /// <returns>Auctions items filtered by updating date.</returns>
        Task<List<Item>> GetNewestAuctionsAsync(string date);
    }
}
