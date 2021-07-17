using System;
using System.Collections.Generic;
using System.Linq;

using BazarCatalogApi.Models;

using Microsoft.EntityFrameworkCore;

namespace BazarCatalogApi.Data
{
    /// <summary>
    ///     this is the real implementation of the ICatalogRepo
    ///     this uses the CatalogContext which is a DbContext to connect to the database
    ///     and then implement the Abstraction, also this Class will be used in the
    ///     Dependency Injection System to be injected to the any class that need the
    ///     implementation of the ICatalogRepo.
    /// </summary>
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

        public void DecreaseBookQuantity(int id)
        {
            var rowCount = _context.Database.ExecuteSqlInterpolated(
                $"UPDATE Books SET Quantity= Quantity - 1 WHERE Id={id} and Quantity > 0");
            if (rowCount == 0) throw new InvalidOperationException($"Book with id={id} is out of stock.");
        }

        public void IncreaseBookQuantity(int id)
        {
            var rowCount = _context.Database.ExecuteSqlInterpolated(
                $"UPDATE Books SET Quantity = Quantity + 1 WHERE Id={id}");
            if (rowCount == 0) throw new InvalidOperationException($"Book with id={id} is out of stock.");
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges() >= 0;
        }
    }
}