using BazarCatalogApi.Data;
using BazarCatalogApi.Models;

using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BazarCatalogApi.Controllers
{
    [Route("/")]
    public class CatalogController : Controller
    {
        private readonly ICatalogRepo _repository;

        public CatalogController(ICatalogRepo repository)
        {
            _repository = repository;
        }

        [HttpGet("book/{id}")]
        public IActionResult GetBookById(int id)
        {
            var bookItem = _repository.GetBookById(id);
            return Ok(bookItem);
        }

        [HttpGet("book/search/{topic}")]
        public IActionResult SearchForBookByTopic(string topic)
        {
            var books = _repository.SearchByTopic(topic);
            return Ok(books);
        }

        [HttpPatch("book/update/{id}")]
        public IActionResult UpdateBookPartially(int id, JsonPatchDocument<Book> patchDocument)
        {
            var book = _repository.GetBookById(id);
            patchDocument.ApplyTo(book, ModelState);
            if (!TryValidateModel(book))
            {
                return ValidationProblem(ModelState);
            }

            _repository.UpdateBook(book);
            _repository.SaveChanges();
            return NoContent();
        }
    }
}