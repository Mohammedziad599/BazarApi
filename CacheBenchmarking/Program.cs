using BenchmarkDotNet.Running;

namespace CacheBenchmarking
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BenchmarkRunner.Run<CatalogBenchmarks>();
            BenchmarkRunner.Run<OrderBenchmarks>();
        }
    }
}