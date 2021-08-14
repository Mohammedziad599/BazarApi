``` ini

BenchmarkDotNet=v0.13.0, OS=fedora 34
Intel Core i7-8550U CPU 1.80GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET SDK=5.0.205
  [Host]     : .NET 5.0.8 (5.0.821.37601), X64 RyuJIT
  Job-RTWLGZ : .NET 5.0.8 (5.0.821.37601), X64 RyuJIT

IterationCount=100  LaunchCount=3  RunStrategy=ColdStart  
WarmupCount=50  

```

|                             Method |         Mean |       Error |      StdDev |       Median |          Min |            Max | Iterations | Rank |
|----------------------------------- |-------------:|------------:|------------:|-------------:|-------------:|---------------:|-----------:|-----:|
|          TestGetAllOrdersFromCache |     974.0 μs |    838.7 μs |  4,371.1 μs |     456.2 μs |     342.3 μs |    44,776.8 μs |      300.0 |    1 |
|          TestGetOrderByIdFromCache |     974.4 μs |    846.3 μs |  4,410.6 μs |     479.2 μs |     363.6 μs |    44,892.7 μs |      300.0 |    1 |
|     TestGetAllOrderFromOrderServer |   2,772.6 μs |    917.4 μs |  4,781.2 μs |   2,261.0 μs |   1,870.2 μs |    51,749.7 μs |      300.0 |    2 |
|    TestGetOrderByIdFromOrderServer |   3,653.9 μs |  2,392.5 μs | 12,469.1 μs |   2,543.5 μs |   2,065.9 μs |   209,138.9 μs |      300.0 |    3 |
| TestPurchaseBookFromOrderWithCache | 674,324.0 μs | 16,630.0 μs | 86,671.1 μs | 651,113.9 μs | 632,588.2 μs | 1,954,112.6 μs |      300.0 |    4 |
