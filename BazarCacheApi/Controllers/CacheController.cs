using System;
using System.Collections.Generic;

using BazarCacheApi.Models;

using Microsoft.AspNetCore.Mvc;

namespace BazarCacheApi.Controllers
{
    [Route("/cache")]
    [ApiController]
    [Produces("application/json")]
    public class CacheController : ControllerBase
    {
        private readonly IDictionary<string, Cache> _dictionary;

        public CacheController(IDictionary<string, Cache> dictionary)
        {
            _dictionary = dictionary;
        }

        [HttpGet("{key}")]
        public IActionResult GetCache(string key)
        {
            if (_dictionary.ContainsKey(key))
            {
                if (_dictionary[key].Time > DateTime.Now)
                {
                    _dictionary.Remove(key);
                    return NotFound();
                }

                _dictionary[key].Time = _dictionary[key].Time.AddMinutes(5);
                return Ok(_dictionary[key].Book);
            }

            return NotFound();
        }

        [HttpPost("{key}")]
        public IActionResult SetCache(string key, Book book)
        {
            if (ModelState.IsValid)
            {
                _dictionary[key] = new Cache
                {
                    Book = book,
                    Time = DateTime.Now.AddMinutes(30)
                };
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("invalidate/{key}")]
        public IActionResult InvalidateCache(string key)
        {
            if (_dictionary.ContainsKey(key)) _dictionary.Remove(key);

            return Ok();
        }
    }
}