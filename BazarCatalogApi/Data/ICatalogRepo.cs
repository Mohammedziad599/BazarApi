using System.Collections.Generic;

using BazarCatalogApi.Models;

namespace BazarCatalogApi.Data
{
    /// <summary>
    ///     This interface has the abstract method that the Catalog Repository Needs
    ///     Note That this is Built To Handle all of our Connecting to the Database
    ///     this design patter called the repository pattern.
    /// </summary>
    public interface ICatalogRepo
    {
        IEnumerable<Book> GetAllBooks();
        Book GetBookById(int id);
        IEnumerable<Book> SearchByTopic(string topic);
        IEnumerable<Book> SearchByName(string name);
        void UpdateBook(Book book);
        void DecreaseBookQuantity(int id);
        void IncreaseBookQuantity(int id);
        bool SaveChanges();
    }
}