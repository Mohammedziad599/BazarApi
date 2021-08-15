using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BazarCacheApi.Models
{
    public class Cache
    {
        public Book Book { get; set; }

        public IEnumerable<Book> Books { get; set; }

        public Order Order { get; set; }

        public IEnumerable<Order> Orders { get; set; }

        [Required]
        public DateTime Time { get; set; }
    }
}