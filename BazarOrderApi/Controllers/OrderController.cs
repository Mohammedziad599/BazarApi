using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

using AutoMapper;

using BazarOrderApi.Data;
using BazarOrderApi.Dto;
using BazarOrderApi.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BazarOrderApi.Controllers
{
    public class OrderController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IOrderRepo _repo;
        private readonly IMapper _mapper;

        public OrderController(IHttpClientFactory clientFactory, IOrderRepo repo, IMapper mapper)
        {
            _clientFactory = clientFactory;
            _repo = repo;
            _mapper = mapper;
        }

        /// <summary>
        /// create an order for a book
        /// </summary>
        /// <remarks>
        /// Sample Request:
        ///
        ///     POST /order/1
        ///     {
        ///         "id": 1,
        ///         "bookId": 1,
        ///         "time": "2021-07-13 00:00:00.00"
        ///     }
        /// </remarks>
        /// <param name="id"></param>
        /// <returns>an order with the book id and a timestamp</returns>
        /// <response code="200">return the order object</response>
        /// <response code="400">
        /// if there is an error in the request either to this endpoint or to the catalog endpoint
        /// </response>
        /// <response code="404">
        /// if the book specified by the id does not exist or if the book is out of stock
        /// </response>
        [HttpPost("/order/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> OrderBook(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/book/" + id);
            request.Headers.Add("Accept", "application/json");
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                var book = await response.Content.ReadFromJsonAsync<Book>();

                if (book?.Quantity > 0)
                {
                    var updateRequest =
                        new HttpRequestMessage(HttpMethod.Post, "http://localhost:5000/book/quantity/dec/" + id)
                        {
                            Content = new StringContent("")
                        };
                    updateRequest.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
                    var updateResponse = await client.SendAsync(updateRequest);
                    if (updateResponse.StatusCode == HttpStatusCode.NoContent)
                    {
                        var order = new Order
                        {
                            BookId = id,
                            Time = DateTime.Now
                        };
                        _repo.AddOrder(order);
                        _repo.SaveChanges();
                        return Ok(_mapper.Map<OrderReadDto>(order));
                    }

                    if (updateResponse.StatusCode == HttpStatusCode.BadRequest)
                    {
                        return Problem("Book is out of Stock",
                            $"http://localhost:5000/book/{id}",
                            404, "Out of Stock Error");
                    }
                }
                else
                {
                    return Problem("Book is out of Stock",
                        $"http://localhost:5000/book/{id}",
                        404, "Out of Stock Error");
                }
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return Problem($"Book with id={id} does not exist",
                    $"http://localhost:5000/book/{id}",
                    404, "Book Does Not Exist Error");
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return BadRequest(response.Content);
            }

            return BadRequest();
        }
    }
}