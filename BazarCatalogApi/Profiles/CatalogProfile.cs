using AutoMapper;

using BazarCatalogApi.Dtos;
using BazarCatalogApi.Models;

namespace BazarCatalogApi.Profiles
{
    public class CatalogProfile : Profile
    {
        public CatalogProfile()
        {
            CreateMap<Book, BookReadDto>();
            CreateMap<Book, BookUpdateDto>();
            CreateMap<BookReadDto, Book>();
        }
    }
}