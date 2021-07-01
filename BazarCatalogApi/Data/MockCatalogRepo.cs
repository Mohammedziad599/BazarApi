using System.Collections.Generic;

using BazarCatalogApi.Models;

namespace BazarCatalogApi.Data
{
    public class MockCatalogRepo : ICatalogRepo
    {
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
            books.Add(new()
            {
                Id = 0,
                Name = "Distributed OS",
                Topic = "Distributed OS",
                Quantity = 20,
                Price = 60.0
            });
            books.Add(new()
            {
                Id = 0,
                Name = "Distributed OS",
                Topic = "Distributed OS",
                Quantity = 20,
                Price = 60.0
            });
            books.Add(new()
            {
                Id = 0,
                Name = "Distributed OS",
                Topic = "Distributed OS",
                Quantity = 20,
                Price = 60.0
            });
            books.Add(new()
            {
                Id = 0,
                Name = "Distributed OS",
                Topic = "Distributed OS",
                Quantity = 20,
                Price = 60.0
            });
            books.Add(new()
            {
                Id = 0,
                Name = "Distributed OS",
                Topic = "Distributed OS",
                Quantity = 20,
                Price = 60.0
            });
            return books;
        }
    }
}