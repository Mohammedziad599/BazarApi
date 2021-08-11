using System.Net.Http;
using System.Threading.Tasks;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Engines;
using BenchmarkDotNet.Order;

namespace CacheBenchmarking
{
    [Orderer(SummaryOrderPolicy.FastestToSlowest)]
    [SimpleJob(RunStrategy.ColdStart, 3, 1, 100)]
    [MinColumn]
    [MaxColumn]
    [MeanColumn]
    [MedianColumn]
    [IterationsColumn]
    [RankColumn]
    public class OrderBenchmarks
    {
        private static readonly HttpClient Client = new();

        [Benchmark]
        public async Task TestGetOrderByIdWithoutCache()
        {
            await Client.GetAsync("http://localhost:6000/purchase/1");
        }

        [Benchmark]
        public async Task TestGetOrderByIdFromCache()
        {
            await Client.GetAsync("http://localhost:3000/cache/o-1");
        }

        [Benchmark]
        public async Task TestGetAllOrderWithoutCache()
        {
            await Client.GetAsync("http://localhost:6000/purchase/list");
        }

        [Benchmark]
        public async Task TestGetAllOrdersFromCache()
        {
            await Client.GetAsync("http://localhost:3000/cache/orders");
        }

        [Benchmark]
        public async Task TestPurchaseBookFromOrderWithCache()
        {
            await Client.PostAsync("http://localhost:6000/purchase/1", new StringContent(""));
        }
    }
}