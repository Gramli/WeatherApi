 <img align="left" width="116" height="116" src=".\doc\img\weatherApi_icon.png" />
 
 # Clean Architecture WeatherApi
[![.NET Build and Test](https://github.com/Gramli/WeatherApi/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Gramli/WeatherApi/actions/workflows/dotnet.yml)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/77a7db482a44489aa5fbe40ca15d3137)](https://www.codacy.com/gh/Gramli/WeatherApi/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=Gramli/WeatherApi&amp;utm_campaign=Badge_Grade)
[![Codacy Badge](https://app.codacy.com/project/badge/Coverage/77a7db482a44489aa5fbe40ca15d3137)](https://www.codacy.com/gh/Gramli/WeatherApi/dashboard?utm_source=github.com&utm_medium=referral&utm_content=Gramli/WeatherApi&utm_campaign=Badge_Coverage)

REST API solution demonstrates how to create clean and modern API (from my point of view) with Clean Architecture, minimal API and various of design patterns.  

Example API allows to get actual/forecast weather data by location from [Weatherbit](https://www.weatherbit.io/) throught [RapidAPI](https://rapidapi.com) and also allow's to add favorite locations into [in memory database](https://learn.microsoft.com/en-us/ef/core/providers/in-memory/?tabs=dotnet-core-cli) and then get weather data by stored (favorites) locations.

## Menu
* [Get Started](#get-started)
* [Motivation](#motivation)
* [Architecture](#architecture)
	* [Minimal API](#minimal-api)
		* [Pros](#pros)
	* [Clean Architecture](#clean-architecture)
  		* [Pros](#pros)
   		* [Cons](#cons)
	* [Clean Architecture Layers](#clean-architecture-layers)
		* [Horizontal Diagram (references)](#horizontal-diagram-references)
* [Technologies](#technologies)


## Get Started
1. Register on [RapidAPI](https://rapidapi.com)
2. Subscribe [Weatherbit](https://rapidapi.com/weatherbit/api/weather) (its for free) and go to Endpoints tab
3. In API documentation copy (from Code Snippet) **X-RapidAPI-Key**, **X-RapidAPI-Host** and put them to appsettings.json file in WeatherAPI project
```json
  "Weatherbit": {
    "BaseUrl": "https://weatherbit-v1-mashape.p.rapidapi.com",
    "XRapidAPIKey": "value from code snippet",
    "XRapidAPIHost": "value from code snippet"
  }
```
4. Run Weather.API 

### Try it in SwaggerUI
![SwaggerUI](./doc/img/weatherApiSwagger.gif)

### Try it using .http file (VS2022)
 * Go to Tests/Debug folder and open **debug-tests.http** file (in VS2022)
 * Send request

## Motivation
Main motivation is to write practical example of minimal API, to see it's benefits and disadvantages. Also to create REST API example project using Clean Architecture and design patterns.
## Architecture

Projects folows **[Clean Architecture](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures#clean-architecture)**, but application layer is splitted to Core and Domain projects where Core project holds business rules and Domain project contains business entities.

As Minimal API allows to inject handlers into endpoint map methods, I decided to do not use **[MediatR](https://github.com/jbogard/MediatR)**, but still every endpoint has its own request and handler. Solution folows **[CQRS pattern](https://learn.microsoft.com/en-us/azure/architecture/patterns/cqrs)**, it means that handlers are separated by commands and queries, command handlers handle command requests and query handlers handle query requests. Also repositories (**[Repository pattern](https://learn.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application)**) are separated by command and queries.

Instead of throwing exceptions, project use **[Result pattern](https://www.forevolve.com/en/articles/2018/03/19/operation-result/)** (using [FluentResuls package](https://github.com/altmann/FluentResults)) and for returning exact http response, every handler returns data wraped into HttpDataResponse object which contains also error messages collection and http response code.

Response value of every http status code is boxed into DataResponse<T> class which holds result data and collection of errors. This approach allows for example return some error messages together with OK status code.

Important part of every project are **[tests](https://github.com/Gramli/WeatherApi/tree/main/src/Tests)**. When writing tests we want to achieve [optimal code coverage](https://stackoverflow.com/questions/90002/what-is-a-reasonable-code-coverage-for-unit-tests-and-why). I think that every project has its own optimal code coverage number by it's need and I always follow the rule: cover your code to be able refactor without worry about functionality change.

In this solution, each 'code' project has its own unit test project and every **unit test** project copy the same directory structure as 'code' project, which is very helpful for orientation in test project.

To ensure that our REST API works as expected for end users we write **System tests**. Typically we call endpoints of the API in particular order defined by business requirements and check expected results. The solution contains simple [System Tests](https://github.com/Gramli/WeatherApi/tree/main/src/Tests/SystemTests) which just call exposed endpoints and check desired response.

### Minimal API
#### Pros
- **Reduce the ceremony of creating APIs**
	- no controllers (but you are still able to organize map methods in files)
	- injects bussines handlers directly into endpoints map methods
- **Minimal Hosting Model**
	- you are able to create single clean start point of the API

### Clean Architecture
#### Pros
- **UI/Framework/Database Independent** 
	- business logic is in the middle so framework, database (any external layer) can be easily changed without major impacts
- **Highly Testable** 
	- clean architechture is designed for testing, for example business logic can be tested without touching any external layer/element like database, UI, external web service etc.
	- easy to mock because of abstractions
- **Maintain and Extensibility**
	- thanks to defined structure, changes has isolated impact and together with SOLID principles it's easy to maintain

#### Cons
- **Understanding**
	- sometimes it's hard to split responsibilities or select right layer for our new code 
- **Heavy Structure**
	- it can be overkill to use it for simple CRUD api beacuse of lot of boilerplate code 

### Clean Architecture Layers

Solution contains four layers: 
* **WeatherAPI** - entry point of the application, top layer
	*  Endpoints
	*  Middlewares (or Filters)
	*  API Configuration
* **Weather.Infrastructure** - layer for communication with external resources like database, cache, web service.. 
	*  Repositories Implementation - access to database
	*  External Services Proxies - proxy classes implementation - to obtain data from external web services
	*  Infastructure Specific Services - services which are needed to interact with external libraries and frameworks
	* **Weatherbit.Client** - standalone project for communication with RapidAPI/Weatherbit
* **Weather.Core** - business logic of the application
	*  Request Handlers/Managers/.. - business implementation
	*  Abstractions - besides abstractions for business logic are there abstractions for Infrastructure layer (Service, Repository, ..) to be able use them in this (core) layer
* **Weather.Domain** - all what should be shared across all projects
	* DTOs
	* General Extensions

#### Horizontal Diagram (references)
![Project Clean Architecture Diagram](./doc/img/cleanArchitecture.jpg)

## Technologies
* [ASP.NET Core 8](https://learn.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-8.0)
* [Entity Framework Core InMemory](https://learn.microsoft.com/en-us/ef/core/providers/in-memory/?tabs=dotnet-core-cli)
* [AutoMapper](https://github.com/AutoMapper/AutoMapper)
* [FluentResuls](https://github.com/altmann/FluentResults)
* [Validot](https://github.com/bartoszlenar/Validot)
* [GuardClauses](https://github.com/ardalis/GuardClauses)
* [Moq](https://github.com/moq/moq4)
* [Xunit](https://github.com/xunit/xunit)




