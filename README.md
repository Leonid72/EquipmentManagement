
# Equipment Management System

A full‑stack **Equipment Management System** consisting of:
- **Angular 19 Frontend** (PrimeNG, RxJS)
- **.NET 8 Web API Backend** (Clean Architecture, EF Core)

https://youtu.be/QuzEIb71MvY

This repository contains everything needed to run the system locally or in production.

---

## 📦 System Overview

### Frontend
- Angular 19 (Standalone Components)
- PrimeNG 19 UI library
- RxJS reactive state
- Responsive & mobile‑friendly UI

### Backend
- ASP.NET Core Web API (.NET 8)
- Clean Architecture
- Entity Framework Core
- SQL Server
- Repository + Unit of Work
- Swagger / OpenAPI
- Serilog logging

---

## 🏗️ Architecture

```
Frontend (Angular 19)
        |
        v
REST API (.NET 8)
        |
        v
SQL Server Database
```

Backend follows Clean Architecture:

```
API
├── Application
├── Domain
├── Infrastructure
```

---

## ✨ Key Features

### Equipment Management
- Create / Edit / Delete equipment
- Categories & Locations
- Status tracking:
  - Active
  - InMaintenance
  - OutOfService
  - Retired

### UI Features
- Search & filtering
- Pagination
- Responsive tables
- Confirmation dialogs
- Toast notifications

### API Features
- Pagination, filtering & sorting
- DTO‑based contracts
- Validation & error handling
- Swagger documentation

---

## 🚀 Getting Started

### Prerequisites
- Node.js 18+
- npm 9+
- .NET SDK 8.0
- SQL Server (LocalDB / Express)

---

## 🖥️ Frontend Setup

```bash
cd frontend
npm install
npm start
```

Open:
```
http://localhost:4200
```

### Environment Configuration

`src/environments/environment.ts`

```ts
export const environment = {
  production: false,
  apiUrl: "https://localhost:5001/api"
};
```

---

## 🔧 Backend Setup

```bash
cd backend
dotnet restore
dotnet run
```

Open Swagger:
```
https://localhost:5001
```

---

## 💾 Database Setup

### Option 1 – EF Core Migrations

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Option 2 – SQL Script
Run:
```
scripts/CreateDatabase.sql
```

---

## 📡 API Endpoints

### Equipment

| Method | Endpoint | Description |
|------|--------|------------|
| GET | /api/equipment | List equipment |
| GET | /api/equipment/{id} | Get by ID |
| POST | /api/equipment | Create |
| PUT | /api/equipment/{id} | Update |
| DELETE | /api/equipment/{id} | Delete |

---

## 🧪 Testing

```bash
dotnet test
```

---

## 📁 Project Structure

```
EquipmentManagement/
├── frontend/
│   └── Angular 19 App
├── backend/
│   └── .NET 8 API
├── scripts/
│   └── SQL scripts
└── README.md
```

---

## 🔐 Validation Rules

- Equipment Name: 3–200 chars
- Serial Number: unique
- Purchase Date: not in future
- Category & Location: required

---

## 📜 License

MIT License

---

## 👤 Author

Equipment Management System  
Built with Angular 19 & .NET 8

---

Happy coding 🚀
