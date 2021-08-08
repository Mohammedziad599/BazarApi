using System;
using System.Collections.Generic;

using BazarCacheApi.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BazarCacheApi.Controllers
{
    [Route("/cache")]
    [ApiController]
    [Produces("application/json")]
    public class CacheController : ControllerBase
    {
        private readonly IDictionary<string, Cache> _dictionary;
        private readonly ILogger<CacheController> _logger;

        public CacheController(Dictionary<string, Cache> dictionary, ILogger<CacheController> logger)
        {
            _dictionary = dictionary;
            _logger = logger;
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
                return Ok(_dictionary[key].Book == null ? _dictionary[key].Books : _dictionary[key].Book);
            }

            return NotFound();
        }

        [HttpPost("{key}")]
        public IActionResult SetCache(string key, [FromBody] Book book)
        {
            if (ModelState.IsValid)
            {
                _dictionary[key] = new Cache
                {
                    Book = book,
                    Books = null,
                    Time = DateTime.Now.AddMinutes(30)
                };
                return Ok();
            }

            return BadRequest();
        }

        [HttpPost("array/{key}")]
        public IActionResult SetCache(string key, [FromBody] IEnumerable<Book> books)
        {
            _logger.LogInformation("Model State : " + ModelState.IsValid);
            if (ModelState.IsValid)
            {
                _dictionary[key] = new Cache
                {
                    Book = null,
                    Books = books,
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

        [HttpPost("invalidateSearches")]
        public IActionResult InvalidateSearchesCaches()
        {
            foreach (var dictionaryKey in _dictionary.Keys)
                if (dictionaryKey.Contains("s-topic"))
                    _dictionary.Remove(dictionaryKey);
                else if (dictionaryKey.Contains("s-name")) _dictionary.Remove(dictionaryKey);

            return Ok();
        }
    }
}