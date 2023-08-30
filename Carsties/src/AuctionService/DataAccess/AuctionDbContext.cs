using AuctionService.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuctionService.DataAccess
{
    public class AuctionDbContext : DbContext 
    {
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<Item> Items { get; set; }

        public AuctionDbContext(DbContextOptions options)
            : base (options)
        {

        }
    }
}
