``` ini

BenchmarkDotNet=v0.13.0, OS=fedora 34
Intel Core i7-8550U CPU 1.80GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET SDK=5.0.205
  [Host]     : .NET 5.0.8 (5.0.821.37601), X64 RyuJIT
  Job-TUACQP : .NET 5.0.8 (5.0.821.37601), X64 RyuJIT

IterationCount=50  RunStrategy=Throughput  

```
|                            Method |       Mean |      Error |     StdDev |     Median | Rank |
|---------------------------------- |-----------:|-----------:|-----------:|-----------:|-----:|
|       TestSearchTopicWithoutCache |   1.304 ms |  0.0367 ms |  0.0724 ms |   1.277 ms |    1 |
|        TestSearchNameWithoutCache |   1.344 ms |  0.0400 ms |  0.0721 ms |   1.346 ms |    2 |
|            TestGetAllWithoutCache |   2.118 ms |  0.5353 ms |  1.0813 ms |   1.244 ms |    2 |
|           TestGetByIdWithoutCache |   2.211 ms |  0.1024 ms |  0.2021 ms |   2.204 ms |    2 |
| TestDecrementQuantityWithoutCache |  55.319 ms | 37.5127 ms | 75.7774 ms |   2.098 ms |    2 |
| TestIncrementQuantityWithoutCache | 159.441 ms |  0.4392 ms |  0.7576 ms | 159.731 ms |    3 |
