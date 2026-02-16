IoT Smoke Detection System – Service-Oriented Architecture (SOA)

Author: Dušan Sotirov
Architecture: Microservices (Service-Oriented Architecture)
Technologies: ASP.NET Core, Node.js, MongoDB, Docker


Project Overview

This project implements a microservice-based IoT Smoke Detection system using a Service-Oriented Architecture (SOA).

The system consists of:

A GatewayService built with ASP.NET Core acting as a facade and API aggregator.

A DataService built with Node.js and Express responsible for data persistence.

A MongoDB NoSQL database for storing sensor readings.

An external Weather API (Open-Meteo) integrated by the Gateway to demonstrate service orchestration.

The project demonstrates distributed system design, RESTful communication between services, container orchestration using Docker Compose, and bulk dataset ingestion from CSV.



Architecture Overview

Client (Swagger / HTTP)
        ↓
GatewayService (ASP.NET Core)
        ↓
DataService (Node.js + Express)
        ↓
MongoDB (Docker)

GatewayService also calls:
        ↓
External Weather API (Open-Meteo)

GatewayService

Acts as a Facade (API Gateway pattern)

Forwards client requests to DataService

Integrates external Weather API

Returns aggregated JSON responses

DataService

Handles CRUD operations

Connects to MongoDB

Stores IoT sensor readings

Supports filtering and querying

 MongoDB

Document-based NoSQL database

Stores sensor readings

Indexed for performance



Technologies Used

ASP.NET Core – Gateway service

Node.js + Express – Data service

MongoDB – NoSQL database

Docker & Docker Compose – Container orchestration

Swagger / OpenAPI – API documentation

CSV Parser – Bulk dataset ingestion

RESTful APIs – Service communication

Open-Meteo API – External service integration



Features

CRUD operations on sensor readings

Filtering by timestamp and fire alarm status

API aggregation (sensor data + live weather)

Dockerized multi-service architecture

Bulk CSV import of IoT dataset

Environment-based configuration

Swagger UI for testing



How to Run

Using Docker

From repository root:

docker compose up --build

After startup:

Swagger UI → http://localhost:8080/swagger

DataService → http://localhost:5001/health


Import CSV Dataset

Place dataset inside:

/data/smoke_detection_iot.csv

Then run:

cd DataService
npm run import:csv

This performs bulk insertion into MongoDB.



API Overview

GatewayService Endpoints
Method	Endpoint	Description
POST	/api/readings	Create new reading
GET	/api/readings	Get readings (with filters)
GET	/api/readings/{id}	Get reading by ID
GET	/api/insights/current	Latest reading + Weather integration
DataService Endpoints
Method	Endpoint	Description
POST	/readings	Store reading
GET	/readings	Query readings
GET	/readings/:id	Get reading by ID
GET	/health	Service health check



Concepts Demonstrated

This project demonstrates:

Service-Oriented Architecture (SOA)

API Gateway / Facade Pattern

RESTful design principles

Microservice communication over HTTP

NoSQL document modeling

Dependency Injection (ASP.NET Core)

Middleware pipeline architecture

Environment-based configuration

Container orchestration

Bulk data ingestion

API aggregation (service composition)