# API Integration Guide

This document provides detailed information about integrating your backend API with the Equipment Management Angular application.

## API Configuration

### 1. Update Environment Files

Edit the environment files to point to your API:

**Development** (`src/environments/environment.ts`):
```typescript
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5000/api'  // Your local API URL
};
```

**Production** (`src/environments/environment.prod.ts`):
```typescript
export const environment = {
  production: true,
  apiUrl: 'https://api.yourdomain.com/api'  // Your production API URL
};
```

## Expected API Endpoints

### Equipment Endpoints

#### Get All Equipment (Paginated & Filtered)
```
GET /api/equipment
```

**Query Parameters:**
- `pageNumber` (required): Page number (starts at 1)
- `pageSize` (required): Number of items per page
- `sortBy` (required): Field to sort by (e.g., "equipmentName")
- `sortDirection` (required): "ASC" or "DESC"
- `searchTerm` (optional): Search by name or serial number
- `categoryID` (optional): Filter by category ID
- `status` (optional): Filter by status
- `purchaseDateFrom` (optional): Filter by purchase date (from)
- `purchaseDateTo` (optional): Filter by purchase date (to)

**Response:**
```json
{
  "success": true,
  "message": "Equipment retrieved successfully",
  "data": {
    "items": [
      {
        "equipmentID": 1,
        "equipmentName": "Laptop Dell XPS 15",
        "serialNumber": "SN12345",
        "categoryID": 1,
        "categoryName": "Computers",
        "locationID": 1,
        "locationName": "IT Department",
        "building": "Main Building",
        "floor": "3rd Floor",
        "purchaseDate": "2023-01-15T00:00:00",
        "status": "Active"
      }
    ],
    "totalCount": 100,
    "pageNumber": 1,
    "pageSize": 10,
    "totalPages": 10,
    "hasPreviousPage": false,
    "hasNextPage": true
  },
  "errors": []
}
```

#### Get Equipment by ID
```
GET /api/equipment/{id}
```

**Response:**
```json
{
  "success": true,
  "message": "Equipment retrieved successfully",
  "data": {
    "equipmentID": 1,
    "equipmentName": "Laptop Dell XPS 15",
    "serialNumber": "SN12345",
    "categoryID": 1,
    "categoryName": "Computers",
    "locationID": 1,
    "locationName": "IT Department",
    "building": "Main Building",
    "floor": "3rd Floor",
    "purchaseDate": "2023-01-15T00:00:00",
    "status": "Active"
  },
  "errors": []
}
```

#### Create Equipment
```
POST /api/equipment
```

**Request Body:**
```json
{
  "equipmentName": "Laptop Dell XPS 15",
  "serialNumber": "SN12345",
  "categoryID": 1,
  "locationID": 1,
  "purchaseDate": "2023-01-15T00:00:00",
  "status": "Active"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Equipment created successfully",
  "data": {
    "equipmentID": 1,
    "equipmentName": "Laptop Dell XPS 15",
    "serialNumber": "SN12345",
    "categoryID": 1,
    "categoryName": "Computers",
    "locationID": 1,
    "locationName": "IT Department",
    "building": "Main Building",
    "floor": "3rd Floor",
    "purchaseDate": "2023-01-15T00:00:00",
    "status": "Active"
  },
  "errors": []
}
```

#### Update Equipment
```
PUT /api/equipment/{id}
```

**Request Body:**
```json
{
  "equipmentID": 1,
  "equipmentName": "Laptop Dell XPS 15 Updated",
  "serialNumber": "SN12345",
  "categoryID": 1,
  "locationID": 1,
  "purchaseDate": "2023-01-15T00:00:00",
  "status": "InMaintenance"
}
```

**Response:**
```json
{
  "success": true,
  "message": "Equipment updated successfully",
  "data": {
    "equipmentID": 1,
    "equipmentName": "Laptop Dell XPS 15 Updated",
    "serialNumber": "SN12345",
    "categoryID": 1,
    "categoryName": "Computers",
    "locationID": 1,
    "locationName": "IT Department",
    "building": "Main Building",
    "floor": "3rd Floor",
    "purchaseDate": "2023-01-15T00:00:00",
    "status": "InMaintenance"
  },
  "errors": []
}
```

#### Delete Equipment
```
DELETE /api/equipment/{id}
```

**Response:**
```json
{
  "success": true,
  "message": "Equipment deleted successfully",
  "data": true,
  "errors": []
}
```

### Category Endpoints

#### Get All Categories
```
GET /api/categories
```

**Response:**
```json
{
  "success": true,
  "message": "Categories retrieved successfully",
  "data": [
    {
      "categoryID": 1,
      "categoryName": "Computers",
      "description": "Desktop and laptop computers"
    },
    {
      "categoryID": 2,
      "categoryName": "Network Equipment",
      "description": "Routers, switches, and network devices"
    }
  ],
  "errors": []
}
```

### Location Endpoints

#### Get All Locations
```
GET /api/locations
```

**Response:**
```json
{
  "success": true,
  "message": "Locations retrieved successfully",
  "data": [
    {
      "locationID": 1,
      "locationName": "IT Department",
      "building": "Main Building",
      "floor": "3rd Floor"
    },
    {
      "locationID": 2,
      "locationName": "HR Department",
      "building": "Main Building",
      "floor": "2nd Floor"
    }
  ],
  "errors": []
}
```

## Error Handling

### Validation Errors (400 Bad Request)
```json
{
  "success": false,
  "message": "Validation failed",
  "data": null,
  "errors": [
    "Equipment name is required",
    "Serial number must be at least 5 characters"
  ]
}
```

### Not Found (404)
```json
{
  "success": false,
  "message": "Equipment not found",
  "data": null,
  "errors": []
}
```

### Server Error (500)
```json
{
  "success": false,
  "message": "An error occurred while processing your request",
  "data": null,
  "errors": ["Detailed error message"]
}
```

## CORS Configuration

Ensure your backend API allows CORS requests from your Angular application domain.

**ASP.NET Core Example:**
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200", "https://yourdomain.com")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

app.UseCors("AllowAngularApp");
```

## Authentication (Optional)

If your API requires authentication, update the HTTP interceptor:

```typescript
// src/app/shared/interceptors/auth.interceptor.ts
import { HttpInterceptorFn } from '@angular/common/http';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const token = localStorage.getItem('auth_token');
  
  if (token) {
    req = req.clone({
      setHeaders: {
        Authorization: `Bearer ${token}`
      }
    });
  }
  
  return next(req);
};
```

Then add it to `app.config.ts`:
```typescript
provideHttpClient(
  withInterceptors([authInterceptor, errorInterceptor])
)
```

## Testing the API

You can use tools like:
- **Postman** - Create a collection with all endpoints
- **Swagger/OpenAPI** - If your API has Swagger documentation
- **curl** - Command line testing

Example curl command:
```bash
curl -X GET "http://localhost:5000/api/equipment?pageNumber=1&pageSize=10&sortBy=equipmentName&sortDirection=ASC" \
  -H "Content-Type: application/json"
```

## Troubleshooting

### Common Issues

1. **CORS errors**: Ensure CORS is properly configured on the backend
2. **404 errors**: Verify the API URL in environment files
3. **Network errors**: Check if the backend is running
4. **Validation errors**: Ensure request body matches expected format

### Debug Mode

Enable detailed logging in the browser console:

```typescript
// In environment.ts
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5000/api',
  debug: true  // Add this
};
```

Then in services, you can add console logs for debugging:
```typescript
this.http.get<ApiResponse<Equipment[]>>(this.apiUrl).pipe(
  tap(response => {
    if (environment.debug) {
      console.log('API Response:', response);
    }
  })
)
```
