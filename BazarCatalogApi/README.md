# Catalog Api

catalog api uses the MVC ( Model View Controller ) pattern to implement the API Functionality, but the View of this
pattern is not used because this will only returns json data.

To see how this api produces log see {RootFolder}/.log/Catalog

## Api Architecture

The api has been developed using MVC pattern and using the repository pattern for abstracting the database operation
from our controller, also we have used DTO ( Data Transfer Objects)
for the external Data for public use.

![Api Architecture](../.images/Application%20Architecture.png "Our Api Architecture")

## Benchmarking

this is to see how much this api takes time compared by the cache to see how much the cache has helped

``` ini

BenchmarkDotNet=v0.13.0, OS=fedora 34
Intel Core i7-8550U CPU 1.80GHz (Kaby Lake R), 1 CPU, 8 logical and 4 physical cores
.NET SDK=5.0.205
  [Host]     : .NET 5.0.8 (5.0.821.37601), X64 RyuJIT
  Job-RTWLGZ : .NET 5.0.8 (5.0.821.37601), X64 RyuJIT

IterationCount=100  LaunchCount=3  RunStrategy=ColdStart  
WarmupCount=50  

```

|                                 Method |         Mean |       Error |       StdDev |       Median |          Min |            Max | Iterations | Rank |
|--------------------------------------- |-------------:|------------:|-------------:|-------------:|-------------:|---------------:|-----------:|-----:|
|                   TestGetByIdFromCache |     978.9 μs |    744.6 μs |   3,880.8 μs |     546.6 μs |     390.8 μs |    41,189.1 μs |      300.0 |    1 |
|                    TestGetAllFromCache |     987.7 μs |    726.8 μs |   3,788.1 μs |     573.7 μs |     412.3 μs |    38,892.6 μs |      300.0 |    1 |
|                TestSearchNameFromCache |     995.8 μs |    758.2 μs |   3,951.7 μs |     569.6 μs |     392.9 μs |    43,701.2 μs |      300.0 |    1 |
|               TestSearchTopicFromCache |   1,226.3 μs |    770.9 μs |   4,017.7 μs |     648.2 μs |     347.2 μs |    41,961.0 μs |      300.0 |    2 |
|      TestUpdateQuantityWithoutRequests |   2,169.9 μs |    802.4 μs |   4,182.1 μs |   1,717.4 μs |   1,341.9 μs |    45,969.5 μs |      300.0 |    3 |
|            TestGetAllFromCatalogServer |   2,925.9 μs |    741.3 μs |   3,863.4 μs |   2,492.6 μs |   2,083.8 μs |    42,610.4 μs |      300.0 |    4 |
|       TestSearchTopicFromCatalogServer |   3,351.1 μs |    971.2 μs |   5,061.9 μs |   2,637.6 μs |   2,212.9 μs |    70,167.6 μs |      300.0 |    5 |
|        TestSearchNameFromCatalogServer |   3,526.1 μs |    874.5 μs |   4,557.7 μs |   2,822.8 μs |   2,205.6 μs |    51,803.3 μs |      300.0 |    6 |
|           TestGetByIdFromCatalogServer |   3,891.8 μs |  2,267.5 μs |  11,817.6 μs |   2,557.9 μs |   2,035.3 μs |   176,867.6 μs |      300.0 |    7 |
|         TestUpdateQuantityWithRequests |   7,598.6 μs | 10,587.1 μs |  55,177.5 μs |   3,996.8 μs |   3,031.8 μs |   958,380.3 μs |      300.0 |    8 |
|   TestDecrementQuantityWithoutRequests | 112,948.7 μs | 15,558.7 μs |  81,087.8 μs | 157,704.5 μs |   2,242.9 μs |   328,641.3 μs |      300.0 |    9 |
|   TestIncrementQuantityWithoutRequests | 176,155.7 μs | 11,400.8 μs |  59,418.1 μs | 160,465.2 μs | 148,624.8 μs |   652,437.6 μs |      300.0 |   10 |
| TestDecrementQuantityWithRequestsCache | 317,200.0 μs | 40,344.6 μs | 210,265.9 μs | 202,666.2 μs | 155,659.0 μs | 1,519,365.1 μs |      300.0 |   11 |
|      TestIncrementQuantityWithRequests | 344,644.0 μs | 11,684.8 μs |  60,898.1 μs | 324,523.8 μs | 310,852.4 μs |   879,262.9 μs |      300.0 |   12 |

the benchmark has been run with a warmup of 50 operation and a iteration Count of 100 * 3, the warmup does not count
with the result.

From the table we can see that the get book from the cache is faster by 1-2 ms also we can see how much it takes to
decrement the book quantity using the endpoint which we have for it two end point one that invalidate everything related
to that object in the cache and sending to the replica and the other without those request and we can see that there is
about 60ms delay also we cannot make those methods faster that they are because we are making an atomic operation on the
database to make sure we never go below zero.

## Dependencies

* [AutoMapper.Extensions.Microsoft.DependencyInjection](https://www.nuget.org/packages/AutoMapper.Extensions.Microsoft.DependencyInjection/ "AutoMapper Nuget Package"):
  Used to Map Between the internal Models and the DTO.
* [Microsoft.AspNetCore.Cors](https://www.nuget.org/packages/Microsoft.AspNetCore.Cors/): Used to allow public usage of
  our Api see
  this [MDN Page](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Access-Control-Allow-Origin "Access-Control-Allow-Origin")
  .
* [Microsoft.AspNetCore.JsonPatch](https://www.nuget.org/packages/Microsoft.AspNetCore.JsonPatch/5.0.8): this is added
  to add support for the PATCH HTTP Method using the syntax
  of [JsonPatch](https://docs.microsoft.com/en-us/aspnet/core/web-api/jsonpatch?view=aspnetcore-5.0#resource-example "Json Patch Example")
  .
* [Microsoft.AspNetCore.Mvc.NewtonsoftJson](https://www.nuget.org/packages/Microsoft.AspNetCore.Mvc.NewtonsoftJson/5.0.8):
  Needed for JsonPatch.
* [Microsoft.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/5.0.8): ORM (Object
  Relational Mapper).
* [Microsoft.EntityFrameworkCore.Design](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design/5.0.8):
  Shared design-time components for Entity Framework Core tools.
* [Microsoft.EntityFrameworkCore.Sqlite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite/5.0.8): the
  Sqlite Mapper for EFCore.
* [Swashbuckle.AspNetCore](https://www.nuget.org/packages/Swashbuckle.AspNetCore/): Swagger tools for api documentation.