using System;
using System.ComponentModel.DataAnnotations;

namespace BazarOrderApi.Dto
{
    public class OrderReadDto
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int BookId { get; set; }

        [Required]
        public DateTime Time { get; set; }
    }
}