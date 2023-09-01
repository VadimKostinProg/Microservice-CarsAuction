using SearchService.Models;

namespace SearchService.RequestHelpers
{
    public class SearchResponse
    {
        public List<Item> Results { get; set; }
        public int PageCount { get; set; }
        public int TotalCount { get; set; }
    }
}
