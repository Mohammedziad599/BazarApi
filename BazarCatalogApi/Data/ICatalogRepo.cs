using System.Collections.Generic;

using BazarCatalogApi.Models;

namespace BazarCatalogApi.Data
{
    public interface ICatalogRepo
    {
        Book GetBookById(int id);
        IEnumerable<Book> SearchByTopic(string topic);
    }
}