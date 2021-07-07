using System;

namespace BazarOrderApi.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public DateTime Time { get; set; }
    }
}