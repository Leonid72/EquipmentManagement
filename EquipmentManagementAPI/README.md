# Equipment Management API

A comprehensive RESTful Web API for managing equipment, categories, and locations built with .NET 8, following Clean Architecture principles and the Repository Pattern.

## 📋 Table of Contents

- [Features](#features)
- [Architecture](#architecture)
- [Technologies Used](#technologies-used)
- [Getting Started](#getting-started)
- [Database Setup](#database-setup)
- [API Endpoints](#api-endpoints)
- [Running Tests](#running-tests)
- [Configuration](#configuration)
- [Project Structure](#project-structure)

## ✨ Features

- ✅ **RESTful API** with full CRUD operations
- ✅ **Clean Architecture** with clear separation of concerns
- ✅ **Repository Pattern** with Unit of Work
- ✅ **Entity Framework Core** for data access
- ✅ **Data Transfer Objects (DTOs)** for API contracts
- ✅ **Input Validation** using Data Annotations
- ✅ **Global Exception Handling** with meaningful error messages
- ✅ **Pagination, Filtering, and Sorting** for efficient data retrieval
- ✅ **Swagger/OpenAPI Documentation** for easy API exploration
- ✅ **Serilog Logging** for structured logging
- ✅ **Unit Tests** with xUnit and Moq
- ✅ **Dependency Injection** throughout the application
- ✅ **Proper HTTP Status Codes** for all responses

## 🏗️ Architecture

This project follows **Clean Architecture** principles with the following layers:

```
┌─────────────────────────────────────────────┐
│           Presentation Layer (API)          │
│   Controllers, Middleware, Configuration    │
└─────────────────┬───────────────────────────┘
                  │
┌─────────────────▼───────────────────────────┐
│         Application Layer                   │
│   Services, DTOs, Interfaces                │
└─────────────────┬───────────────────────────┘
                  │
┌─────────────────▼───────────────────────────┐
│         Domain Layer                        │
│   Entities, Interfaces, Business Logic      │
└─────────────────┬───────────────────────────┘
                  │
┌─────────────────▼───────────────────────────┐
│         Infrastructure Layer                │
│   EF Core, Repositories, Database Context   │
└─────────────────────────────────────────────┘
```

## 🛠️ Technologies Used

- **.NET 8.0** - Latest .NET framework
- **ASP.NET Core Web API** - Web framework
- **Entity Framework Core 8.0** - ORM for data access
- **SQL Server** - Database
- **Serilog** - Structured logging
- **Swagger/OpenAPI** - API documentation
- **xUnit** - Unit testing framework
- **Moq** - Mocking framework for tests

## 🚀 Getting Started

### Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (LocalDB, Express, or full version)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) or [VS Code](https://code.visualstudio.com/)

### Installation

1. **Clone or download the project**

2. **Navigate to the project directory**
   ```bash
   cd EquipmentManagementAPI
   ```

3. **Restore NuGet packages**
   ```bash
   dotnet restore
   ```

4. **Update the connection string**
   
   Edit `src/EquipmentManagement.API/appsettings.json` and update the connection string:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=YOUR_SERVER;Database=EquipmentManagementDB;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```

## 💾 Database Setup

### Option 1: Using the SQL Script (Recommended)

1. Open SQL Server Management Studio (SSMS)
2. Connect to your SQL Server instance
3. Open the script file: `scripts/CreateDatabase.sql`
4. Execute the script (F5)

This will:
- Create the database
- Create all tables with proper relationships
- Create indexes for performance
- Insert sample data
- Create views and stored procedures

### Option 2: Using Entity Framework Migrations

```bash
cd src/EquipmentManagement.API
dotnet ef migrations add InitialCreate --project ../EquipmentManagement.Infrastructure
dotnet ef database update
```

## 📡 API Endpoints

### Equipment Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/equipment` | Get all equipment (with pagination) |
| GET | `/api/equipment/{id}` | Get equipment by ID |
| GET | `/api/equipment/search` | Search equipment by name, date, category |
| POST | `/api/equipment` | Create new equipment |
| PUT | `/api/equipment/{id}` | Update equipment |
| DELETE | `/api/equipment/{id}` | Delete equipment |
| GET | `/api/equipment/category/{categoryId}` | Get equipment by category |

### Query Parameters for GET /api/equipment

- `pageNumber` (int, default: 1)
- `pageSize` (int, default: 10, max: 100)
- `searchTerm` (string, optional)
- `categoryId` (int, optional)
- `status` (string, optional: Active, InMaintenance, OutOfService, Retired)
- `sortBy` (string, default: EquipmentName)
- `sortDirection` (string, default: ASC)

### Example Requests

**Get all equipment with pagination:**
```http
GET /api/equipment?pageNumber=1&pageSize=10
```

**Search equipment:**
```http
GET /api/equipment/search?searchTerm=Dell&categoryId=1&status=Active
```

**Create equipment:**
```http
POST /api/equipment
Content-Type: application/json

{
  "equipmentName": "Dell Laptop",
  "serialNumber": "DL-2024-001",
  "categoryID": 1,
  "locationID": 1,
  "purchaseDate": "2024-01-15",
  "status": "Active"
}
```

## 🧪 Running Tests

Run all tests:
```bash
dotnet test
```

Run tests with coverage:
```bash
dotnet test /p:CollectCoverage=true
```

Run specific test class:
```bash
dotnet test --filter EquipmentControllerTests
```

## ⚙️ Configuration

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=EquipmentManagementDB;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "Microsoft.EntityFrameworkCore": "Warning"
      }
    }
  }
}
```

### Logging

Logs are written to:
- Console (all environments)
- File: `logs/equipment-api-YYYYMMDD.txt` (rolling daily logs)

## 📁 Project Structure

```
EquipmentManagementAPI/
├── src/
│   ├── EquipmentManagement.Domain/           # Domain entities and interfaces
│   │   ├── Entities/
│   │   │   ├── Equipment.cs
│   │   │   ├── Category.cs
│   │   │   └── Location.cs
│   │   └── Interfaces/
│   │       ├── IRepository.cs
│   │       ├── IEquipmentRepository.cs
│   │       └── IUnitOfWork.cs
│   │
│   ├── EquipmentManagement.Application/      # Application business logic
│   │   ├── DTOs/
│   │   │   ├── EquipmentDto.cs
│   │   │   ├── OtherDto.cs
│   │   │   └── ApiResponse.cs
│   │   ├── Interfaces/
│   │   │   └── IEquipmentService.cs
│   │   └── Services/
│   │       └── EquipmentService.cs
│   │
│   ├── EquipmentManagement.Infrastructure/   # Data access implementation
│   │   ├── Data/
│   │   │   └── ApplicationDbContext.cs
│   │   └── Repositories/
│   │       ├── Repository.cs
│   │       ├── EquipmentRepository.cs
│   │       └── UnitOfWork.cs
│   │
│   └── EquipmentManagement.API/              # API presentation layer
│       ├── Controllers/
│       │   └── EquipmentController.cs
│       ├── Middleware/
│       │   └── GlobalExceptionHandlerMiddleware.cs
│       ├── Program.cs
│       └── appsettings.json
│
├── tests/
│   └── EquipmentManagement.Tests/            # Unit tests
│       └── Controllers/
│           └── EquipmentControllerTests.cs
│
├── scripts/
│   └── CreateDatabase.sql                    # Database creation script
│
└── EquipmentManagement.sln                   # Solution file
```

## 🎯 Key Features Explained

### 1. Clean Architecture
- **Domain Layer**: Contains business entities and interfaces (no dependencies)
- **Application Layer**: Contains business logic and DTOs (depends only on Domain)
- **Infrastructure Layer**: Contains data access implementation (depends on Domain)
- **API Layer**: Contains controllers and middleware (depends on Application and Infrastructure)

### 2. Repository Pattern
- Generic `Repository<T>` for common CRUD operations
- Specialized `EquipmentRepository` for equipment-specific queries
- `UnitOfWork` for transaction management

### 3. DTOs (Data Transfer Objects)
- Separate DTOs for different operations (Create, Update, Read)
- Input validation using Data Annotations
- Prevents over-posting and under-posting

### 4. Error Handling
- Global exception handler middleware
- Consistent error response format
- Meaningful error messages with proper HTTP status codes

### 5. Logging
- Structured logging with Serilog
- Request/response logging
- Error logging with stack traces

## 📊 Database Schema

### Equipment Table
- EquipmentID (PK, Identity)
- EquipmentName (NVARCHAR(200), Required)
- SerialNumber (NVARCHAR(100), Unique, Required)
- CategoryID (FK to Categories)
- LocationID (FK to Locations)
- PurchaseDate (DATE, Required)
- Status (NVARCHAR(50), CHECK constraint)
- CreatedDate (DATETIME2)
- ModifiedDate (DATETIME2)

### Categories Table
- CategoryID (PK, Identity)
- CategoryName (NVARCHAR(100), Unique, Required)
- Description (NVARCHAR(500))
- CreatedDate (DATETIME2)
- ModifiedDate (DATETIME2)

### Locations Table
- LocationID (PK, Identity)
- LocationName (NVARCHAR(100), Required)
- Building (NVARCHAR(100), Required)
- Floor (NVARCHAR(50), Required)
- CreatedDate (DATETIME2)
- ModifiedDate (DATETIME2)
- Unique constraint on (LocationName, Building, Floor)

## 🔍 Swagger UI

Once the API is running, navigate to:
```
https://localhost:5001
```

Swagger UI provides:
- Interactive API documentation
- Ability to test endpoints directly
- Request/response examples
- Model schemas

## 🤝 Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## 📝 License

This project is created for educational and demonstration purposes.

## 👤 Author

Equipment Management System Team

## 📞 Support

For issues and questions, please create an issue in the repository.

---

**Happy Coding! 🚀**
