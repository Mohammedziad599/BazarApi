using System.ComponentModel.DataAnnotations;

namespace BazarCacheApi.Models
{
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