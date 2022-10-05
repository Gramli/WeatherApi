# IN PROGRESS

# WeatherApi
Example API allows to get actual/forecast/favorite weather data by location and safe favorites locations to memory db.

REST API solution demonstrates how to create clean and modern API (from my point of view) with Clean Architecture, minimal API and CQRS, Repository patterns.  

### Benefits of Minimal API
- **Reduce the ceremony of creating APIs**
	- no controllers (but you are still able to organize map methods in files)
	- injects bussines handlers directly into endpoints map methods
	- you are able to create single clean start point of the API with minimal hosting model

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
* **Weather.Domain** - POCO classes, extensions, all what needs solution
	* DTOs
	* Payloads
	* General Extensions

#### Onion Diagram
![Project Clean Architecture Diagram - Onion](./doc/img/cleanArchitectureOnion.jpg)
#### Horizontal Diagram (references)
![Project Clean Architecture Diagram](./doc/img/cleanArchitecture.jpg)

## Conclusion
..



