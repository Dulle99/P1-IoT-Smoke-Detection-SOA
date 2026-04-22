# IoT Smoke Detection System – Service-Oriented Architecture (SOA)

**Author:** Dušan Sotirov

## Project Description

This project implements an **IoT Smoke Detection System** using a **service-oriented architecture with two microservices built in different technologies**.

The goal of the project is to demonstrate how an IoT-oriented system can be decomposed into smaller services that communicate over REST, while also showing how an API Gateway can aggregate internal and external data into a single response.

The system is based on smoke sensor readings and simulates a realistic monitoring scenario in which sensor data is stored, queried, updated, and integrated with external environmental information.

---

## Problem Statement

Modern IoT systems often collect large amounts of sensor data from distributed devices.  
In a smoke detection scenario, raw readings such as temperature, humidity, air quality indicators, and fire alarm state are useful on their own, but their value becomes much higher when they are:

- stored in a structured way,
- made accessible through APIs,
- processed by separate services with clear responsibilities,
- and enriched with external contextual data.

A monolithic solution could handle all of this in one application, but that would make the system harder to extend, maintain, and reuse.

This project solves that problem by separating responsibilities into two microservices:

- a **GatewayService**, which acts as the public entry point and facade,
- and a **DataService**, which is responsible for storing and retrieving smoke sensor data.

The gateway also integrates an **external weather API**, so the client can receive both local sensor readings and current weather information in a single response.

---

## Value and Motivation of the Project

The project demonstrates why service-oriented and microservice-based design is useful in IoT systems.

### Main value of the solution

- **Separation of concerns**  
  Data persistence is isolated in one service, while orchestration and aggregation are handled in another.

- **Technology heterogeneity**  
  The system uses two different technologies:
  - **ASP.NET Core** for the GatewayService
  - **Node.js + Express** for the DataService

- **REST-based interoperability**  
  Services communicate synchronously through HTTP APIs, which reflects a common SOA pattern.

- **Integration of internal and external data**  
  The system combines local smoke readings with live weather data from an external API.

- **Improved maintainability**  
  Each service can be modified independently without changing the whole system design.

- **Realistic IoT data handling**  
  The project includes dataset import and CRUD operations over sensor readings.

---

## Chosen Data Domain

The chosen domain is **smoke detection / environmental monitoring in IoT systems**.

The application works with smoke-related and environment-related sensor readings, including values such as:

- `utc` – timestamp of the reading
- `temperatureC` – measured temperature in Celsius
- `humidityPercent` – humidity percentage
- `eCo2Ppm` – equivalent CO2 level
- `tVocPpb` – total volatile organic compounds
- `pressureHpa` – atmospheric pressure
- `pm25` – PM2.5 particle concentration
- `fireAlarm` – boolean value indicating detected alarm state

These values are useful because they represent typical environmental readings that can indicate smoke, air pollution, or dangerous indoor conditions.

---

## Architecture Overview

The system is composed of the following parts:

- **GatewayService** – ASP.NET Core microservice acting as API Gateway / Facade
- **DataService** – Node.js + Express microservice for persistence and CRUD operations
- **MongoDB** – NoSQL database for storing sensor readings
- **Open-Meteo API** – external public API used for weather integration

### High-Level Flow

```text
Client
   |
   v
GatewayService (.NET / ASP.NET Core)
   |
   +--> DataService (Node.js / Express)
   |        |
   |        v
   |      MongoDB
   |
   +--> External API (Open-Meteo)
```

### Service Responsibilities

#### GatewayService
GatewayService acts as the **facade** of the system.

Responsibilities:

- exposes the main public REST API,
- forwards CRUD requests to DataService,
- synchronously communicates with DataService,
- integrates data from the external Open-Meteo API,
- returns aggregated responses to the client.

#### DataService
DataService is responsible for persistence and direct access to smoke readings.

Responsibilities:

- validates incoming requests,
- stores smoke sensor readings in MongoDB,
- supports create, read, update, and delete operations,
- exposes its own REST API,
- provides OpenAPI documentation,
- supports bulk CSV import.

#### MongoDB
MongoDB is used as the **local NoSQL database**.

Responsibilities:

- stores the smoke reading documents,
- supports flexible schema for sensor data,
- provides persistent storage for the data service.

---

## Implemented Requirements

This project satisfies the following key project requirements:

- two microservices in **different technologies**
- REST API communication between microservices
- one microservice acting as an **API Gateway / facade**
- **CRUD** operations implemented through the gateway
- synchronous access from gateway to data microservice
- integration with one **external public API**
- local **NoSQL database**
- OpenAPI / Swagger documentation
- project source code hosted on GitHub
- API testing support through **Swagger UI** and **Postman collection**

---

## Technologies Used

- **ASP.NET Core** – GatewayService
- **Node.js + Express** – DataService
- **MongoDB** – NoSQL database
- **Docker & Docker Compose** – container orchestration
- **Swagger / OpenAPI** – API documentation
- **Postman** – API testing
- **CSV Parser** – dataset import
- **REST APIs** – service communication
- **Open-Meteo API** – external weather integration

---

## API Overview

### GatewayService Endpoints

- `GET /api/health`  
  Health check for GatewayService

- `POST /api/readings`  
  Create a new smoke reading

- `GET /api/readings`  
  Get a list of readings

- `GET /api/readings/{id}`  
  Get one reading by ID

- `PUT /api/readings/{id}`  
  Update a reading by ID

- `DELETE /api/readings/{id}`  
  Delete a reading by ID

- `GET /api/insights/current?limit=...`  
  Returns recent smoke readings enriched with current weather data

### DataService Endpoints

- `GET /health`  
  Health check for DataService and MongoDB connection

- `POST /readings`  
  Store a new reading in MongoDB

- `GET /readings`  
  Get readings from MongoDB

- `GET /readings/{id}`  
  Get a single reading by ID

- `PUT /readings/{id}`  
  Update a reading by ID

- `DELETE /readings/{id}`  
  Delete a reading by ID

---

## API Documentation and Testing

### Swagger / OpenAPI

#### GatewayService Swagger UI
- `http://localhost:8080/swagger`

#### DataService Swagger UI
- `http://localhost:5001/api-docs`

#### DataService OpenAPI JSON
- `http://localhost:5001/api-docs.json`

### Postman Collection
A Postman collection is included in the repository for testing both services.

Current location in the repository:
- `DataService/postman_collection.json`

---

## How to Run the Project

### Prerequisites

Before running the project, make sure you have installed:

- **Docker Desktop**
- **Git**
- **Node.js** (only needed for local CSV import)
- optionally **Postman** for API testing

---

## Running the Project with Docker Compose

From the repository root, run:

```bash
docker compose up --build
```

This will start:

- MongoDB
- DataService
- GatewayService

### After startup, the following URLs should be available

#### GatewayService
- Swagger UI: `http://localhost:8080/swagger`
- Health endpoint: `http://localhost:8080/api/health`

#### DataService
- Health endpoint: `http://localhost:5001/health`
- Swagger UI: `http://localhost:5001/api-docs`
- OpenAPI JSON: `http://localhost:5001/api-docs.json`

---

## CSV Dataset Import

The project supports importing a smoke detection dataset from CSV into MongoDB.

Expected dataset location:

```text
data/smoke_detection_iot.csv
```

To import the dataset manually, run:

```bash
cd DataService
npm install
npm run import:csv
```

This script reads the CSV file and inserts the data into MongoDB.

---

## Example Usage Scenario

A typical usage flow of the system is:

1. Start all services with Docker Compose
2. Open Gateway Swagger UI
3. Create a new smoke reading through GatewayService
4. Read the stored readings
5. Update or delete a reading if needed
6. Call `/api/insights/current` to retrieve sensor data together with weather information
7. Optionally import the dataset into MongoDB using the CSV import script

---

## Project Structure

```text
P1-IoT-Smoke-Detection-SOA/
│
├── GatewayService/
│   └── GatewayService/
│       ├── Controllers/
│       ├── Dtos/
│       └── Program.cs
│
├── DataService/
│   ├── src/
│   ├── scripts/
│   ├── openapi.yaml
│   ├── Dockerfile
│   └── postman_collection.json
│
├── data/
│   └── smoke_detection_iot.csv
│
└── docker-compose.yaml
```

---

## Concepts Demonstrated

This project demonstrates:

- Service-Oriented Architecture (SOA)
- API Gateway / Facade pattern
- RESTful communication between microservices
- synchronous service orchestration
- NoSQL data storage
- CRUD operations across services
- external API integration
- OpenAPI documentation
- Dockerized deployment
- bulk dataset import

---

## Notes

- GatewayService forwards CRUD operations to DataService.
- DataService is the only service directly connected to MongoDB.
- The external Open-Meteo API is used only by GatewayService.
- The API responses use a unified `id` field instead of raw MongoDB `_id`.
- The system is intended as a university SOA project demonstrating distributed design and service composition.
