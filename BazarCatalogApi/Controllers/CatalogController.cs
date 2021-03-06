using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;

using BazarCatalogApi.Data;
using BazarCatalogApi.Dtos;
using BazarCatalogApi.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using JsonSerializer = System.Text.Json.JsonSerializer;

namespace BazarCatalogApi.Controllers
{
    /// <summary>
    ///     This Controller Handle Api Requests for the Catalog Api.
    /// </summary>
    [Produces("application/json")]
    [Route("book")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _hostName;
        private readonly ILogger<CatalogController> _logger;
        private readonly IMapper _mapper;
        private readonly ICatalogRepo _repository;

        public CatalogController(ICatalogRepo repository, IMapper mapper, ILogger<CatalogController> logger,
            IHttpClientFactory clientFactory)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
            _clientFactory = clientFactory;
            InDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
            _hostName = Dns.GetHostName();
        }

        private bool InDocker { get; }

        /// <summary>
        ///     return all the books stored, also it will cache the result on the cache server with id = "books".
        /// </summary>
        /// <remarks>
        ///     Sample request:
        ///     GET /book/
        /// </remarks>
        /// <returns>all the books as a json array</returns>
        /// <response code="200">success books as json array</response>
        /// <response code="404">if there is no books in the store</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllBooks()
        {
            _logger.LogInformation($"{DateTime.Now} -- GET /book/ Requested from {Request.Host.Host}");

            var books = _repository.GetAllBooks();

            var enumerable = books as Book[] ?? books.ToArray();
            if (!enumerable.Any())
            {
                _logger.LogError("There is no book in the database");
                return NotFound();
            }

            var client = _clientFactory.CreateClient();

            _logger.LogInformation($"{DateTime.Now} -- Setting Cache[\"books\"]={enumerable}");
            await client.PostAsJsonAsync($"http://{(InDocker ? "cache" : "192.168.50.102")}/cache/book/array/books",
                enumerable);

            _logger.LogInformation($"{DateTime.Now} -- Result = {JsonSerializer.Serialize(enumerable)}");

            return Ok(_mapper.Map<IEnumerable<BookReadDto>>(books));
        }

        /// <summary>
        ///     returns a specific book, also it will cache the result on the cache server with id = "b-{id}";
        /// </summary>
        /// <remarks>
        ///     Sample request:
        ///     GET /book/1
        /// </remarks>
        /// <param name="id"> the id of the book starting from 1</param>
        /// <returns>book info</returns>
        /// <response code="200">returns the book info</response>
        /// <response code="400">if the id is not an integer</response>
        /// <response code="404">if the book does not exist</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetBookById(int id)
        {
            _logger.LogInformation($"{DateTime.Now} -- GET /book/{id} Requested from {Request.Host.Host}");

            var book = _repository.GetBookById(id);
            if (book == null)
            {
                _logger.LogError($"{DateTime.Now} -- Book with id={id} Not Found");
                return NotFound();
            }

            var client = _clientFactory.CreateClient();

            _logger.LogInformation($"{DateTime.Now} -- Setting Cache[\"b-{id}\"]={book}");
            await client.PostAsJsonAsync($"http://{(InDocker ? "cache" : "192.168.50.102")}/cache/book/b-{book.Id}",
                book);

            _logger.LogInformation($"{DateTime.Now} -- Result = {JsonSerializer.Serialize(book)}");

            return Ok(_mapper.Map<BookReadDto>(book));
        }

        /// <summary>
        ///     search for the books using a topic, also it will cache the result on the cache server
        /// with id = "s-topic-{topic}".
        /// </summary>
        /// <remarks>
        ///     Sample request:
        ///     GET /book/search/dist
        /// </remarks>
        /// <param name="topic">a part or the whole topic to be searched</param>
        /// <returns>an array of books</returns>
        /// <response code="200">returns an array of books info</response>
        /// <response code="400">if the topic is not an specified</response>
        /// <response code="404">if there is no books with matching topic</response>
        [HttpGet("topic/search/{topic}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SearchForBookByTopic(string topic)
        {
            _logger.LogInformation($"{DateTime.Now} -- GET /book/topic/search/{topic} From {Request.Host.Host}");

            var books = _repository.SearchByTopic(topic);
            if (books == null)
            {
                _logger.LogError($"{DateTime.Now} -- No Book found with topic containing \"{topic}\"");
                return NotFound();
            }

            var client = _clientFactory.CreateClient();

            var enumerable = books as Book[] ?? books.ToArray();
            _logger.LogInformation($"{DateTime.Now} -- Setting Cache[\"s-topic-{topic}\"]={enumerable}");
            await client.PostAsJsonAsync(
                $"http://{(InDocker ? "cache" : "192.168.50.102")}/cache/book/array/s-topic-{topic}",
                enumerable);

            _logger.LogInformation($"{DateTime.Now} -- Result = {JsonSerializer.Serialize(enumerable)}");

            return Ok(_mapper.Map<IEnumerable<BookReadDto>>(books));
        }

        /// <summary>
        ///     search for the books using a name, also it will cache the result on the cache server
        /// with id = "s-name-{name}".
        /// </summary>
        /// <remarks>
        ///     Sample request:
        ///     GET /book/name/search/DOS
        /// </remarks>
        /// <param name="name">a part or the whole name to be searched</param>
        /// <returns>a book</returns>
        /// <response code="200">returns a book info</response>
        /// <response code="400">if the name is not an specified</response>
        /// <response code="404">if there is no book with matching name</response>
        [HttpGet("name/search/{name}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> SearchForBookByName(string name)
        {
            _logger.LogInformation($"{DateTime.Now} -- GET /book/name/search/{name} From {Request.Host.Host}");

            var books = _repository.SearchByName(name);
            if (books == null)
            {
                _logger.LogError($"{DateTime.Now} -- No Book found with name containing \"{name}\"");
                return NotFound();
            }

            var client = _clientFactory.CreateClient();

            var enumerable = books as Book[] ?? books.ToArray();
            _logger.LogInformation($"{DateTime.Now} -- Setting Cache[\"s-name-{name}\"]={enumerable}");
            await client.PostAsJsonAsync(
                $"http://{(InDocker ? "cache" : "192.168.50.102")}/cache/book/array/s-name-{name}",
                enumerable);

            _logger.LogInformation($"{DateTime.Now} -- Result = {JsonSerializer.Serialize(enumerable)}");

            return Ok(_mapper.Map<IEnumerable<BookReadDto>>(books));
        }

        /// <summary>
        ///     the same as the other update but without calling other services.
        /// </summary>
        /// <param name="id">the id of the book</param>
        /// <param name="patchDocument">the json patch document</param>
        /// <returns>nothing</returns>
        [HttpPatch("update/patch/{id}")]
        public IActionResult PartialUpdate(int id, JsonPatchDocument<BookUpdateDto> patchDocument)
        {
            _logger.LogInformation($"{DateTime.Now} -- PATCH /book/update/{id} From {Request.Host.Host}");

            var bookFromRepo = _repository.GetBookById(id);
            if (bookFromRepo == null)
            {
                _logger.LogError($"{DateTime.Now} -- Book with id={id} Not Found");
                return NotFound();
            }

            var bookToPatch = _mapper.Map<BookUpdateDto>(bookFromRepo);
            patchDocument.ApplyTo(bookToPatch, ModelState);
            if (!TryValidateModel(bookToPatch))
            {
                _logger.LogError($"{DateTime.Now} -- There is an error in the Received Json Patch");
                return ValidationProblem(ModelState);
            }

            _mapper.Map(bookToPatch, bookFromRepo);
            _repository.UpdateBook(bookFromRepo);
            _repository.SaveChanges();

            return NoContent();
        }

        /// <summary>
        ///     update the book partially
        ///     only the Price and Quantity are updatable.
        /// </summary>
        /// <remarks>
        ///     Sample request:
        ///     PATCH /book/update/1
        ///     [
        ///     {
        ///     "op": "replace",
        ///     "path": "/price",
        ///     "value": "40.0"
        ///     }
        ///     ]
        /// </remarks>
        /// <param name="id">the id of the book starting from 1</param>
        /// <param name="patchDocument">the patch document as specified by the json patch</param>
        /// <returns>nothing</returns>
        /// <response code="204">return Nothing if the operation has Succeed</response>
        /// <response code="400">if there is an error in the request or in the Json Patch Syntax</response>
        /// <response code="404">if the book specified by the id does not exist</response>
        /// <response code="405">if the id was not specified</response>
        [HttpPatch("update/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        public async Task<IActionResult> UpdateBookPartially(int id, JsonPatchDocument<BookUpdateDto> patchDocument)
        {
            _logger.LogInformation($"{DateTime.Now} -- PATCH /book/update/{id} From {Request.Host.Host}");

            var bookFromRepo = _repository.GetBookById(id);
            if (bookFromRepo == null)
            {
                _logger.LogError($"{DateTime.Now} -- Book with id={id} Not Found");
                return NotFound();
            }

            var bookToPatch = _mapper.Map<BookUpdateDto>(bookFromRepo);
            patchDocument.ApplyTo(bookToPatch, ModelState);
            if (!TryValidateModel(bookToPatch))
            {
                _logger.LogError($"{DateTime.Now} -- There is an error in the Received Json Patch");
                return ValidationProblem(ModelState);
            }

            _mapper.Map(bookToPatch, bookFromRepo);
            _repository.UpdateBook(bookFromRepo);
            _repository.SaveChanges();


            var client = _clientFactory.CreateClient();
            _logger.LogInformation($"{DateTime.Now} -- Sending book update to the other replica");
            await client.PatchAsync(
                $"http://{(InDocker ? _hostName == "catalog" ? "catalog_replica" : "catalog" : _hostName == "catalog" ? "192.168.50.200" : "192.18.50.100")}/book/update/patch/{id}",
                new StringContent(JsonConvert.SerializeObject(patchDocument), Encoding.UTF8,
                    "application/json-patch+json"));

            await client.PostAsync($"http://{(InDocker ? "cache" : "192.168.50.102")}/cache/invalidateSearches",
                new StringContent(""));

            await client.PostAsync(
                $"http://{(InDocker ? "cache" : "192.168.50.102")}/cache/invalidate/b-{id}"
                , new StringContent(""));

            await client.PostAsync(
                $"http://{(InDocker ? "cache" : "192.168.50.102")}/cache/invalidate/books"
                , new StringContent(""));

            _logger.LogInformation($"{DateTime.Now} -- Result = {{}}");

            return NoContent();
        }

        /// <summary>
        ///     this method will decrement book quantity without the http calls to the other services.
        /// </summary>
        /// <param name="id">the id of the book</param>
        /// <returns>nothing</returns>
        [HttpPost("dec/{id}")]
        public IActionResult DecBook(int id)
        {
            if (_repository.GetBookById(id) == null) return NotFound();

            _repository.DecreaseBookQuantity(id);

            return NoContent();
        }

        /// <summary>
        ///     decrement the book quantity by 1.
        /// </summary>
        /// <remarks>
        ///     Sample request:
        ///     POST /book/quantity/dec/1
        /// </remarks>
        /// >
        /// <param name="id">the id of the book starting from 1</param>
        /// <returns>nothing</returns>
        /// <response code="204">return nothing if the operation has Succeed</response>
        /// <response code="400">if there is an error in the request or the quantity equal to zero</response>
        /// <response code="404">if the book specified by the id does not exist</response>
        /// <response code="405">if the id was not specified</response>
        [HttpPost("quantity/dec/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        public async Task<IActionResult> DecrementBookQuantity(int id)
        {
            _logger.LogInformation($"{DateTime.Now} -- POST /book/quantity/dec/{id} From {Request.Host.Host}");

            if (_repository.GetBookById(id) == null) return NotFound();

            try
            {
                _repository.DecreaseBookQuantity(id);
            }
            catch (InvalidOperationException)
            {
                _logger.LogError($"{DateTime.Now} -- Item {id} is out of Stock");

                return Problem("Item is Out of Stock",
                    $"/book/{id}",
                    400, $"Cannot Purchase Book with id={id}");
            }

            var client = _clientFactory.CreateClient();

            _logger.LogInformation($"{DateTime.Now} -- Sending book decrement to the other replica");

            await client.PostAsync(
                $"http://{(InDocker ? _hostName == "catalog" ? "catalog_replica" : "catalog" : _hostName == "catalog" ? "192.168.50.200" : "192.18.50.100")}/book/dec/{id}",
                new StringContent(""));
            await client.PostAsync($"http://{(InDocker ? "cache" : "192.168.50.102")}/cache/invalidateSearches",
                new StringContent(""));

            await client.PostAsync(
                $"http://{(InDocker ? "cache" : "192.168.50.102")}/cache/invalidate/b-{id}"
                , new StringContent(""));

            await client.PostAsync(
                $"http://{(InDocker ? "cache" : "192.168.50.102")}/cache/invalidate/books"
                , new StringContent(""));

            _logger.LogInformation($"{DateTime.Now} -- Result = {{}}");

            return NoContent();
        }

        /// <summary>
        ///     this method increment book quantity without the http calls to the other services.
        /// </summary>
        /// <param name="id">the id of the book</param>
        /// <returns>nothing</returns>
        [HttpPost("inc/{id}")]
        public IActionResult IncBook(int id)
        {
            if (_repository.GetBookById(id) == null) return NotFound();

            _repository.IncreaseBookQuantity(id);

            return NoContent();
        }

        /// <summary>
        ///     increment the book quantity by 1.
        /// </summary>
        /// <remarks>
        ///     Sample request:
        ///     POST /book/quantity/inc/1
        /// </remarks>
        /// >
        /// <param name="id">the id of the book starting from 1</param>
        /// <returns>nothing</returns>
        /// <response code="204">return nothing if the operation has Succeed</response>
        /// <response code="400">if there is an error in the request</response>
        /// <response code="404">if the book specified by the id does not exist</response>
        /// <response code="405">if the id was not specified</response>
        [HttpPost("quantity/inc/{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status405MethodNotAllowed)]
        public async Task<IActionResult> IncrementBookQuantity(int id)
        {
            _logger.LogInformation($"{DateTime.Now} -- POST /book/quantity/inc/{id} From {Request.Host.Host}");

            if (_repository.GetBookById(id) == null) return NotFound();

            _repository.IncreaseBookQuantity(id);

            var client = _clientFactory.CreateClient();
            _logger.LogInformation($"{DateTime.Now} -- Sending book increment to the other replica");
            await client.PostAsync(
                $"http://{(InDocker ? _hostName == "catalog" ? "catalog_replica" : "catalog" : _hostName == "catalog" ? "192.168.50.200" : "192.18.50.100")}/book/inc/{id}",
                new StringContent(""));
            await client.PostAsync($"http://{(InDocker ? "cache" : "192.168.50.102")}/cache/invalidateSearches",
                new StringContent(""));

            await client.PostAsync(
                $"http://{(InDocker ? "cache" : "192.168.50.102")}/cache/invalidate/b-{id}"
                , new StringContent(""));

            await client.PostAsync(
                $"http://{(InDocker ? "cache" : "192.168.50.102")}/cache/invalidate/books"
                , new StringContent(""));

            _logger.LogInformation($"{DateTime.Now} -- Result = {{}}");

            return NoContent();
        }
    }
}