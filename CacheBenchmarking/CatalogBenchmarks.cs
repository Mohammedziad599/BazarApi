using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Order;

using CacheBenchmarking.Models;

using Microsoft.AspNetCore.JsonPatch;

using Newtonsoft.Json;

namespace CacheBenchmarking
{
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [SimpleJob(RunStrategy.ColdStart, 100)]
    [MinColumn]
    [MaxColumn]
    [MeanColumn]
    [MedianColumn]
    [IterationsColumn]
    [RankColumn]
    public class CatalogBenchmarks
    {
        private static readonly HttpClient Client = new();
        private static readonly JsonPatchDocument<BookUpdateDto> PatchDocument = new();
        private readonly StringContent _requestContent;

        public CatalogBenchmarks()
        {
            PatchDocument.Replace(book => book.Quantity, 200);
            PatchDocument.Replace(book => book.Price, 50.9);
            var serializedDoc = JsonConvert.SerializeObject(PatchDocument);
            _requestContent = new StringContent(serializedDoc, Encoding.UTF8, "application/json-patch+json");
        }

        [Benchmark]
        public async Task TestGetByIdWithoutCache()
        {
            await Client.GetAsync("http://localhost:5000/book/1");
        }

        [Benchmark]
        public async Task TestGetByIdWithCache()
        {
            await Client.GetAsync("http://localhost:3000/cache/b-1");
        }

        [Benchmark]
        public async Task TestGetAllWithoutCache()
        {
            await Client.GetAsync("http://localhost:5000/book");
        }

        [Benchmark]
        public async Task TestGetAllWithCache()
        {
            await Client.GetAsync("http://localhost:3000/cache/books");
        }

        [Benchmark]
        public async Task TestSearchTopicWithoutCache()
        {
            await Client.GetAsync("http://localhost:5000/book/topic/search/dist");
        }

        [Benchmark]
        public async Task TestSearchTopicWithCache()
        {
            await Client.GetAsync("http://localhost:3000/cache/s-topic-dist");
        }

        [Benchmark]
        public async Task TestSearchNameWithoutCache()
        {
            await Client.GetAsync("http://localhost:5000/book/name/search/for");
        }

        [Benchmark]
        public async Task TestSearchNameWithCache()
        {
            await Client.GetAsync("http://localhost:3000/cache/s-name-for");
        }

        [Benchmark]
        public async Task TestUpdateQuantityWithoutCache()
        {
            await Client.PatchAsync("http://localhost:5000/book/update/1", _requestContent);
        }

        [Benchmark]
        public async Task TestDecrementQuantityWithoutCache()
        {
            await Client.PostAsync("http://localhost:5000/book/quantity/dec/1", new StringContent(""));
        }

        [Benchmark]
        public async Task TestIncrementQuantityWithoutCache()
        {
            await Client.PostAsync("http://localhost:5000/book/quantity/inc/1", new StringContent(""));
        }
    }
}