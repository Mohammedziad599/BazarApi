using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

using AutoMapper;

using BazarOrderApi.Data;
using BazarOrderApi.Dto;
using BazarOrderApi.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace BazarOrderApi.Controllers
{
    /// <summary>
    ///     This Controller Handle Api Requests for the Order Api.
    /// </summary>
    [Produces("application/json")]
    [Route("/purchase")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly string _hostName;
        private readonly ILogger<OrderController> _logger;
        private readonly IMapper _mapper;
        private readonly IOrderRepo _repo;

        // private readonly bool _useCacheInPurchase = true;

        public OrderController(IHttpClientFactory clientFactory, IOrderRepo repo, IMapper mapper,
            ILogger<OrderController> logger)
        {
            _clientFactory = clientFactory;
            _repo = repo;
            _mapper = mapper;
            _logger = logger;
            InDocker = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
            _hostName = Dns.GetHostName();
        }

        private bool InDocker { get; }

        /// <summary>
        ///     return all the orders stored, also it cache the result on the cache server by id = "orders".
        /// </summary>
        /// <remarks>
        ///     Sample request:
        ///     GET /purchase/list
        ///     Output:
        ///     [
        ///     {
        ///     "id": 1,
        ///     "bookId": 1,
        ///     "time": "2021-07-13 00:00:00.00"
        ///     }
        ///     ]
        /// </remarks>
        /// <returns>all the orders as a json array</returns>
        /// <response code="200">success orders as json array</response>
        /// <response code="404">if there is no orders</response>
        [HttpGet("list")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetAllOrders()
        {
            _logger.LogInformation($"{DateTime.Now} -- GET /purchase/list Requested from {Request.Host.Host}");

            var orders = _repo.GetAllOrders();
            if (orders == null)
            {
                _logger.LogError($"{DateTime.Now} -- No Order Found");
                return NotFound();
            }

            var client = _clientFactory.CreateClient();

            var enumerable = orders as Order[] ?? orders.ToArray();
            _logger.LogInformation($"{DateTime.Now} -- Setting Cache[\"orders\"]={enumerable}");
            client.PostAsync($"http://{(InDocker ? "cache" : "192.168.50.102")}/cache/order/array/orders",
                new StringContent(JsonSerializer.Serialize(enumerable)));

            _logger.LogInformation($"{DateTime.Now} -- Result = {JsonSerializer.Serialize(enumerable)}");

            return Ok(_mapper.Map<IEnumerable<OrderReadDto>>(orders));
        }

        /// <summary>
        ///     returns a specific order, also it cache the result on the cache server by key = "o-{id}".
        /// </summary>
        /// <remarks>
        ///     Sample request:
        ///     GET /purchase/1
        ///     Output:
        ///     {
        ///     "id": 1,
        ///     "bookId": 1,
        ///     "time": "2021-07-13 00:00:00.00"
        ///     }
        /// </remarks>
        /// <param name="id"> the id of the purchase order starting from 1</param>
        /// <returns>order info</returns>
        /// <response code="200">returns the order info</response>
        /// <response code="404">if the order does not exist</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult GetOrderById(int id)
        {
            _logger.LogInformation($"{DateTime.Now} -- GET /purchase/{id} Requested from {Request.Host.Host}");

            var order = _repo.GetOrderById(id);
            if (order == null)
            {
                _logger.LogError($"{DateTime.Now} -- Order Not Found id={id}");
                return NotFound();
            }

            var client = _clientFactory.CreateClient();

            _logger.LogInformation($"{DateTime.Now} -- Setting Cache[\"o-{order.Id}\"]={order}");
            client.PostAsync($"http://{(InDocker ? "cache" : "192.168.50.102")}/cache/order/o-{order.Id}",
                new StringContent(JsonSerializer.Serialize(order)));

            _logger.LogInformation($"{DateTime.Now} -- Result = {JsonSerializer.Serialize(order)}");

            return Ok(_mapper.Map<OrderReadDto>(order));
        }

        /// <summary>
        ///     this method used just to add the order as is to the database, used in a replication.
        /// </summary>
        /// <param name="id">the id of the order</param>
        /// <param name="orderWriteDto">the order</param>
        /// <returns>the order it self</returns>
        /// <response code="200">success</response>
        [HttpPost("add/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult AddOrder(int id, [FromBody] OrderWriteDto orderWriteDto)
        {
            _logger.LogInformation($"{DateTime.Now} -- POST /purchase/add/{id} Requested from {Request.Host.Host}");
            _repo.AddOrder(_mapper.Map<Order>(orderWriteDto));
            _repo.SaveChanges();
            return Ok(_mapper.Map<OrderReadDto>(orderWriteDto));
        }

        /// <summary>
        ///     create an order for a book, it first see if the cache has the value then the catalog.
        /// </summary>
        /// <remarks>
        ///     Sample Request:
        ///     POST /purchase/1
        ///     Output:
        ///     {
        ///     "id": 1,
        ///     "bookId": 1,
        ///     "time": "2021-07-13 00:00:00.00"
        ///     }
        /// </remarks>
        /// <param name="id">the book id that you want to purchase</param>
        /// <returns>an order with the book id and a timestamp</returns>
        /// <response code="200">return the order object</response>
        /// <response code="400">
        ///     if there is an error in the request either to this endpoint or to the catalog endpoint
        /// </response>
        /// <response code="404">
        ///     if the book specified by the id does not exist or if the book is out of stock
        /// </response>
        [HttpPost("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> OrderBook(int id)
        {
            _logger.LogInformation($"{DateTime.Now} -- POST /purchase/{id} Requested From {Request.Host.Host}");

            var request = new HttpRequestMessage(HttpMethod.Get,
                $"http://{(InDocker ? "cache" : "192.168.50.102")}/cache/b-{id}");
            request.Headers.Add("Accept", "application/json");

            var client = _clientFactory.CreateClient();
            _logger.LogInformation($"{DateTime.Now} -- Sending Request to Cache Server /book/{id}");
            var cacheResponse = await client.SendAsync(request);

            if (cacheResponse.StatusCode == HttpStatusCode.OK)
            {
                _logger.LogInformation($"{DateTime.Now} -- Cache Server Returned Status code 200");
                var book = await cacheResponse.Content.ReadFromJsonAsync<Book>();

                if (book?.Quantity > 0)
                {
                    var updateRequest =
                        new HttpRequestMessage(HttpMethod.Post,
                            $"http://{(InDocker ? "catalog" : "192.168.50.100")}/book/quantity/dec/{id}")
                        {
                            Content = new StringContent("")
                        };
                    updateRequest.Content.Headers.ContentType =
                        new MediaTypeWithQualityHeaderValue("application/json");
                    _logger.LogInformation($"{DateTime.Now} -- Sending Decrement Request to Catalog Server");
                    var updateResponse = await client.SendAsync(updateRequest);
                    if (updateResponse.StatusCode == HttpStatusCode.NoContent)
                    {
                        _logger.LogInformation($"{DateTime.Now} -- Decrement Succeed in the Catalog Server");
                        var order = new Order
                        {
                            BookId = id,
                            Time = DateTime.Now
                        };
                        _repo.AddOrder(order);
                        _repo.SaveChanges();

                        _logger.LogInformation($"{DateTime.Now} -- Sending purchase order to the other replica");
                        await client.PostAsJsonAsync(
                            $"http://{(InDocker ? _hostName == "order" ? "order_replica" : "order" : _hostName == "order" ? "192.168.50.201" : "192.18.50.101")}/purchase/add/{id}",
                            order);

                        await client.PostAsJsonAsync(
                            $"http://{(InDocker ? "cache" : "192.168.50.102")}/cache/invalidate/o-{order.Id}",
                            "");

                        await client.PostAsJsonAsync(
                            $"http://{(InDocker ? "cache" : "192.168.50.102")}/cache/invalidate/orders",
                            "");

                        _logger.LogInformation($"{DateTime.Now} -- Result = {JsonSerializer.Serialize(order)}");
                        return Ok(_mapper.Map<OrderReadDto>(order));
                    }

                    if (updateResponse.StatusCode == HttpStatusCode.BadRequest)
                    {
                        _logger.LogError($"{DateTime.Now} -- Book is out of stock id={id}");
                        return Problem("Book is out of Stock.",
                            $"http://{(InDocker ? "catalog" : "192.168.50.100")}/book/{id}",
                            404, "Out of Stock Error");
                    }
                }
                else
                {
                    _logger.LogError($"{DateTime.Now} -- Book is out of stock id={id}");
                    return Problem("Book is out of Stock.",
                        $"http://{(InDocker ? "catalog" : "192.168.50.100")}/book/{id}",
                        404, "Out of Stock Error");
                }
            }
            else if (cacheResponse.StatusCode == HttpStatusCode.NotFound)
            {
                var catalogRequest = new HttpRequestMessage(HttpMethod.Get,
                    $"http://{(InDocker ? "catalog" : "192.168.50.100")}/book/{id}");

                var catalogResponse = client.Send(catalogRequest);

                if (catalogResponse.StatusCode == HttpStatusCode.OK)
                {
                    _logger.LogInformation($"{DateTime.Now} -- Catalog Server Returned Status code 200");
                    var book = await catalogResponse.Content.ReadFromJsonAsync<Book>();

                    if (book?.Quantity > 0)
                    {
                        var updateRequest =
                            new HttpRequestMessage(HttpMethod.Post,
                                $"http://{(InDocker ? "catalog" : "192.168.50.100")}/book/quantity/dec/{id}")
                            {
                                Content = new StringContent("")
                            };
                        updateRequest.Content.Headers.ContentType =
                            new MediaTypeWithQualityHeaderValue("application/json");
                        _logger.LogInformation($"{DateTime.Now} -- Sending Decrement Request to Catalog Server");
                        var updateResponse = await client.SendAsync(updateRequest);
                        if (updateResponse.StatusCode == HttpStatusCode.NoContent)
                        {
                            _logger.LogInformation($"{DateTime.Now} -- Decrement Succeed in the Catalog Server");
                            var order = new Order
                            {
                                BookId = id,
                                Time = DateTime.Now
                            };
                            _repo.AddOrder(order);
                            _repo.SaveChanges();

                            _logger.LogInformation(
                                $"{DateTime.Now} -- Sending purchase order to the other replica");
                            await client.PostAsJsonAsync(
                                $"http://{(InDocker ? _hostName == "order" ? "order_replica" : "order" : _hostName == "order" ? "192.168.50.201" : "192.18.50.101")}/purchase/add/{id}",
                                order);

                            await client.PostAsJsonAsync(
                                $"http://{(InDocker ? "cache" : "192.168.50.102")}/cache/invalidate/o-{order.Id}",
                                "");

                            await client.PostAsJsonAsync(
                                $"http://{(InDocker ? "cache" : "192.168.50.102")}/cache/invalidate/orders",
                                "");

                            _logger.LogInformation($"{DateTime.Now} -- Result = {JsonSerializer.Serialize(order)}");
                            return Ok(_mapper.Map<OrderReadDto>(order));
                        }

                        if (updateResponse.StatusCode == HttpStatusCode.BadRequest)
                        {
                            _logger.LogError($"{DateTime.Now} -- Book is out of stock id={id}");
                            return Problem("Book is out of Stock.",
                                $"http://{(InDocker ? "catalog" : "192.168.50.100")}/book/{id}",
                                404, "Out of Stock Error");
                        }
                    }
                    else
                    {
                        _logger.LogError($"{DateTime.Now} -- Book is out of stock id={id}");
                        return Problem("Book is out of Stock.",
                            $"http://{(InDocker ? "catalog" : "192.168.50.100")}/book/{id}",
                            404, "Out of Stock Error");
                    }
                }
                else if (catalogResponse.StatusCode == HttpStatusCode.NotFound)
                {
                    _logger.LogError($"{DateTime.Now} -- Book is not found id={id}");
                    return Problem($"Book with id={id} does not exist.",
                        $"http://{(InDocker ? "catalog" : "192.168.50.100")}/book/{id}",
                        404, "Book Does Not Exist Error");
                }
                else if (catalogResponse.StatusCode == HttpStatusCode.BadRequest)
                {
                    _logger.LogError($"{DateTime.Now} -- Bad Request to catalog server " +
                                     $"the request ==> http://{(InDocker ? "catalog" : "192.168.50.100")}/book/{id}");
                    return BadRequest(catalogResponse.Content);
                }
            }

            _logger.LogError($"{DateTime.Now} -- Something went wrong with this request.");
            return BadRequest("Something Wrong Happen Please Check the Request and try Again.");
        }
    }
}