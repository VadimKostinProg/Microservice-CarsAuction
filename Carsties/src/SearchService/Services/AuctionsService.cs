using AutoMapper;
using Contracts;
using MongoDB.Driver;
using MongoDB.Entities;
using SearchService.Data;
using SearchService.Models;
using SearchService.RequestHelpers;
using SearchService.ServiceContracts;

namespace SearchService.Services
{
    public class AuctionsService : IAuctionsService
    {
        private readonly AuctionsContext _context;
        private readonly IMapper _mapper;

        public AuctionsService(AuctionsContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task AddNewItemAsync(AuctionCreated auction)
        {
            var item = _mapper.Map<Item>(auction);

            await _context.Items.InsertOneAsync(item);
        }

        public async Task DeleteItemAsync(string id)
        {
            var filter = Builders<Item>.Filter.Eq(x => x.Id, id);
            await _context.Items.DeleteOneAsync(filter);
        }

        public async Task FinishItemAsync(AuctionFinished item)
        {
            var auction = await _context.Items.Find(x => x.Id == item.AuctionId).FirstOrDefaultAsync();

            if (item.ItemSold)
            {
                auction.Winner = item.Winner;
                auction.SoldAmount = item.Amount;
            }

            auction.Status = "Finished";

            var filter = Builders<Item>.Filter.Eq(x => x.Id, auction.Id);
            await _context.Items.ReplaceOneAsync(filter, auction);
        }

        public async Task PlaceBidAsync(BidPlaced bid)
        {
            var item = await _context.Items.Find(x => x.Id == bid.AuctionId).FirstOrDefaultAsync();

            if (bid.BidStatus.Contains("Accepted") && bid.Amount > item.CurrentHighBid)
            {
                item.CurrentHighBid = bid.Amount;

                var filter = Builders<Item>.Filter.Eq(x => x.Id, item.Id);
                await _context.Items.ReplaceOneAsync(filter, item);
            }
        }

        public async Task<SearchResponse> SearchItemsAsync(SearchParams searchParams)
        {
            List<Item> query;

            if (string.IsNullOrEmpty(searchParams.SearchTerm))
            {
                query = await _context.Items.Find(x => true).ToListAsync();
            }
            else
            {
                var filter = Builders<Item>.Filter.Text(searchParams.SearchTerm);
                query = await _context.Items.Find(filter).ToListAsync();
            }

            int totalCount = query.Count;

            query = searchParams.OrderBy switch
            {
                "make" => query.OrderBy(x => x.Make).ToList(),
                "new" => query.OrderByDescending(x => x.CreatedAt).ToList(),
                _ => query.OrderBy(x => x.AuctionEnd).ToList()
            };

            query = searchParams.FilterBy switch
            {
                "finished" => query.Where(x => x.AuctionEnd < DateTime.UtcNow).ToList(),
                "ending" => query.Where(x => x.AuctionEnd > DateTime.UtcNow &&
                x.AuctionEnd < DateTime.UtcNow.AddHours(6)).ToList(),
                _ => query.Where(x => x.AuctionEnd > DateTime.UtcNow).ToList()
            };

            if (!string.IsNullOrEmpty(searchParams.Seller))
            {
                query = query.Where(x => x.Seller == searchParams.Seller).ToList();
            }

            if (!string.IsNullOrEmpty(searchParams.Winner))
            {
                query = query.Where(x => x.Winner == searchParams.Winner).ToList();
            }

            int skip = (searchParams.PageNumber - 1) * searchParams.PageSize;
            var result = query.Skip(skip)
                .Take(searchParams.PageSize)
                .ToList();

            int pageCount = result.Count;

            return new SearchResponse()
            {
                Results = result,
                PageCount = pageCount,
                TotalCount = totalCount
            };
        }

        public async Task UpdateItemAsync(AuctionUpdated auction)
        {
            var item = _context.Items.Find(x => x.Id == auction.Id).FirstOrDefault();

            if (item is null)
                throw new KeyNotFoundException("Item with such Id is not found.");

            item.Make = auction.Make;
            item.Model = auction.Model;
            item.Color = auction.Color;
            item.Mileage = auction.Mileage;
            item.Year = auction.Year;

            var filter = Builders<Item>.Filter.Eq(x => x.Id, item.Id);
            await _context.Items.ReplaceOneAsync(filter, item);
        }
    }
}
