``` ini

BenchmarkDotNet=v0.13.0, OS=fedora 34
Intel Core i7-8550U CPU 1.80GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET SDK=5.0.205
  [Host]     : .NET 5.0.8 (5.0.821.37601), X64 RyuJIT
  Job-ABPSTG : .NET 5.0.8 (5.0.821.37601), X64 RyuJIT

IterationCount=100  LaunchCount=3  RunStrategy=ColdStart  
WarmupCount=1  

```

|                            Method |       Mean |      Error |    StdDev |      Median |         Min |         Max | Iterations | Rank |
|---------------------------------- |-----------:|-----------:|----------:|------------:|------------:|------------:|-----------:|-----:|
|               TestGetAllWithCache |   1.267 ms |  0.9244 ms |  4.818 ms |   0.7187 ms |   0.4264 ms |    51.38 ms |      300.0 |    1 |
|           TestSearchNameWithCache |   1.276 ms |  0.9247 ms |  4.819 ms |   0.7394 ms |   0.4723 ms |    53.70 ms |      300.0 |    1 |
|              TestGetByIdWithCache |   1.295 ms |  0.9608 ms |  5.007 ms |   0.7396 ms |   0.4620 ms |    52.95 ms |      300.0 |    1 |
|          TestSearchTopicWithCache |   1.310 ms |  0.9774 ms |  5.094 ms |   0.7418 ms |   0.4730 ms |    56.39 ms |      300.0 |    1 |
|           TestGetByIdWithoutCache |   2.654 ms |  1.0126 ms |  5.278 ms |   1.9657 ms |   1.4562 ms |    60.27 ms |      300.0 |    2 |
|        TestSearchNameWithoutCache |   2.694 ms |  1.0229 ms |  5.331 ms |   2.0235 ms |   1.5641 ms |    57.18 ms |      300.0 |    2 |
|            TestGetAllWithoutCache |   2.911 ms |  2.0920 ms | 10.903 ms |   1.8469 ms |   1.4553 ms |   178.65 ms |      300.0 |    3 |
|       TestSearchTopicWithoutCache |   3.126 ms |  2.1753 ms | 11.337 ms |   2.0261 ms |   1.5483 ms |   185.19 ms |      300.0 |    4 |
|    TestUpdateQuantityWithoutCache |   8.159 ms | 12.0102 ms | 62.594 ms |   3.8590 ms |   3.0299 ms | 1,085.96 ms |      300.0 |    5 |
| TestDecrementQuantityWithoutCache | 120.052 ms | 17.7702 ms | 92.614 ms | 155.0550 ms |   2.2098 ms |   578.28 ms |      300.0 |    6 |
| TestIncrementQuantityWithoutCache | 165.137 ms |  5.4194 ms | 28.244 ms | 156.7205 ms | 149.5393 ms |   392.23 ms |      300.0 |    7 |
