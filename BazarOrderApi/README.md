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