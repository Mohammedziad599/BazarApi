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
    }
}