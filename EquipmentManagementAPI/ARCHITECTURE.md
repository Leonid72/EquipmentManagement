# Equipment Management API - Architecture Overview

## System Architecture Diagram

```
┌─────────────────────────────────────────────────────────────────────┐
│                          CLIENT LAYER                               │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐             │
│  │   Browser    │  │   Postman    │  │  Mobile App  │             │
│  │ (Swagger UI) │  │              │  │              │             │
│  └──────┬───────┘  └──────┬───────┘  └──────┬───────┘             │
└─────────┼──────────────────┼──────────────────┼────────────────────┘
          │                  │                  │
          └──────────────────┴──────────────────┘
                             │
                    HTTPS/REST API
                             │
┌─────────────────────────────▼─────────────────────────────────────┐
│                     PRESENTATION LAYER                            │
│                  (EquipmentManagement.API)                        │
│                                                                   │
│  ┌────────────────────────────────────────────────────────────┐  │
│  │                    Controllers                             │  │
│  │  • EquipmentController                                    │  │
│  │    - GET /api/equipment                                   │  │
│  │    - GET /api/equipment/{id}                              │  │
│  │    - POST /api/equipment                                  │  │
│  │    - PUT /api/equipment/{id}                              │  │
│  │    - DELETE /api/equipment/{id}                           │  │
│  │    - GET /api/equipment/search                            │  │
│  │    - GET /api/equipment/category/{categoryId}             │  │
│  └────────────────────────────────────────────────────────────┘  │
│                                                                   │
│  ┌────────────────────────────────────────────────────────────┐  │
│  │                     Middleware                             │  │
│  │  • GlobalExceptionHandler                                 │  │
│  │  • Logging (Serilog)                                      │  │
│  │  • CORS                                                   │  │
│  │  • Swagger/OpenAPI                                        │  │
│  └────────────────────────────────────────────────────────────┘  │
└─────────────────────────────┬───────────────────────────────────┘
                              │
                    Dependency Injection
                              │
┌─────────────────────────────▼───────────────────────────────────────┐
│                      APPLICATION LAYER                              │
│                (EquipmentManagement.Application)                    │
│                                                                     │
│  ┌──────────────────────────────────────────────────────────────┐  │
│  │                        Services                              │  │
│  │  • EquipmentService                                         │  │
│  │    - Business Logic                                         │  │
│  │    - Validation                                             │  │
│  │    - Error Handling                                         │  │
│  │    - DTO Mapping                                            │  │
│  └──────────────────────────────────────────────────────────────┘  │
│                                                                     │
│  ┌──────────────────────────────────────────────────────────────┐  │
│  │                         DTOs                                 │  │
│  │  • CreateEquipmentDto                                       │  │
│  │  • UpdateEquipmentDto                                       │  │
│  │  • EquipmentDto                                             │  │
│  │  • EquipmentSearchDto                                       │  │
│  │  • ApiResponse<T>                                           │  │
│  │  • PagedResult<T>                                           │  │
│  └──────────────────────────────────────────────────────────────┘  │
└─────────────────────────────┬───────────────────────────────────────┘
                              │
                      Interface Contracts
                              │
┌─────────────────────────────▼───────────────────────────────────────┐
│                        DOMAIN LAYER                                 │
│                  (EquipmentManagement.Domain)                       │
│                                                                     │
│  ┌──────────────────────────────────────────────────────────────┐  │
│  │                       Entities                               │  │
│  │  • Equipment                                                │  │
│  │  • Category                                                 │  │
│  │  • Location                                                 │  │
│  │  • EquipmentStatus (Enum)                                   │  │
│  └──────────────────────────────────────────────────────────────┘  │
│                                                                     │
│  ┌──────────────────────────────────────────────────────────────┐  │
│  │                      Interfaces                              │  │
│  │  • IRepository<T>                                           │  │
│  │  • IEquipmentRepository                                     │  │
│  │  • IUnitOfWork                                              │  │
│  └──────────────────────────────────────────────────────────────┘  │
└─────────────────────────────┬───────────────────────────────────────┘
                              │
                    Implementation
                              │
┌─────────────────────────────▼───────────────────────────────────────┐
│                    INFRASTRUCTURE LAYER                             │
│                (EquipmentManagement.Infrastructure)                 │
│                                                                     │
│  ┌──────────────────────────────────────────────────────────────┐  │
│  │                    Repositories                              │  │
│  │  • Repository<T> (Generic)                                  │  │
│  │  • EquipmentRepository                                      │  │
│  │  • UnitOfWork                                               │  │
│  └──────────────────────────────────────────────────────────────┘  │
│                                                                     │
│  ┌──────────────────────────────────────────────────────────────┐  │
│  │                 Data Access (EF Core)                        │  │
│  │  • ApplicationDbContext                                     │  │
│  │  • Entity Configurations                                    │  │
│  │  • Migrations                                               │  │
│  └──────────────────────────────────────────────────────────────┘  │
└─────────────────────────────┬───────────────────────────────────────┘
                              │
                        SQL Queries
                              │
┌─────────────────────────────▼───────────────────────────────────────┐
│                       DATABASE LAYER                                │
│                      (SQL Server)                                   │
│                                                                     │
│  ┌──────────────────────────────────────────────────────────────┐  │
│  │                        Tables                                │  │
│  │  ┌─────────────┐  ┌──────────────┐  ┌──────────────┐       │  │
│  │  │  Equipment  │  │  Categories  │  │  Locations   │       │  │
│  │  │             │  │              │  │              │       │  │
│  │  │ • ID (PK)   │  │ • ID (PK)    │  │ • ID (PK)    │       │  │
│  │  │ • Name      │  │ • Name       │  │ • Name       │       │  │
│  │  │ • Serial    │  │ • Desc       │  │ • Building   │       │  │
│  │  │ • CategoryID│──┼─ (Unique)    │  │ • Floor      │       │  │
│  │  │ • LocationID│  │              │──┼─ (Composite  │       │  │
│  │  │ • PurchDate │  │              │  │   Unique)    │       │  │
│  │  │ • Status    │  │              │  │              │       │  │
│  │  └─────────────┘  └──────────────┘  └──────────────┘       │  │
│  │                                                              │  │
│  │  ┌──────────────────────────────────────────────────────┐   │  │
│  │  │              Indexes & Constraints                   │   │  │
│  │  │  • UQ_SerialNumber (Unique)                          │   │  │
│  │  │  • FK_Equipment_Categories                           │   │  │
│  │  │  • FK_Equipment_Locations                            │   │  │
│  │  │  • CHK_Status                                        │   │  │
│  │  │  • IX_Equipment_CategoryID                           │   │  │
│  │  │  • IX_Equipment_LocationID                           │   │  │
│  │  └──────────────────────────────────────────────────────┘   │  │
│  └──────────────────────────────────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────────┘
```

## Data Flow Diagram

### Create Equipment Flow

```
┌──────────┐
│  Client  │
└────┬─────┘
     │ POST /api/equipment
     │ { equipmentData }
     ▼
┌────────────────────────┐
│  EquipmentController   │
│  • Validate ModelState │
│  • Check business rules│
└────────┬───────────────┘
         │
         ▼
┌────────────────────────┐
│   EquipmentService     │
│  • Validate data       │
│  • Check serial exists │
│  • Verify category     │
│  • Verify location     │
└────────┬───────────────┘
         │
         ▼
┌────────────────────────┐
│      UnitOfWork        │
│  • Begin transaction   │
└────────┬───────────────┘
         │
         ▼
┌────────────────────────┐
│  EquipmentRepository   │
│  • AddAsync()          │
└────────┬───────────────┘
         │
         ▼
┌────────────────────────┐
│   DbContext (EF Core)  │
│  • Track entity        │
└────────┬───────────────┘
         │
         ▼
┌────────────────────────┐
│      UnitOfWork        │
│  • SaveChanges()       │
│  • Commit transaction  │
└────────┬───────────────┘
         │
         ▼
┌────────────────────────┐
│     SQL Server         │
│  • INSERT INTO         │
│  • Return IDENTITY     │
└────────┬───────────────┘
         │
         ▼
┌────────────────────────┐
│   EquipmentService     │
│  • Map to DTO          │
│  • Create response     │
└────────┬───────────────┘
         │
         ▼
┌────────────────────────┐
│  EquipmentController   │
│  • Return 201 Created  │
│  • Include Location    │
└────────┬───────────────┘
         │
         ▼
┌──────────┐
│  Client  │
│  Success │
└──────────┘
```

## Dependency Flow

```
API Layer
   ↓ depends on
Application Layer
   ↓ depends on
Domain Layer
   ↑ implemented by
Infrastructure Layer
```

## Key Architectural Principles

### 1. Separation of Concerns
- Each layer has a specific responsibility
- No cross-cutting concerns
- Clean boundaries between layers

### 2. Dependency Inversion
- High-level modules don't depend on low-level modules
- Both depend on abstractions (interfaces)
- Infrastructure implements domain interfaces

### 3. Single Responsibility
- Each class has one reason to change
- Controllers handle HTTP
- Services handle business logic
- Repositories handle data access

### 4. Open/Closed Principle
- Open for extension
- Closed for modification
- Use interfaces and inheritance

## Component Interactions

### Request Pipeline

```
Request → Middleware → Controller → Service → Repository → Database
                ↓
            Logging
                ↓
        Exception Handler
```

### Response Pipeline

```
Database → Repository → Service → Controller → Middleware → Response
                                                    ↑
                                             DTO Serialization
```

## Cross-Cutting Concerns

### Logging
- Serilog integration
- Structured logging
- File and console sinks
- Request/response logging

### Error Handling
- Global exception middleware
- Consistent error responses
- Proper HTTP status codes
- Detailed error messages

### Validation
- Data Annotations
- FluentValidation ready
- Business rule validation
- Database constraints

### Testing
- Unit tests for controllers
- Mock-based testing
- xUnit framework
- Moq for mocking

## Design Patterns Used

1. **Repository Pattern**
   - Abstracts data access
   - Testable data layer
   - Consistent API

2. **Unit of Work Pattern**
   - Transaction management
   - Coordinated saves
   - Rollback support

3. **Dependency Injection**
   - Loose coupling
   - Testability
   - Flexibility

4. **DTO Pattern**
   - API contracts
   - Validation separation
   - Over-posting prevention

5. **Factory Pattern**
   - Repository creation
   - Service instantiation

## Benefits of This Architecture

✅ **Maintainability**
- Easy to understand
- Easy to modify
- Easy to test

✅ **Scalability**
- Can add new features easily
- Can swap implementations
- Can add new layers

✅ **Testability**
- Each layer can be tested independently
- Mock-based testing
- High test coverage

✅ **Flexibility**
- Can change database
- Can change business logic
- Can change UI

✅ **Reusability**
- Generic repository
- Shared DTOs
- Common patterns
