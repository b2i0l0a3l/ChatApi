# ChatApi — Real-Time Chat API with Clean Architecture

![.NET 9](https://img.shields.io/badge/.NET-9.0-blue)
![License: MIT](https://img.shields.io/badge/License-MIT-green)

A scalable, maintainable chat API built using **Clean Architecture**, **SignalR**, and **Entity Framework Core**.

---

## Table of Contents

- [About](#about)  
- [Features](#features)  
- [Architecture](#architecture)  
- [Getting Started](#getting-started)  
  - [Prerequisites](#prerequisites)  
  - [Installation](#installation)  
  - [Running the API](#running-the-api)  
- [Configuration](#configuration)  
- [Usage](#usage)  
- [Contributing](#contributing)  
- [License](#license)  
- [Contact](#contact)  

---

## About

ChatApi is a backend project that provides **real-time chat functionality** via RESTful endpoints and SignalR hubs.  
It is structured using **Clean Architecture**, which separates the project into Core, Application, Infrastructure, and API layers, making it testable, maintainable, and scalable.

---

## Features

- ✅ Clean Architecture for modularity  
- ✅ Real-time chat using SignalR  
- ✅ RESTful API endpoints (create chat, send messages, retrieve history)  
- ✅ Entity Framework Core for database operations  
- ✅ Dependency Injection and modular design  
- ✅ Unit tests and integration tests support  
- ✅ Extensible and maintainable  

---

## Architecture

The project layers:

| Layer          | Description                                      |
|----------------|--------------------------------------------------|
| **Core**       | Domain entities, interfaces, and business rules |
| **Application**| Use-cases and application logic                  |
| **Infrastructure** | , SignalR hub, external integrations |
| **API**        | Controllers, endpoints, and models              |

---

## Getting Started

### Prerequisites

- [.NET SDK 9.0 or higher](https://dotnet.microsoft.com/download/dotnet/9.0)  
- SQL Server / PostgreSQL / SQLite database  
- (Optional) Docker for containerized deployment  

### Installation

```bash
git clone https://github.com/b2i0l0a3l/ChatApi.git
cd ChatApi
dotnet restore
dotnet build
```

### Running the API

1. **Configure your connection string**  
   Open `appsettings.json` and update the connection string to point to your database.

2. **Apply database migrations**  

```bash
dotnet ef database update --project ChatApi.Infrastructure
```
3.**Run The Api**  
```bash
dotnet run --project ChatApi.Api
```
4.**Access the API**  
  The API will be available at: https://localhost:5230

---

```markdown```
## Configuration

The `appsettings.json` file contains the following settings:

- **Connection strings** – Database connection details  
- **JWT / Authentication settings** – Security and token configuration  

