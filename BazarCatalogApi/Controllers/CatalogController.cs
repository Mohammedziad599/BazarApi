using System.Collections;
using System.Collections.Generic;

using AutoMapper;

using BazarCatalogApi.Data;
using BazarCatalogApi.Dtos;
using BazarCatalogApi.Models;

using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BazarCatalogApi.Controllers
{
    [Route("/book")]
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

        [HttpGet("{id}")]
        public IActionResult GetBookById(int id)
        {
            var book = _repository.GetBookById(id);
            if (book == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<BookReadDto>(book));
        }

        [HttpGet("search/{topic}")]
        public IActionResult SearchForBookByTopic(string topic)
        {
            var books = _repository.SearchByTopic(topic);
            if (books == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<IEnumerable<BookReadDto>>(books));
        }

        [HttpPatch("update/{id}")]
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
    }
}