using System.ComponentModel.DataAnnotations;

namespace BazarCatalogApi.Dtos
{
    /// <summary>
    ///     This is the Dto that will be used to receive the update from it so
    ///     the user is only allowed to change those field's using our api.
    /// </summary>
    public class BookUpdateDto
    {
        [Required]
        public int Quantity { get; set; }

        [Required]
        public double Price { get; set; }
    }
}