using System;
using System.Collections.Generic;
using System.Linq;

using AutoMapper;

using BazarCatalogApi.Data;
using BazarCatalogApi.Dtos;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BazarCatalogApi.Controllers
{
    [Produces("application/json")]
    [Route("book")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogRepo _repository;
        private readonly IMapper _mapper;

        public CatalogController(ICatalogRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// return all the books stored.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /book/
        /// 
        /// </remarks>
        /// <returns>all the books as a json array</returns>
        /// <response code="200">success books as json array</response>
        /// <response code="404">if there is no books in the store</response>
        [HttpGet("/book/")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAllBooks()
        {
            var books = _repository.GetAllBooks();

            if (!books.Any())
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<BookReadDto>>(books));
        }

        /// <summary>
        /// returns a specific book.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /book/1
        /// 
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
        public IActionResult GetBookById(int id)
        {
            var book = _repository.GetBookById(id);
            if (book == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<BookReadDto>(book));
        }

        /// <summary>
        /// search for the books using a topic.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /book/search/dist
        /// 
        /// </remarks>
        /// <param name="topic">a part or the whole topic to be searched</param>
        /// <returns>an array of books</returns>
        /// <response code="200">returns an array of books info</response>
        /// <response code="400">if the topic is not an specified</response>
        /// <response code="404">if there is no books with matching topic</response>
        [HttpGet("search/{topic}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult SearchForBookByTopic(string topic)
        {
            var books = _repository.SearchByTopic(topic);
            if (books == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<BookReadDto>>(books));
        }
        
        /// <summary>
        /// search for the books using a name.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /book/searchN/dist
        /// 
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
        public IActionResult SearchForBookByName(string name)
        {
            var books = _repository.SearchByName(name);
            if (books == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<BookReadDto>>(books));
        }

        /// <summary>
        /// update the book partially
        /// only the Price and Quantity are updatable.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PATCH /book/update/1
        ///     [
        ///         {
        ///             "op": "replace",
        ///             "path": "/price",
        ///             "value": "40.0"
        ///         }
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
        public IActionResult UpdateBookPartially(int id, JsonPatchDocument<BookUpdateDto> patchDocument)
        {
            var bookFromRepo = _repository.GetBookById(id);
            if (bookFromRepo == null)
            {
                return NotFound();
            }

            var bookToPatch = _mapper.Map<BookUpdateDto>(bookFromRepo);
            patchDocument.ApplyTo(bookToPatch, ModelState);
            if (!TryValidateModel(bookToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(bookToPatch, bookFromRepo);
            _repository.UpdateBook(bookFromRepo);
            _repository.SaveChanges();

            return NoContent();
        }

        /// <summary>
        /// decrement the book quantity by 1.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /book/quantity/dec/1
        ///     
        /// </remarks>>
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
        public IActionResult DecrementBookQuantity(int id)
        {
            if (_repository.GetBookById(id) == null)
            {
                return NotFound();
            }

            try
            {
                _repository.DecreaseBookQuantity(id);
            }
            catch (InvalidOperationException)
            {
                return Problem("Item is Out of Stock",
                    $"/book/{id}",
                    400, $"Cannot Purchase Book with id={id}");
            }

            return NoContent();
        }

        /// <summary>
        /// increment the book quantity by 1.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /book/quantity/inc/1
        ///     
        /// </remarks>>
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
        public IActionResult IncrementBookQuantity(int id)
        {
            if (_repository.GetBookById(id) == null)
            {
                return NotFound();
            }

            _repository.IncreaseBookQuantity(id);

            return NoContent();
        }
    }
}