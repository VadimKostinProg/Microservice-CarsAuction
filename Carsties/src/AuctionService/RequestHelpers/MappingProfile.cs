using AuctionService.DTO;
using AuctionService.Entities;
using AutoMapper;

namespace AuctionService.RequestHelpers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Auction, AuctionResponse>()
                .IncludeMembers(x => x.Item);
            CreateMap<Item, AuctionResponse>();
            CreateMap<AuctionAddRequest, Auction>()
                .ForMember(d => d.Item, o => o.MapFrom(s => s));
            CreateMap<AuctionAddRequest, Item>();
        }
    }
}
