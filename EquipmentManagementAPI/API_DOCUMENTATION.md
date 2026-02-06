# Equipment Management API Documentation

## Base URL
```
https://localhost:5001/api
```

## Response Format

All endpoints return responses in the following format:

```json
{
  "success": true,
  "message": "Operation successful",
  "data": { /* Response data */ },
  "errors": []
}
```

## HTTP Status Codes

| Status Code | Description |
|-------------|-------------|
| 200 OK | Request successful |
| 201 Created | Resource created successfully |
| 400 Bad Request | Invalid request data |
| 404 Not Found | Resource not found |
| 500 Internal Server Error | Server error |

---

## Endpoints

### 1. Get All Equipment (Paginated)

Retrieves a paginated list of equipment with optional filtering and sorting.

**Endpoint:** `GET /api/equipment`

**Query Parameters:**

| Parameter | Type | Required | Default | Description |
|-----------|------|----------|---------|-------------|
| pageNumber | integer | No | 1 | Page number |
| pageSize | integer | No | 10 | Items per page (max 100) |
| searchTerm | string | No | - | Search in name or serial number |
| categoryId | integer | No | - | Filter by category ID |
| status | string | No | - | Filter by status (Active, InMaintenance, OutOfService, Retired) |
| sortBy | string | No | EquipmentName | Sort field (EquipmentName, PurchaseDate, Status, CategoryName) |
| sortDirection | string | No | ASC | Sort direction (ASC, DESC) |

**Example Request:**
```http
GET /api/equipment?pageNumber=1&pageSize=10&searchTerm=Dell&categoryId=1&status=Active&sortBy=EquipmentName&sortDirection=ASC
```

**Example Response:**
```json
{
  "success": true,
  "message": "Operation successful",
  "data": {
    "items": [
      {
        "equipmentID": 1,
        "equipmentName": "Dell Latitude 5520",
        "serialNumber": "DL-2023-001",
        "categoryID": 1,
        "categoryName": "Computers",
        "locationID": 1,
        "locationName": "IT Department",
        "building": "Main Building",
        "floor": "3rd Floor",
        "purchaseDate": "2023-01-15",
        "status": "Active"
      }
    ],
    "totalCount": 1,
    "pageNumber": 1,
    "pageSize": 10,
    "totalPages": 1,
    "hasPreviousPage": false,
    "hasNextPage": false
  },
  "errors": []
}
```

---

### 2. Get Equipment By ID

Retrieves a single equipment item by its ID.

**Endpoint:** `GET /api/equipment/{id}`

**Path Parameters:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| id | integer | Yes | Equipment ID |

**Example Request:**
```http
GET /api/equipment/1
```

**Example Response:**
```json
{
  "success": true,
  "message": "Operation successful",
  "data": {
    "equipmentID": 1,
    "equipmentName": "Dell Latitude 5520",
    "serialNumber": "DL-2023-001",
    "categoryID": 1,
    "categoryName": "Computers",
    "locationID": 1,
    "locationName": "IT Department",
    "building": "Main Building",
    "floor": "3rd Floor",
    "purchaseDate": "2023-01-15",
    "status": "Active"
  },
  "errors": []
}
```

**Error Response (404):**
```json
{
  "success": false,
  "message": "Equipment with ID 999 not found",
  "data": null,
  "errors": ["Equipment not found"]
}
```

---

### 3. Search Equipment

Advanced search with multiple criteria including date range.

**Endpoint:** `GET /api/equipment/search`

**Query Parameters:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| searchTerm | string | No | Search in name or serial number |
| categoryId | integer | No | Filter by category ID |
| purchaseDateFrom | date | No | Start of date range (YYYY-MM-DD) |
| purchaseDateTo | date | No | End of date range (YYYY-MM-DD) |
| status | string | No | Filter by status |

**Example Request:**
```http
GET /api/equipment/search?searchTerm=Dell&categoryId=1&purchaseDateFrom=2023-01-01&purchaseDateTo=2023-12-31&status=Active
```

**Example Response:**
```json
{
  "success": true,
  "message": "Operation successful",
  "data": [
    {
      "equipmentID": 1,
      "equipmentName": "Dell Latitude 5520",
      "serialNumber": "DL-2023-001",
      "categoryID": 1,
      "categoryName": "Computers",
      "locationID": 1,
      "locationName": "IT Department",
      "building": "Main Building",
      "floor": "3rd Floor",
      "purchaseDate": "2023-01-15",
      "status": "Active"
    }
  ],
  "errors": []
}
```

---

### 4. Create Equipment

Creates a new equipment item.

**Endpoint:** `POST /api/equipment`

**Request Body:**

```json
{
  "equipmentName": "string (3-200 chars, required)",
  "serialNumber": "string (5-100 chars, required, unique)",
  "categoryID": "integer (required, must exist)",
  "locationID": "integer (required, must exist)",
  "purchaseDate": "date (required, YYYY-MM-DD, cannot be future)",
  "status": "string (required, Active|InMaintenance|OutOfService|Retired)"
}
```

**Example Request:**
```http
POST /api/equipment
Content-Type: application/json

{
  "equipmentName": "Dell Latitude 7420",
  "serialNumber": "DL-2024-NEW-001",
  "categoryID": 1,
  "locationID": 1,
  "purchaseDate": "2024-02-01",
  "status": "Active"
}
```

**Example Response (201 Created):**
```json
{
  "success": true,
  "message": "Equipment created successfully",
  "data": {
    "equipmentID": 11,
    "equipmentName": "Dell Latitude 7420",
    "serialNumber": "DL-2024-NEW-001",
    "categoryID": 1,
    "categoryName": "Computers",
    "locationID": 1,
    "locationName": "IT Department",
    "building": "Main Building",
    "floor": "3rd Floor",
    "purchaseDate": "2024-02-01",
    "status": "Active"
  },
  "errors": []
}
```

**Validation Error Response (400):**
```json
{
  "success": false,
  "message": "Validation failed",
  "data": null,
  "errors": [
    "Equipment name must be between 3 and 200 characters",
    "Serial number is required"
  ]
}
```

**Business Logic Error (400):**
```json
{
  "success": false,
  "message": "Serial number already exists",
  "data": null,
  "errors": [
    "Equipment with serial number 'DL-2024-NEW-001' already exists"
  ]
}
```

---

### 5. Update Equipment

Updates an existing equipment item.

**Endpoint:** `PUT /api/equipment/{id}`

**Path Parameters:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| id | integer | Yes | Equipment ID (must match request body) |

**Request Body:**

```json
{
  "equipmentID": "integer (required, must match path parameter)",
  "equipmentName": "string (3-200 chars, required)",
  "serialNumber": "string (5-100 chars, required, unique)",
  "categoryID": "integer (required, must exist)",
  "locationID": "integer (required, must exist)",
  "purchaseDate": "date (required, YYYY-MM-DD, cannot be future)",
  "status": "string (required, Active|InMaintenance|OutOfService|Retired)"
}
```

**Example Request:**
```http
PUT /api/equipment/1
Content-Type: application/json

{
  "equipmentID": 1,
  "equipmentName": "Dell Latitude 5520 - Updated",
  "serialNumber": "DL-2023-001",
  "categoryID": 1,
  "locationID": 2,
  "purchaseDate": "2023-01-15",
  "status": "InMaintenance"
}
```

**Example Response (200 OK):**
```json
{
  "success": true,
  "message": "Equipment updated successfully",
  "data": {
    "equipmentID": 1,
    "equipmentName": "Dell Latitude 5520 - Updated",
    "serialNumber": "DL-2023-001",
    "categoryID": 1,
    "categoryName": "Computers",
    "locationID": 2,
    "locationName": "HR Department",
    "building": "Main Building",
    "floor": "2nd Floor",
    "purchaseDate": "2023-01-15",
    "status": "InMaintenance"
  },
  "errors": []
}
```

**ID Mismatch Error (400):**
```json
{
  "success": false,
  "message": "Equipment ID mismatch",
  "data": null,
  "errors": [
    "ID in URL does not match ID in request body"
  ]
}
```

---

### 6. Delete Equipment

Deletes an equipment item.

**Endpoint:** `DELETE /api/equipment/{id}`

**Path Parameters:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| id | integer | Yes | Equipment ID |

**Example Request:**
```http
DELETE /api/equipment/1
```

**Example Response (200 OK):**
```json
{
  "success": true,
  "message": "Equipment deleted successfully",
  "data": true,
  "errors": []
}
```

**Not Found Error (404):**
```json
{
  "success": false,
  "message": "Equipment with ID 999 not found",
  "data": false,
  "errors": ["Equipment not found"]
}
```

---

### 7. Get Equipment By Category

Retrieves all equipment items belonging to a specific category.

**Endpoint:** `GET /api/equipment/category/{categoryId}`

**Path Parameters:**

| Parameter | Type | Required | Description |
|-----------|------|----------|-------------|
| categoryId | integer | Yes | Category ID |

**Example Request:**
```http
GET /api/equipment/category/1
```

**Example Response:**
```json
{
  "success": true,
  "message": "Operation successful",
  "data": [
    {
      "equipmentID": 1,
      "equipmentName": "Dell Latitude 5520",
      "serialNumber": "DL-2023-001",
      "categoryID": 1,
      "categoryName": "Computers",
      "locationID": 1,
      "locationName": "IT Department",
      "building": "Main Building",
      "floor": "3rd Floor",
      "purchaseDate": "2023-01-15",
      "status": "Active"
    },
    {
      "equipmentID": 6,
      "equipmentName": "Lenovo ThinkPad X1",
      "serialNumber": "LT-2021-006",
      "categoryID": 1,
      "categoryName": "Computers",
      "locationID": 3,
      "locationName": "Finance Department",
      "building": "East Wing",
      "floor": "1st Floor",
      "purchaseDate": "2021-08-22",
      "status": "InMaintenance"
    }
  ],
  "errors": []
}
```

---

## Data Models

### Equipment Status Enum

Valid values:
- `Active`
- `InMaintenance`
- `OutOfService`
- `Retired`

### Category IDs (Sample Data)

| ID | Category Name | Description |
|----|---------------|-------------|
| 1 | Computers | Desktop computers, laptops, and tablets |
| 2 | Network Equipment | Routers, switches, and network infrastructure |
| 3 | Printers | Laser and inkjet printers |
| 4 | Mobile Devices | Smartphones and tablets |
| 5 | Furniture | Office desks, chairs, and cabinets |
| 6 | Audio/Visual | Projectors, monitors, and audio equipment |

### Location IDs (Sample Data)

| ID | Location Name | Building | Floor |
|----|---------------|----------|-------|
| 1 | IT Department | Main Building | 3rd Floor |
| 2 | HR Department | Main Building | 2nd Floor |
| 3 | Finance Department | East Wing | 1st Floor |
| 4 | Conference Room A | Main Building | 1st Floor |
| 5 | Conference Room B | East Wing | 2nd Floor |
| 6 | Storage Room | West Wing | Basement |

---

## Error Handling

### Validation Errors

All validation errors return a 400 Bad Request with detailed error messages:

```json
{
  "success": false,
  "message": "Validation failed",
  "data": null,
  "errors": [
    "Equipment name must be between 3 and 200 characters",
    "Status must be Active, InMaintenance, OutOfService, or Retired"
  ]
}
```

### Business Logic Errors

Business rule violations (e.g., duplicate serial number) return 400 Bad Request:

```json
{
  "success": false,
  "message": "Serial number already exists",
  "data": null,
  "errors": [
    "Equipment with serial number 'DL-2024-001' already exists"
  ]
}
```

### Not Found Errors

Resource not found returns 404 Not Found:

```json
{
  "success": false,
  "message": "Equipment with ID 999 not found",
  "data": null,
  "errors": ["Equipment not found"]
}
```

### Server Errors

Internal server errors return 500 Internal Server Error:

```json
{
  "success": false,
  "message": "An internal server error occurred. Please try again later.",
  "data": null,
  "errors": ["Error details"]
}
```

---

## Best Practices

### Pagination
- Always use pagination for list endpoints
- Keep page size reasonable (10-50 items)
- Maximum page size is 100

### Filtering
- Combine multiple filters for precise results
- Use date ranges for temporal queries
- Status filtering for workflow management

### Sorting
- Sort by relevant fields (name, date, status)
- Use ascending order for names
- Use descending order for dates

### Error Handling
- Always check the `success` field
- Display `message` to users
- Log `errors` array for debugging

### Performance
- Use pagination to reduce data transfer
- Filter at the API level, not client-side
- Cache frequently accessed data

---

## Rate Limiting

Currently, no rate limiting is implemented. For production use, consider:
- Request rate limits per IP/user
- Throttling for expensive operations
- API key authentication

---

## Security Considerations

### Input Validation
- All inputs are validated
- SQL injection prevention via Entity Framework
- XSS prevention via JSON serialization

### Future Enhancements
- JWT authentication
- Role-based authorization
- API key management
- Request signing

---

## Support

For issues or questions:
- Check Swagger UI at `https://localhost:5001`
- Review logs in `logs/` directory
- Create an issue in the repository
