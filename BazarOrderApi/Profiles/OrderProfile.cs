using AutoMapper;

using BazarOrderApi.Dto;
using BazarOrderApi.Models;

namespace BazarOrderApi.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<Order, OrderReadDto>();
        }
    }
}