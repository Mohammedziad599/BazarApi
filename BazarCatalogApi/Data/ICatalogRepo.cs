using System.Collections.Generic;

using BazarCatalogApi.Models;

namespace BazarCatalogApi.Data
{
    public interface ICatalogRepo
    {
        IEnumerable<Book> GetAllBooks();
        Book GetBookById(int id);
        IEnumerable<Book> SearchByTopic(string topic);
        void UpdateBook(Book book);
        bool SaveChanges();
    }
}