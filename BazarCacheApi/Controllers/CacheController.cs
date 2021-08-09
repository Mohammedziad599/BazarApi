using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

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
            _logger.LogInformation($"{DateTime.Now} -- GET /cache/{key} Requested from {Request.Host.Host}");
            if (_dictionary.ContainsKey(key))
            {
                if (_dictionary[key].Time < DateTime.Now)
                {
                    _logger.LogInformation($"{DateTime.Now} -- cache value is out of date for Cache[\"{key}\"]");
                    _dictionary.Remove(key);
                    return NotFound();
                }

                _logger.LogInformation($"{DateTime.Now} -- Adding 1 minutes for Cache[\"{key}\"]");
                _dictionary[key].Time = _dictionary[key].Time.AddMinutes(1);
                return Ok(_dictionary[key].Book == null ? _dictionary[key].Books : _dictionary[key].Book);
            }

            return NotFound();
        }

        [HttpPost("{key}")]
        public IActionResult SetCache(string key, [FromBody] Book book)
        {
            _logger.LogInformation($"{DateTime.Now} -- POST /cache/{key} Requested from {Request.Host.Host}");
            if (ModelState.IsValid)
            {
                _logger.LogInformation(
                    $"{DateTime.Now} -- setting Cache[\"{key}\"]={JsonSerializer.Serialize(book)}"
                );
                _dictionary[key] = new Cache
                {
                    Book = book,
                    Books = null,
                    Time = DateTime.Now.AddMinutes(30)
                };
                return Ok();
            }

            _logger.LogInformation($"{DateTime.Now} -- Request on /cache/{key} is invalid");

            return BadRequest();
        }

        [HttpPost("array/{key}")]
        public IActionResult SetCache(string key, [FromBody] IEnumerable<Book> books)
        {
            _logger.LogInformation(
                $"{DateTime.Now} -- POST /cache/array/{key} Requested from {Request.Host.Host}"
            );
            if (ModelState.IsValid)
            {
                var enumerable = books as Book[] ?? books.ToArray();

                _logger.LogInformation(
                    $"{DateTime.Now} -- setting Cache[\"{key}\"] = {JsonSerializer.Serialize(enumerable)}");
                _dictionary[key] = new Cache
                {
                    Book = null,
                    Books = enumerable,
                    Time = DateTime.Now.AddMinutes(30)
                };
                return Ok();
            }

            _logger.LogInformation($"{DateTime.Now} -- Request on /cache/array/{key} is invalid");

            return BadRequest();
        }

        [HttpPost("invalidate/{key}")]
        public IActionResult InvalidateCache(string key)
        {
            _logger.LogInformation(
                $"{DateTime.Now} -- POST /cache/invalidate/{key} Requested from {Request.Host.Host}"
            );
            if (_dictionary.ContainsKey(key)) _dictionary.Remove(key);

            return Ok();
        }

        [HttpPost("invalidateSearches")]
        public IActionResult InvalidateSearchesCaches()
        {
            _logger.LogInformation(
                $"{DateTime.Now} -- POST /cache/invalidateSearches/ Requested from {Request.Host.Host}");
            foreach (var dictionaryKey in _dictionary.Keys)
                if (dictionaryKey.Contains("s-topic"))
                {
                    _logger.LogInformation($"{DateTime.Now} -- Removing Cache[\"{dictionaryKey}\"]");
                    _dictionary.Remove(dictionaryKey);
                }
                else if (dictionaryKey.Contains("s-name"))
                {
                    _logger.LogInformation($"{DateTime.Now} -- Removing Cache[\"{dictionaryKey}\"]");
                    _dictionary.Remove(dictionaryKey);
                }

            return Ok();
        }
    }
}