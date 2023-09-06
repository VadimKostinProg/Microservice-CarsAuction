using Contracts;
using SearchService.Models;
using SearchService.RequestHelpers;

namespace SearchService.ServiceContracts
{
    /// <summary>
    /// Service for managing auctions.
    /// </summary>
    public interface IAuctionsService
    {
        /// <summary>
        /// Method for the searching items due to the passed search conditions.
        /// </summary>
        /// <param name="searchParams">Object with search parametrs.</param>
        /// <returns>Object with result collection, page count and total count of elements.</returns>
        Task<SearchResponse> SearchItemsAsync(SearchParams searchParams);

        /// <summary>
        /// Method for adding new item to the data base.
        /// </summary>
        /// <param name="item">Item to add.</param>
        Task AddNewItemAsync(AuctionCreated item);

        /// <summary>
        /// Method fot updating item in the data base.
        /// </summary>
        /// <param name="item">Item to update.</param>
        Task UpdateItemAsync(AuctionUpdated item);

        /// <summary>
        /// Method for handling placed bid.
        /// </summary>
        /// <param name="bid">Placed bid to handle.</param>
        Task PlaceBidAsync(BidPlaced bid);

        /// <summary>
        /// Method for handling finished auction.
        /// </summary>
        /// <param name="item">Finished auction to handle.</param>
        Task FinishItemAsync(AuctionFinished item);

        /// <summary>
        /// Method for deleting item from the data base.
        /// </summary>
        /// <param name="id">Id of item to delete.</param>
        Task DeleteItemAsync(string id);
    }
}
