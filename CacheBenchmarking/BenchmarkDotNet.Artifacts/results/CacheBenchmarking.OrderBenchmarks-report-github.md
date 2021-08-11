``` ini

BenchmarkDotNet=v0.13.0, OS=fedora 34
Intel Core i7-8550U CPU 1.80GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET SDK=5.0.205
  [Host]     : .NET 5.0.8 (5.0.821.37601), X64 RyuJIT
  Job-ABPSTG : .NET 5.0.8 (5.0.821.37601), X64 RyuJIT

IterationCount=100  LaunchCount=3  RunStrategy=ColdStart  
WarmupCount=1  

```

|                             Method |       Mean |     Error |     StdDev |      Median |         Min |         Max | Iterations | Rank |
|----------------------------------- |-----------:|----------:|-----------:|------------:|------------:|------------:|-----------:|-----:|
|          TestGetOrderByIdFromCache |   1.305 ms |  1.037 ms |   5.404 ms |   0.7216 ms |   0.4482 ms |    55.97 ms |      300.0 |    1 |
|          TestGetAllOrdersFromCache |   1.496 ms |  1.284 ms |   6.694 ms |   0.7546 ms |   0.4253 ms |    83.05 ms |      300.0 |    2 |
|        TestGetAllOrderWithoutCache |   3.035 ms |  1.502 ms |   7.829 ms |   1.9527 ms |   1.2740 ms |    88.79 ms |      300.0 |    3 |
|       TestGetOrderByIdWithoutCache |  11.968 ms | 30.521 ms | 159.066 ms |   2.0708 ms |   1.5300 ms | 2,756.45 ms |      300.0 |    3 |
| TestPurchaseBookFromOrderWithCache | 397.104 ms | 24.485 ms | 127.610 ms | 332.7234 ms | 304.0694 ms |   926.88 ms |      300.0 |    4 |
