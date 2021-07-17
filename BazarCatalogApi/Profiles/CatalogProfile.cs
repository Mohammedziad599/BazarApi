using AutoMapper;

using BazarCatalogApi.Dtos;
using BazarCatalogApi.Models;

namespace BazarCatalogApi.Profiles
{
    /// <summary>
    ///     this class is used by the AutoMapper and inside we create the mapping that
    ///     we want to apply and AutoMapper is smart enough to figure out how to map between the two models.
    /// </summary>
    public class CatalogProfile : Profile
    {
        public CatalogProfile()
        {
            CreateMap<Book, BookReadDto>();
            CreateMap<Book, BookUpdateDto>();
            CreateMap<BookUpdateDto, Book>();
        }
    }
}