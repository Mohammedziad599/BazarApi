using BazarCatalogApi.Data;

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
    }
}