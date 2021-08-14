# Order Api

order api uses the MVC ( Model View Controller ) pattern to implement the API Functionality, but the View of this
pattern is not used because this will only returns json data.

To see how this api produces log see {RootFolder}/.log/Order

## Api Architecture

The api has been developed using MVC pattern and using the repository pattern for abstracting the database operation
from our controller, also we have used DTO ( Data Transfer Objects)
for the external Data for public use.

![Api Architecture](../.images/Application%20Architecture.png "Our Api Architecture")

another thing about this api is that it call the catalog api to check for the quantity of the book to see if the book is
available in stock.

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

|                             Method |         Mean |       Error |      StdDev |       Median |          Min |            Max | Iterations | Rank |
|----------------------------------- |-------------:|------------:|------------:|-------------:|-------------:|---------------:|-----------:|-----:|
|          TestGetAllOrdersFromCache |     974.0 μs |    838.7 μs |  4,371.1 μs |     456.2 μs |     342.3 μs |    44,776.8 μs |      300.0 |    1 |
|          TestGetOrderByIdFromCache |     974.4 μs |    846.3 μs |  4,410.6 μs |     479.2 μs |     363.6 μs |    44,892.7 μs |      300.0 |    1 |
|     TestGetAllOrderFromOrderServer |   2,772.6 μs |    917.4 μs |  4,781.2 μs |   2,261.0 μs |   1,870.2 μs |    51,749.7 μs |      300.0 |    2 |
|    TestGetOrderByIdFromOrderServer |   3,653.9 μs |  2,392.5 μs | 12,469.1 μs |   2,543.5 μs |   2,065.9 μs |   209,138.9 μs |      300.0 |    3 |
| TestPurchaseBookFromOrderWithCache | 674,324.0 μs | 16,630.0 μs | 86,671.1 μs | 651,113.9 μs | 632,588.2 μs | 1,954,112.6 μs |      300.0 |    4 |

the benchmark has been run with a warmup of 50 operation and a iteration Count of 100 * 3, the warmup does not count
with the result.

also in this benchmark we can see that the cache is 2x time faster than using this api but for the purchase of a book
the cache seem to not add a value because when someone buys a book it will decrement it's quantity count and because of
this the catalog api will send invalidation requests to the cache which will make anyone that need the book to get it
from the catalog because it is invalid in the cache, also the purcahse request will send another request to the replica
which will also take time which has made this method time so big.

## Dependencies

* [AutoMapper.Extensions.Microsoft.DependencyInjection](https://www.nuget.org/packages/AutoMapper.Extensions.Microsoft.DependencyInjection/ "AutoMapper Nuget Package"):
  Used to Map Between the internal Models and the DTO.
* [Microsoft.AspNetCore.Cors](https://www.nuget.org/packages/Microsoft.AspNetCore.Cors/): Used to allow public usage of
  our Api see
  this [MDN Page](https://developer.mozilla.org/en-US/docs/Web/HTTP/Headers/Access-Control-Allow-Origin "Access-Control-Allow-Origin")
  .
* [Microsoft.EntityFrameworkCore](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore/5.0.8): ORM (Object
  Relational Mapper).
* [Microsoft.EntityFrameworkCore.Design](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Design/5.0.8):
  Shared design-time components for Entity Framework Core tools.
* [Microsoft.EntityFrameworkCore.Sqlite](https://www.nuget.org/packages/Microsoft.EntityFrameworkCore.Sqlite/5.0.8): the
  Sqlite Mapper for EFCore.
* [Swashbuckle.AspNetCore](https://www.nuget.org/packages/Swashbuckle.AspNetCore/): Swagger tools for api documentation.