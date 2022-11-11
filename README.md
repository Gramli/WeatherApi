# IN PROGRESS

# WeatherApi
REST API solution demonstrates how to create clean and modern API (from my point of view) with Clean Architecture, minimal API and various of design patterns.  

Example API allows to get actual/forecast/favorite weather data by location and safe favorites locations to in memory db.

## Motivation
Main motivation is to write practical example of minimal API, to see it's benefits and disadvantages. Also to create REST API template project using Clean Architecture and design patterns.
## Architecture

Projects folows **[Clean Architecture](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures#clean-architecture)**, but application layer is splitted to Core and Domain projects where Core project holds business rules and Domain project contains business entities.

As Minimal API allows to inject handlers into endpoint map methods, I decided to do not use **[MediatR](https://github.com/jbogard/MediatR)**, but still every endpoint has its own request and handler. Solution folows **[CQRS pattern](https://learn.microsoft.com/en-us/azure/architecture/patterns/cqrs)**, it means that handlers are separated by commands and queries, command handlers handle command requests and query handlers handle query requests.

Instead of throwing exceptions, project use **[Result pattern](https://www.forevolve.com/en/articles/2018/03/19/operation-result/)** (using [FluentResuls package](https://github.com/altmann/FluentResults)) and for returning exact http response, every handler returns data wraped into HttpDataResponse object which contains also error messages collection and http response code.

Important part of every project are **Tests**. In ideal world test coverage should be 100%, but in most commercial projects (by my experience) we are trying to find **optimal test coverage**. Optimal test coverage can't be simply quantified and every project has diferent number by it's need.
In this solution, each 'code' project has its own unit test project and every **unit test** project copy the same directory structure as 'code' project. API projects also has its own **system test** project, because we want to test our endpoints completely. Infrastructure project on the other hand has **integration test** project where we want to test Weatherbit connection for example.

### Minimal API
#### Pros
- **Reduce the ceremony of creating APIs**
	- no controllers (but you are still able to organize map methods in files)
	- injects bussines handlers directly into endpoints map methods
- **Minimal Hosting Model**
	- you are able to create single clean start point of the API
#### Cons
- **Complex Query Parameters**
	- does not support complex query parameters, you have to write [Custom Binding](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis?view=aspnetcore-6.0#custom-binding), anyway [ASP.NET Core updates in .NET 7](https://devblogs.microsoft.com/dotnet/asp-net-core-updates-in-dotnet-7-preview-5/) contains binding support using **[AsParameters]** attribute.
### Benefits of Clean Architecture
- **UI/Framework/Database Independent** 
	- easily change framework/database without touching internal layers
	- UI is top layer so you are able to change it without touching any internal layer
- **Highly Testable** - clean architechture is designed for testing, so you can fox example easily test business logic without touching any external element like database, UI, external web service etc.

### Clean Architecture Layers

Solution contains four layers: 
* **WeatherAPI** - entry point of the application, top layer
	*  Endpoints
	*  Middlewares (or Filters)
	*  API Configuration
* **Weather.Infrastructure** - layer for communication with external resoucers like database, cache, web service.. 
	*  Repositories Implementation - access to database
	*  External Services Proxies - proxy classes implementation to obtain data from external web services
	*  Infastructure Specific Services - services which are needed to interact with infrastructure
* **Weather.Core** - business logic implementatin of the application
	*  Request Handlers/Managers/.. - business implementation
	*  Interfaces - interfaces for Infrastructure layer (Service, Repository, ..)
* **Weather.Domain** - POCO classes, extensions, all what should be shared in all projects in solution
	* DTOs
	* General Extensions

#### Horizontal Diagram (references)
![Project Clean Architecture Diagram](./doc/img/cleanArchitecture.jpg)

## Setup & Run
..

## Conclusion
..



