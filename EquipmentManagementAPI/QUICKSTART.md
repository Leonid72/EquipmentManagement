# Equipment Management API - Quick Start Guide

## 🚀 5-Minute Setup

### Prerequisites Check
- [ ] .NET 8.0 SDK installed
- [ ] SQL Server running (LocalDB, Express, or full version)
- [ ] Your favorite code editor (VS Code, Visual Studio 2022, or Rider)

### Step 1: Database Setup (2 minutes)

**Option A: Automated (Recommended)**
1. Open SQL Server Management Studio (SSMS)
2. Connect to your SQL Server
3. Open file: `scripts/CreateDatabase.sql`
4. Press F5 to execute
5. ✅ Done! Database with sample data is ready

**Option B: Connection String First**
If you're using LocalDB or a specific server:
1. Open `src/EquipmentManagement.API/appsettings.json`
2. Update the connection string:
```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=EquipmentManagementDB;Trusted_Connection=True;"
}
```

### Step 2: Build & Run (2 minutes)

```bash
# Navigate to solution directory
cd EquipmentManagementAPI

# Restore packages
dotnet restore

# Build the solution
dotnet build

# Run the API
cd src/EquipmentManagement.API
dotnet run
```

### Step 3: Test the API (1 minute)

1. Open your browser
2. Navigate to: `https://localhost:5001`
3. You'll see Swagger UI automatically!
4. Try the "GET /api/equipment" endpoint
5. Click "Try it out" → "Execute"
6. ✅ You should see sample data!

## 🎯 Quick API Test Scenarios

### Scenario 1: View All Equipment
```
GET https://localhost:5001/api/equipment?pageNumber=1&pageSize=5
```

### Scenario 2: Create New Equipment
```
POST https://localhost:5001/api/equipment
Content-Type: application/json

{
  "equipmentName": "MacBook Pro 16",
  "serialNumber": "MBP-2024-001",
  "categoryID": 1,
  "locationID": 1,
  "purchaseDate": "2024-02-05",
  "status": "Active"
}
```

### Scenario 3: Search Equipment
```
GET https://localhost:5001/api/equipment/search?searchTerm=Dell&categoryId=1
```

### Scenario 4: Get Equipment by Category
```
GET https://localhost:5001/api/equipment/category/1
```

## 📊 Sample Data Overview

After running the database script, you'll have:
- **6 Categories**: Computers, Network Equipment, Printers, Mobile Devices, Furniture, Audio/Visual
- **6 Locations**: Across different buildings and floors
- **10 Equipment Items**: Mix of computers, printers, phones, and furniture

## 🧪 Running Tests

```bash
# Run all tests
cd tests/EquipmentManagement.Tests
dotnet test

# Run with detailed output
dotnet test --logger "console;verbosity=detailed"
```

## 🔍 Using Swagger UI

Swagger UI is available at the root URL: `https://localhost:5001`

**Features:**
- 📖 Complete API documentation
- ▶️ Try out endpoints directly
- 📋 See request/response examples
- 🔧 Test with different parameters

## 📝 Common Configuration Changes

### Change Database Connection
Edit `appsettings.json`:
```json
"ConnectionStrings": {
  "DefaultConnection": "YOUR_CONNECTION_STRING_HERE"
}
```

### Change Logging Level
Edit `appsettings.json`:
```json
"Serilog": {
  "MinimumLevel": {
    "Default": "Debug"  // Change from Information to Debug
  }
}
```

### Enable CORS for Specific Origins
Edit `Program.cs`:
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecific", policy =>
    {
        policy.WithOrigins("https://yourfrontend.com")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
```

## 🐛 Troubleshooting

### Issue: "Cannot connect to database"
**Solution:** 
1. Verify SQL Server is running
2. Check connection string in `appsettings.json`
3. Ensure database exists (run CreateDatabase.sql)

### Issue: "Port already in use"
**Solution:** 
Edit `launchSettings.json` to use different ports:
```json
"applicationUrl": "https://localhost:7001;http://localhost:5000"
```

### Issue: Build errors
**Solution:** 
```bash
# Clean and rebuild
dotnet clean
dotnet restore
dotnet build
```

## 📚 Next Steps

1. ✅ **Explore the API** using Swagger UI
2. ✅ **Import Postman Collection** (`EquipmentManagement.postman_collection.json`)
3. ✅ **Review the code structure** in the README.md
4. ✅ **Run the unit tests** to see test coverage
5. ✅ **Check the logs** in the `logs/` folder

## 🎓 Learning Resources

- **Clean Architecture**: Review the project structure
- **Repository Pattern**: Check `Infrastructure/Repositories`
- **DTOs**: See `Application/DTOs`
- **Error Handling**: Review `Middleware/GlobalExceptionHandlerMiddleware.cs`
- **Unit Testing**: Explore `tests/EquipmentManagement.Tests`

## 💡 Tips

- Use Swagger UI for quick testing
- Check logs in `logs/` folder for debugging
- All endpoints return consistent JSON responses
- Validation errors are detailed and helpful
- Use pagination for large datasets

## 📞 Need Help?

- Check the main `README.md` for detailed documentation
- Review code comments for implementation details
- Check logs for error details
- All endpoints have XML documentation in Swagger

---

**Happy Coding! 🚀**

Built with ❤️ using .NET 8.0 and Clean Architecture
