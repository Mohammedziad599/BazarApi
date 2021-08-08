``` ini

BenchmarkDotNet=v0.13.0, OS=fedora 34
Intel Core i7-8550U CPU 1.80GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET SDK=5.0.205
  [Host]     : .NET 5.0.8 (5.0.821.37601), X64 RyuJIT
  Job-FILVJN : .NET 5.0.8 (5.0.821.37601), X64 RyuJIT

IterationCount=50  RunStrategy=Throughput  

```
|                            Method |         Mean |        Error |       StdDev |       Median | Rank |
|---------------------------------- |-------------:|-------------:|-------------:|-------------:|-----:|
|              TestGetByIdWithCache |     277.3 μs |     12.22 μs |     24.69 μs |     291.8 μs |    1 |
|               TestGetAllWithCache |     292.6 μs |      1.21 μs |      2.41 μs |     292.6 μs |    1 |
|           TestSearchNameWithCache |     295.2 μs |      1.23 μs |      2.34 μs |     295.0 μs |    1 |
|          TestSearchTopicWithCache |     298.4 μs |      2.63 μs |      5.13 μs |     296.9 μs |    1 |
|           TestGetByIdWithoutCache |   2,070.3 μs |     16.30 μs |     32.93 μs |   2,065.0 μs |    2 |
|       TestSearchTopicWithoutCache |   2,201.8 μs |      6.91 μs |     13.95 μs |   2,201.5 μs |    3 |
|            TestGetAllWithoutCache |   2,259.2 μs |     98.79 μs |    199.55 μs |   2,388.2 μs |    4 |
|        TestSearchNameWithoutCache |   2,314.7 μs |      4.91 μs |      9.58 μs |   2,314.7 μs |    5 |
| TestDecrementQuantityWithoutCache |  52,132.5 μs | 36,846.66 μs | 74,432.07 μs |   2,103.8 μs |    6 |
| TestIncrementQuantityWithoutCache | 159,668.7 μs |    445.30 μs |    802.96 μs | 159,844.8 μs |    7 |
