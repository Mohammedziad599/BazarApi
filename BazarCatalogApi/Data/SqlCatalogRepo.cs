using System.Collections.Generic;
using System.Linq;

using BazarCatalogApi.Models;

namespace BazarCatalogApi.Data
{
    public class SqlCatalogRepo : ICatalogRepo
    {
        private readonly CatalogContext _context;

        public SqlCatalogRepo(CatalogContext context)
        {
            _context = context;
        }

        public IEnumerable<Book> GetAllBooks()
        {
            return _context.Books.ToList();
        }

        public Book GetBookById(int id)
        {
            return _context.Books.FirstOrDefault(p => p.Id == id);
        }

        public IEnumerable<Book> SearchByTopic(string topic)
        {
            var booksList = (from book in _context.Books
                where book.Topic.Contains(topic)
                select book).ToList();
            return booksList.Count == 0 ? null : booksList;
        }

        public IEnumerable<Book> SearchByName(string name)
        {
            var booksList = (from book in _context.Books
                where book.Name.Contains(name)
                select book).ToList();
            return booksList.Count == 0 ? null : booksList;
        }

        public void UpdateBook(Book book)
        {
            //We Don't need to do anything here.
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}