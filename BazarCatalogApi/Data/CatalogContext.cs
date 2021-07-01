using BazarCatalogApi.Models;

using Microsoft.EntityFrameworkCore;

namespace BazarCatalogApi.Data
{
    public class CatalogContext : DbContext
    {
        public CatalogContext(DbContextOptions<CatalogContext> options): base(options)
        {
            
        }

        public DbSet<Book> Books { get; set; }
    }
}