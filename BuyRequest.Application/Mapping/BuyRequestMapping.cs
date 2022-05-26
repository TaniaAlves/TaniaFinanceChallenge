using AutoMapper;
using BuyRequest.Application.DTOs;

namespace BuyRequest.Application.Mapping
{
    public class BuyRequestMapping : Profile
    {
        public BuyRequestMapping()
        {
            CreateMap<Domain.Entities.BuyRequest, BuyRequestDTO>().ReverseMap();
            CreateMap<Domain.Entities.ProductRequest, ProductRequestDTO>().ReverseMap();
        }
    }
}
