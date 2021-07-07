using System;
using System.ComponentModel.DataAnnotations;

namespace BazarOrderApi.Models
{
    public class Order
    {
        [Required] public int Id { get; set; }
        [Required] public int BookId { get; set; }
        [Required] public DateTime Time { get; set; }
    }
}