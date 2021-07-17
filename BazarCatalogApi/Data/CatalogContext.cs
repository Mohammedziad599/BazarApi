using BazarCatalogApi.Models;

using Microsoft.EntityFrameworkCore;

namespace BazarCatalogApi.Data
{
    /// <summary>
    ///     this class implement the DbContext which is a way to let the
    ///     ORM (Object Relational Mapping) in this case it is Entity Framework
    ///     know what we want it to manage.
    /// </summary>
    public class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> options) : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
    }
}