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

        [HttpPost("/order/{id}")]
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