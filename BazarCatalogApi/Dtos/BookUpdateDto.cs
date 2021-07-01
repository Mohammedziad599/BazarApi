using System.ComponentModel.DataAnnotations;

namespace BazarCatalogApi.Dtos
{
    public class BookUpdateDto
    {
        [Required]
        public int Quantity { get; set; }

        [Required]
        public double Price { get; set; }
    }
}