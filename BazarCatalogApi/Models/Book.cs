using System.ComponentModel.DataAnnotations;

namespace BazarCatalogApi.Models
{
    /// <summary>
    ///     this is the book model and it is used in the DbContext of the Entity Framework
    ///     to create the mapping between the database and the object which is called ORM (Object Relational Mapping)
    /// </summary>
    public class Book
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Topic { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public double Price { get; set; }
    }
}