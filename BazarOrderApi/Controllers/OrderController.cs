using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BazarOrderApi.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BazarOrderApi.Controllers
{
    public class OrderController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;

        public OrderController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpPost("/order/{id}")]
        public async Task<IActionResult> Orderbook(int id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:5000/book/" + id);
            request.Headers.Add("Accept", "application/json");
            var client = _clientFactory.CreateClient();
            var response = await client.SendAsync(request);

            if(response.StatusCode == HttpStatusCode.OK)
            {
                var book = await response.Content.ReadFromJsonAsync<Book>();

                if (book?.Quantity > 0)
                {
                    var newQuantity = book.Quantity - 1;
                    var updateRequest =
                        new HttpRequestMessage(HttpMethod.Patch, "http://localhost:5000/book/update/" + id);
                    var patchDocument = new JsonPatchDocument();
                    patchDocument.Replace("/quantity", "" + newQuantity);
                    updateRequest.Content = new StringContent(JsonConvert.SerializeObject(patchDocument));
                    updateRequest.Content.Headers.ContentType = new MediaTypeWithQualityHeaderValue("application/json");
                    var updateResponse = await client.SendAsync(updateRequest);
                    if (updateResponse.StatusCode == HttpStatusCode.NoContent)
                    {
                        return Ok();
                    }
                }
                else
                {
                    return NotFound("{\"status\":\"Book Out Of Stock\"}");
                }
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return NotFound(response.Content);
            }
            else if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                return BadRequest(response.Content);
            }

            return BadRequest();
        }
    }
}