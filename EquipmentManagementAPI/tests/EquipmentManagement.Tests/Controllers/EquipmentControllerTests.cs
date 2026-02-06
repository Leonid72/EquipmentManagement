using EquipmentManagement.API.Controllers;
using EquipmentManagement.Application.DTOs;
using EquipmentManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EquipmentManagement.Tests.Controllers;

public class EquipmentControllerTests
{
    private readonly Mock<IEquipmentService> _mockService;
    private readonly Mock<ILogger<EquipmentsController>> _mockLogger;
    private readonly EquipmentsController _controller;

    public EquipmentControllerTests()
    {
        _mockService = new Mock<IEquipmentService>();
        _mockLogger = new Mock<ILogger<EquipmentsController>>();
        _controller = new EquipmentsController(_mockService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetAllEquipment_WithValidParameters_ReturnsOkResult()
    {
        // Arrange
        var searchDto = new EquipmentSearchDto
        {
            PageNumber = 1,
            PageSize = 10
        };

        var expectedResult = ApiResponse<PagedResult<EquipmentDto>>.SuccessResponse(
            new PagedResult<EquipmentDto>
            {
                Items = new List<EquipmentDto>
                {
                    new EquipmentDto
                    {
                        EquipmentID = 1,
                        EquipmentName = "Test Equipment",
                        SerialNumber = "TEST-001",
                        Status = "Active"
                    }
                },
                TotalCount = 1,
                PageNumber = 1,
                PageSize = 10
            });

        _mockService
            .Setup(s => s.GetAllEquipmentAsync(It.IsAny<EquipmentSearchDto>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.GetAllEquipment(searchDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<PagedResult<EquipmentDto>>>(okResult.Value);
        Assert.True(response.Success);
        Assert.Single(response.Data!.Items);
    }

    [Fact]
    public async Task GetAllEquipment_WithInvalidPageNumber_ReturnsBadRequest()
    {
        // Arrange
        var searchDto = new EquipmentSearchDto
        {
            PageNumber = 0,
            PageSize = 10
        };

        // Act
        var result = await _controller.GetAllEquipment(searchDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<PagedResult<EquipmentDto>>>(badRequestResult.Value);
        Assert.False(response.Success);
        Assert.Contains("Page number must be greater than 0", response.Message);
    }

    [Fact]
    public async Task GetEquipmentById_WithValidId_ReturnsOkResult()
    {
        // Arrange
        int equipmentId = 1;
        var expectedEquipment = new EquipmentDto
        {
            EquipmentID = equipmentId,
            EquipmentName = "Test Equipment",
            SerialNumber = "TEST-001",
            CategoryName = "Computers",
            Status = "Active"
        };

        var expectedResult = ApiResponse<EquipmentDto>.SuccessResponse(expectedEquipment);

        _mockService
            .Setup(s => s.GetEquipmentByIdAsync(equipmentId))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.GetEquipmentById(equipmentId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<EquipmentDto>>(okResult.Value);
        Assert.True(response.Success);
        Assert.Equal(equipmentId, response.Data!.EquipmentID);
    }

    [Fact]
    public async Task GetEquipmentById_WithInvalidId_ReturnsBadRequest()
    {
        // Arrange
        int invalidId = 0;

        // Act
        var result = await _controller.GetEquipmentById(invalidId);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<EquipmentDto>>(badRequestResult.Value);
        Assert.False(response.Success);
    }

    [Fact]
    public async Task GetEquipmentById_WithNonExistentId_ReturnsNotFound()
    {
        // Arrange
        int nonExistentId = 999;
        var expectedResult = ApiResponse<EquipmentDto>.ErrorResponse(
            $"Equipment with ID {nonExistentId} not found",
            new List<string> { "Equipment not found" });

        _mockService
            .Setup(s => s.GetEquipmentByIdAsync(nonExistentId))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.GetEquipmentById(nonExistentId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<EquipmentDto>>(notFoundResult.Value);
        Assert.False(response.Success);
    }

    [Fact]
    public async Task CreateEquipment_WithValidData_ReturnsCreatedResult()
    {
        // Arrange
        var createDto = new CreateEquipmentDto
        {
            EquipmentName = "New Equipment",
            SerialNumber = "NEW-001",
            CategoryID = 1,
            LocationID = 1,
            PurchaseDate = DateTime.Now.AddDays(-30),
            Status = "Active"
        };

        var createdEquipment = new EquipmentDto
        {
            EquipmentID = 1,
            EquipmentName = createDto.EquipmentName,
            SerialNumber = createDto.SerialNumber,
            CategoryID = createDto.CategoryID,
            LocationID = createDto.LocationID,
            PurchaseDate = createDto.PurchaseDate,
            Status = createDto.Status
        };

        var expectedResult = ApiResponse<EquipmentDto>.SuccessResponse(
            createdEquipment,
            "Equipment created successfully");

        _mockService
            .Setup(s => s.CreateEquipmentAsync(It.IsAny<CreateEquipmentDto>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.CreateEquipment(createDto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var response = Assert.IsType<ApiResponse<EquipmentDto>>(createdResult.Value);
        Assert.True(response.Success);
        Assert.Equal(createDto.EquipmentName, response.Data!.EquipmentName);
    }

    [Fact]
    public async Task CreateEquipment_WithFuturePurchaseDate_ReturnsBadRequest()
    {
        // Arrange
        var createDto = new CreateEquipmentDto
        {
            EquipmentName = "New Equipment",
            SerialNumber = "NEW-001",
            CategoryID = 1,
            LocationID = 1,
            PurchaseDate = DateTime.Now.AddDays(1), // Future date
            Status = "Active"
        };

        // Act
        var result = await _controller.CreateEquipment(createDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<EquipmentDto>>(badRequestResult.Value);
        Assert.False(response.Success);
        Assert.Contains("Purchase date cannot be in the future", response.Message);
    }

    [Fact]
    public async Task UpdateEquipment_WithValidData_ReturnsOkResult()
    {
        // Arrange
        var updateDto = new UpdateEquipmentDto
        {
            EquipmentID = 1,
            EquipmentName = "Updated Equipment",
            SerialNumber = "UPD-001",
            CategoryID = 1,
            LocationID = 1,
            PurchaseDate = DateTime.Now.AddDays(-30),
            Status = "Active"
        };

        var updatedEquipment = new EquipmentDto
        {
            EquipmentID = updateDto.EquipmentID,
            EquipmentName = updateDto.EquipmentName,
            SerialNumber = updateDto.SerialNumber,
            Status = updateDto.Status
        };

        var expectedResult = ApiResponse<EquipmentDto>.SuccessResponse(
            updatedEquipment,
            "Equipment updated successfully");

        _mockService
            .Setup(s => s.UpdateEquipmentAsync(It.IsAny<UpdateEquipmentDto>()))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.UpdateEquipment(updateDto.EquipmentID, updateDto);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<EquipmentDto>>(okResult.Value);
        Assert.True(response.Success);
    }

    [Fact]
    public async Task UpdateEquipment_WithMismatchedId_ReturnsBadRequest()
    {
        // Arrange
        var updateDto = new UpdateEquipmentDto
        {
            EquipmentID = 1,
            EquipmentName = "Updated Equipment",
            SerialNumber = "UPD-001",
            CategoryID = 1,
            LocationID = 1,
            PurchaseDate = DateTime.Now.AddDays(-30),
            Status = "Active"
        };

        int differentId = 2;

        // Act
        var result = await _controller.UpdateEquipment(differentId, updateDto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<EquipmentDto>>(badRequestResult.Value);
        Assert.False(response.Success);
        Assert.Contains("Equipment ID mismatch", response.Message);
    }

    [Fact]
    public async Task DeleteEquipment_WithValidId_ReturnsOkResult()
    {
        // Arrange
        int equipmentId = 1;
        var expectedResult = ApiResponse<bool>.SuccessResponse(true, "Equipment deleted successfully");

        _mockService
            .Setup(s => s.DeleteEquipmentAsync(equipmentId))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.DeleteEquipment(equipmentId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<bool>>(okResult.Value);
        Assert.True(response.Success);
    }

    [Fact]
    public async Task DeleteEquipment_WithNonExistentId_ReturnsNotFound()
    {
        // Arrange
        int nonExistentId = 999;
        var expectedResult = ApiResponse<bool>.ErrorResponse(
            $"Equipment with ID {nonExistentId} not found",
            new List<string> { "Equipment not found" });

        _mockService
            .Setup(s => s.DeleteEquipmentAsync(nonExistentId))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.DeleteEquipment(nonExistentId);

        // Assert
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<bool>>(notFoundResult.Value);
        Assert.False(response.Success);
    }

    [Fact]
    public async Task GetEquipmentByCategory_WithValidCategoryId_ReturnsOkResult()
    {
        // Arrange
        int categoryId = 1;
        var expectedEquipmentList = new List<EquipmentDto>
        {
            new EquipmentDto
            {
                EquipmentID = 1,
                EquipmentName = "Equipment 1",
                CategoryID = categoryId,
                CategoryName = "Computers"
            },
            new EquipmentDto
            {
                EquipmentID = 2,
                EquipmentName = "Equipment 2",
                CategoryID = categoryId,
                CategoryName = "Computers"
            }
        };

        var expectedResult = ApiResponse<IEnumerable<EquipmentDto>>.SuccessResponse(expectedEquipmentList);

        _mockService
            .Setup(s => s.GetEquipmentByCategoryAsync(categoryId))
            .ReturnsAsync(expectedResult);

        // Act
        var result = await _controller.GetEquipmentByCategory(categoryId);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<ApiResponse<IEnumerable<EquipmentDto>>>(okResult.Value);
        Assert.True(response.Success);
        Assert.Equal(2, response.Data!.Count());
    }
}
