using System;
using System.Collections.Generic;

using BazarCatalogApi.Models;

namespace BazarCatalogApi.Data
{
    /// <summary>
    ///     This is a Mock Repository used in the first testing of the Api.
    /// </summary>
    public class MockCatalogRepo : ICatalogRepo
    {
        public IEnumerable<Book> GetAllBooks()
        {
            return SearchByTopic("");
        }

        public Book GetBookById(int id)
        {
            return new()
            {
                Id = id,
                Name = "Distributed OS",
                Topic = "Distributed OS",
                Quantity = 20,
                Price = 60.0
            };
        }

        public IEnumerable<Book> SearchByTopic(string topic)
        {
            var books = new List<Book>();
            books.Add(new Book()
            {
                Id = 0,
                Name = "Distributed OS",
                Topic = "Distributed OS",
                Quantity = 20,
                Price = 60.0
            });
            books.Add(new Book()
            {
                Id = 0,
                Name = "Distributed OS",
                Topic = "Distributed OS",
                Quantity = 20,
                Price = 60.0
            });
            books.Add(new Book()
            {
                Id = 0,
                Name = "Distributed OS",
                Topic = "Distributed OS",
                Quantity = 20,
                Price = 60.0
            });
            books.Add(new Book()
            {
                Id = 0,
                Name = "Distributed OS",
                Topic = "Distributed OS",
                Quantity = 20,
                Price = 60.0
            });
            books.Add(new Book()
            {
                Id = 0,
                Name = "Distributed OS",
                Topic = "Distributed OS",
                Quantity = 20,
                Price = 60.0
            });
            return books;
        }

        public IEnumerable<Book> SearchByName(string name)
        {
            var books = new List<Book>();
            books.Add(new Book()
            {
                Id = 0,
                Name = "Distributed OS",
                Topic = "Distributed OS",
                Quantity = 20,
                Price = 60.0
            });
            books.Add(new Book()
            {
                Id = 0,
                Name = "Distributed OS",
                Topic = "Distributed OS",
                Quantity = 20,
                Price = 60.0
            });
            books.Add(new Book()
            {
                Id = 0,
                Name = "Distributed OS",
                Topic = "Distributed OS",
                Quantity = 20,
                Price = 60.0
            });
            books.Add(new Book()
            {
                Id = 0,
                Name = "Distributed OS",
                Topic = "Distributed OS",
                Quantity = 20,
                Price = 60.0
            });
            books.Add(new Book()
            {
                Id = 0,
                Name = "Distributed OS",
                Topic = "Distributed OS",
                Quantity = 20,
                Price = 60.0
            });
            return books;
        }

        public void UpdateBook(Book book)
        {
        }

        public void DecreaseBookQuantity(int id)
        {
            throw new NotImplementedException();
        }

        public void IncreaseBookQuantity(int id)
        {
            throw new NotImplementedException();
        }

        public bool SaveChanges()
        {
            return true;
        }
    }
}