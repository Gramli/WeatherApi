 <img align="left" width="116" height="116" src=".\doc\img\weatherApi_icon.png" />
 
 # Clean Architecture WeatherApi
[![.NET Build and Test](https://github.com/Gramli/WeatherApi/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Gramli/WeatherApi/actions/workflows/dotnet.yml)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/77a7db482a44489aa5fbe40ca15d3137)](https://www.codacy.com/gh/Gramli/WeatherApi/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=Gramli/WeatherApi&amp;utm_campaign=Badge_Grade)
[![Codacy Badge](https://app.codacy.com/project/badge/Coverage/77a7db482a44489aa5fbe40ca15d3137)](https://www.codacy.com/gh/Gramli/WeatherApi/dashboard?utm_source=github.com&utm_medium=referral&utm_content=Gramli/WeatherApi&utm_campaign=Badge_Coverage)

This REST API solution demonstrates how to create a clean, modern API (from my point of view) using Clean Architecture, Minimal API, and various design patterns.

The example API allows users to retrieve current and forecasted weather data by location from [Weatherbit](https://www.weatherbit.io/) via [RapidAPI](https://rapidapi.com). It also allows users to add favorite locations to an [in memory database](https://learn.microsoft.com/en-us/ef/core/providers/in-memory/?tabs=dotnet-core-cli) and retrieve weather data for those stored locations.

## Menu
* [Get Started](#get-started)
* [Motivation](#motivation)
* [Architecture](#architecture)
	* [Clean Architecture Layers](#clean-architecture-layers)
 	* [Pros and Cons](#pros-and-cons) 
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
The main motivation for this project is to create a practical example of Minimal API, to explore its benefits and drawbacks, and to build a REST API using Clean Architecture and various design patterns.
## Architecture

This project follows **[Clean Architecture](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures#clean-architecture)**. The application layer is split into the Core and Domain projects, where the Core project holds the business rules, and the Domain project contains the business entities.

Since Minimal API allows injecting handlers into endpoint mapping methods, I decided not to use **[MediatR](https://github.com/jbogard/MediatR)**. Instead, each endpoint has its own request and handler. The solution follows the **[CQRS pattern](https://learn.microsoft.com/en-us/azure/architecture/patterns/cqrs)**, where handlers are separated into commands and queries. Command handlers handle command requests, while query handlers handle query requests. Additionally, repositories (**[Repository pattern](https://learn.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application)**) are also separated into command and query repositories.

Instead of throwing exceptions, the project uses the **[Result pattern](https://www.forevolve.com/en/articles/2018/03/19/operation-result/)** (using the [FluentResuls package](https://github.com/altmann/FluentResults)). Every handler returns data wrapped in an HttpDataResponse object, which also contains a collection of error messages and an HTTP status code.

Each HTTP status code's response is wrapped in a DataResponse<T> class, which holds the result data and any errors. This approach allows, for example, returning error messages alongside an "OK" status code.

An important aspect of any project is **[testing](https://github.com/Gramli/WeatherApi/tree/main/src/Tests)**. When writing tests, we aim for [optimal code coverage](https://stackoverflow.com/questions/90002/what-is-a-reasonable-code-coverage-for-unit-tests-and-why). I believe every project has its own optimal coverage based on its needs. My rule is: cover your code enough to confidently refactor without worrying about functionality changes.

In this solution, each code project has its own unit test project, and each unit test project mirrors the directory structure of its respective code project. This structure helps with organization in larger projects.

To ensure the REST API works as expected for end users, we write **system tests**. These tests typically call API endpoints in a specific order defined by business requirements and check the expected results. The solution contains simple [System Tests](https://github.com/Gramli/WeatherApi/tree/main/src/Tests/SystemTests), which call the exposed endpoints and validate the response.

### Clean Architecture Layers

The solution consists of four layers:
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

### Pros and Cons
* [Clean Architecture pros and cons](https://gramli.github.io//posts/architecture/clean-architecture-pros-and-cons)
* [Minimal API pros and cons](https://gramli.github.io/posts/code/aspnet/minimap-api-pros-and-cons)

## Technologies
* [ASP.NET Core 8](https://learn.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-8.0)
* [Entity Framework Core InMemory](https://learn.microsoft.com/en-us/ef/core/providers/in-memory/?tabs=dotnet-core-cli)
* [AutoMapper](https://github.com/AutoMapper/AutoMapper)
* [FluentResuls](https://github.com/altmann/FluentResults)
* [Validot](https://github.com/bartoszlenar/Validot)
* [GuardClauses](https://github.com/ardalis/GuardClauses)
* [Moq](https://github.com/moq/moq4)
* [Xunit](https://github.com/xunit/xunit)




