using System;
using System.ComponentModel.DataAnnotations;

namespace BazarCacheApi.Models
{
    public class Cache
    {
        [Required]
        public Book Book { get; set; }

        [Required]
        public DateTime Time { get; set; }
    }
}