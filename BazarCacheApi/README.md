# Cache Api

Cache api uses the same pattern as the other Api's in this Solution, which is MVC (Model View Controller) but the View
is now used.

this api is running in docker-compose on the port `3000` and it is running on vagrant on `192.168.50.102`

## Functionality

This is used as an inmemory cache using Dictionary as it hold a key string and the value in an object.

## Dependencies

* [Swashbuckle.AspNetCore](https://www.nuget.org/packages/Swashbuckle.AspNetCore/): Swagger tools for api documentation.