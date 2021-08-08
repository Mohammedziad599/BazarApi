using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BazarCacheApi.Models
{
    public class Cache
    {
        [Required]
        public Book Book { get; set; }

        public IEnumerable<Book> Books { get; set; }

        [Required]
        public DateTime Time { get; set; }
    }
}