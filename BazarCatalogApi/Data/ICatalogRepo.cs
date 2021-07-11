using System.Collections.Generic;

using BazarCatalogApi.Models;

namespace BazarCatalogApi.Data
{
    public interface ICatalogRepo
    {
        IEnumerable<Book> GetAllBooks();
        Book GetBookById(int id);
        IEnumerable<Book> SearchByTopic(string topic);
        IEnumerable<Book> SearchByName(string name);
        void UpdateBook(Book book);
        bool SaveChanges();
    }
}