using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

using BazarCacheApi.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BazarCacheApi.Controllers
{
    /// <summary>
    ///     This Controller Handle Api request for the Cache Api
    /// </summary>
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

        /// <summary>
        ///     this method gives the cache value based on the key.
        /// </summary>
        /// <remarks>
        ///     Sample request:
        ///     GET /cache/b-1
        /// </remarks>
        /// <param name="key">the key of the value in the cache</param>
        /// <returns>the value as either array of objects ot an object ot not found if the key does not exist</returns>
        /// <response code="200">success cache as either json array or json object</response>
        /// <response code="404">if the key does not exist ot timed out</response>
        [HttpGet("{key}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
                if (_dictionary[key].Book == null)
                {
                    if (_dictionary[key].Books == null)
                    {
                        if (_dictionary[key].Order == null)
                            return Ok(_dictionary[key].Orders);
                        return Ok(_dictionary[key].Order);
                    }

                    return Ok(_dictionary[key].Books);
                }

                return Ok(_dictionary[key].Book);
            }

            return NotFound();
        }

        /// <summary>
        ///     this method store the book in the cache given the key.
        /// </summary>
        /// <remarks>
        ///     Sample request:
        ///     POST /cache/book/b-1
        /// </remarks>
        /// <param name="key">the key of the value in the cache</param>
        /// <param name="book">the book to be stored</param>
        /// <returns>nothing</returns>
        /// <response code="200">success</response>
        /// <response code="400">if the book is invalid</response>
        [HttpPost("book/{key}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult SetCache(string key, [FromBody] Book book)
        {
            _logger.LogInformation($"{DateTime.Now} -- POST /cache/book/{key} Requested from {Request.Host.Host}");
            if (ModelState.IsValid)
            {
                _logger.LogInformation(
                    $"{DateTime.Now} -- setting Cache[\"{key}\"]={JsonSerializer.Serialize(book)}"
                );
                _dictionary[key] = new Cache
                {
                    Book = book,
                    Books = null,
                    Order = null,
                    Orders = null,
                    Time = DateTime.Now.AddMinutes(30)
                };
                return Ok();
            }

            _logger.LogInformation($"{DateTime.Now} -- Request on /cache/book/{key} is invalid");

            return BadRequest();
        }

        /// <summary>
        ///     this method store the order in the cache given the key.
        /// </summary>
        /// <remarks>
        ///     Sample request:
        ///     POST /cache/order/o-1
        /// </remarks>
        /// <param name="key">the key of the value in the cache</param>
        /// <param name="order">the order to be stored</param>
        /// <returns>nothing</returns>
        /// <response code="200">success</response>
        /// <response code="400">if the order is invalid</response>
        [HttpPost("order/{key}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult SetCache(string key, [FromBody] Order order)
        {
            _logger.LogInformation($"{DateTime.Now} -- POST /cache/order/{key} Requested from {Request.Host.Host}");
            if (ModelState.IsValid)
            {
                _logger.LogInformation(
                    $"{DateTime.Now} -- setting Cache[\"{key}\"]={JsonSerializer.Serialize(order)}"
                );
                _dictionary[key] = new Cache
                {
                    Book = null,
                    Books = null,
                    Order = order,
                    Orders = null,
                    Time = DateTime.Now.AddMinutes(30)
                };
                return Ok();
            }

            _logger.LogInformation($"{DateTime.Now} -- Request on /cache/order/{key} is invalid");

            return BadRequest();
        }

        /// <summary>
        ///     this method store the books as an array in the cache given the key.
        /// </summary>
        /// <remarks>
        ///     Sample request:
        ///     POST /cache/book/array/books
        /// </remarks>
        /// <param name="key">the key of the value in the cache</param>
        /// <param name="books">the books array to be stored</param>
        /// <returns>nothing</returns>
        /// <response code="200">success</response>
        /// <response code="400">if the books is invalid</response>
        [HttpPost("book/array/{key}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult SetCache(string key, [FromBody] IEnumerable<Book> books)
        {
            _logger.LogInformation(
                $"{DateTime.Now} -- POST /cache/book/array/{key} Requested from {Request.Host.Host}"
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
                    Order = null,
                    Orders = null,
                    Time = DateTime.Now.AddMinutes(30)
                };
                return Ok();
            }

            _logger.LogInformation($"{DateTime.Now} -- Request on /cache/book/array/{key} is invalid");

            return BadRequest();
        }

        /// <summary>
        ///     this method store the orders as an array in the cache given the key.
        /// </summary>
        /// <remarks>
        ///     Sample request:
        ///     POST /cache/order/array/orders
        /// </remarks>
        /// <param name="key">the key of the value in the cache</param>
        /// <param name="orders">the orders array to be stored</param>
        /// <returns>nothing</returns>
        /// <response code="200">success</response>
        /// <response code="400">if the orders is invalid</response>
        [HttpPost("order/array/{key}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult SetCache(string key, [FromBody] IEnumerable<Order> orders)
        {
            _logger.LogInformation(
                $"{DateTime.Now} -- POST /cache/order/array/{key} Requested from {Request.Host.Host}"
            );
            if (ModelState.IsValid)
            {
                var enumerable = orders as Order[] ?? orders.ToArray();

                _logger.LogInformation(
                    $"{DateTime.Now} -- setting Cache[\"{key}\"] = {JsonSerializer.Serialize(enumerable)}");
                _dictionary[key] = new Cache
                {
                    Book = null,
                    Books = null,
                    Order = null,
                    Orders = enumerable,
                    Time = DateTime.Now.AddMinutes(30)
                };
                return Ok();
            }

            _logger.LogInformation($"{DateTime.Now} -- Request on /cache/order/array/{key} is invalid");

            return BadRequest();
        }

        /// <summary>
        ///     this method is to remove the value that response to the key from the cache.
        /// </summary>
        /// <remarks>
        ///     Sample request:
        ///     POST /cache/invalidate/orders
        /// </remarks>
        /// <param name="key">the key to be removed</param>
        /// <returns>nothing</returns>
        /// <response code="200">success</response>
        [HttpPost("invalidate/{key}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult InvalidateCache(string key)
        {
            _logger.LogInformation(
                $"{DateTime.Now} -- POST /cache/invalidate/{key} Requested from {Request.Host.Host}"
            );
            if (_dictionary.ContainsKey(key)) _dictionary.Remove(key);

            return Ok();
        }

        /// <summary>
        ///     this method is to remove the searches values in the cache this means all the keys that contains
        ///     s-topic or s-name in it.
        /// </summary>
        /// <remarks>
        ///     Sample request:
        ///     POST /cache/invalidateSearches
        /// </remarks>
        /// <returns>nothing</returns>
        /// <response code="200">success</response>
        [HttpPost("invalidateSearches")]
        [ProducesResponseType(StatusCodes.Status200OK)]
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