using EquipmentManagement.Application.DTOs;
using EquipmentManagement.Application.Interfaces;
using EquipmentManagement.Domain.Entities;
using EquipmentManagement.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace EquipmentManagement.Application.Services;

public class EquipmentService : IEquipmentService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<EquipmentService> _logger;

    public EquipmentService(IUnitOfWork unitOfWork, ILogger<EquipmentService> logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ApiResponse<PagedResult<EquipmentDto>>> GetAllEquipmentAsync(EquipmentSearchDto searchDto)
    {
        try
        {
            _logger.LogInformation("Fetching equipment with pagination. Page: {PageNumber}, Size: {PageSize}", 
                searchDto.PageNumber, searchDto.PageSize);

            EquipmentStatus? status = null;
            if (!string.IsNullOrEmpty(searchDto.Status) && 
                Enum.TryParse<EquipmentStatus>(searchDto.Status, out var parsedStatus))
            {
                status = parsedStatus;
            }

            var (items, totalCount) = await _unitOfWork.Equipment.GetPagedAsync(
                searchDto.PageNumber,
                searchDto.PageSize,
                searchDto.SearchTerm,
                searchDto.CategoryID,
                status,
                searchDto.SortBy,
                searchDto.SortDirection
            );

            var equipmentDtos = items.Select(MapToDto).ToList();

            var pagedResult = new PagedResult<EquipmentDto>
            {
                Items = equipmentDtos,
                TotalCount = totalCount,
                PageNumber = searchDto.PageNumber,
                PageSize = searchDto.PageSize
            };

            _logger.LogInformation("Successfully fetched {Count} equipment items out of {Total}", 
                equipmentDtos.Count, totalCount);

            return ApiResponse<PagedResult<EquipmentDto>>.SuccessResponse(pagedResult);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching equipment");
            return ApiResponse<PagedResult<EquipmentDto>>.ErrorResponse(
                "An error occurred while fetching equipment",
                new List<string> { ex.Message }
            );
        }
    }

    public async Task<ApiResponse<EquipmentDto>> GetEquipmentByIdAsync(int id)
    {
        try
        {
            _logger.LogInformation("Fetching equipment with ID: {EquipmentId}", id);

            var equipment = await _unitOfWork.Equipment.GetByIdAsync(id);

            if (equipment == null)
            {
                _logger.LogWarning("Equipment with ID {EquipmentId} not found", id);
                return ApiResponse<EquipmentDto>.ErrorResponse(
                    $"Equipment with ID {id} not found",
                    new List<string> { "Equipment not found" }
                );
            }

            var equipmentDto = MapToDto(equipment);

            _logger.LogInformation("Successfully fetched equipment with ID: {EquipmentId}", id);

            return ApiResponse<EquipmentDto>.SuccessResponse(equipmentDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching equipment with ID: {EquipmentId}", id);
            return ApiResponse<EquipmentDto>.ErrorResponse(
                "An error occurred while fetching equipment",
                new List<string> { ex.Message }
            );
        }
    }

    public async Task<ApiResponse<IEnumerable<EquipmentDto>>> SearchEquipmentAsync(EquipmentSearchDto searchDto)
    {
        try
        {
            _logger.LogInformation("Searching equipment with criteria: {SearchTerm}", searchDto.SearchTerm);

            var equipmentList = await _unitOfWork.Equipment.FindAsync(e =>
                (string.IsNullOrEmpty(searchDto.SearchTerm) ||
                 e.EquipmentName.Contains(searchDto.SearchTerm) ||
                 e.SerialNumber.Contains(searchDto.SearchTerm)) &&
                (!searchDto.CategoryID.HasValue || e.CategoryID == searchDto.CategoryID.Value) &&
                (!searchDto.PurchaseDateFrom.HasValue || e.PurchaseDate >= searchDto.PurchaseDateFrom.Value) &&
                (!searchDto.PurchaseDateTo.HasValue || e.PurchaseDate <= searchDto.PurchaseDateTo.Value) &&
                (string.IsNullOrEmpty(searchDto.Status) || e.Status.ToString() == searchDto.Status)
            );

            var equipmentDtos = equipmentList.Select(MapToDto).ToList();

            _logger.LogInformation("Found {Count} equipment items matching search criteria", equipmentDtos.Count);

            return ApiResponse<IEnumerable<EquipmentDto>>.SuccessResponse(equipmentDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while searching equipment");
            return ApiResponse<IEnumerable<EquipmentDto>>.ErrorResponse(
                "An error occurred while searching equipment",
                new List<string> { ex.Message }
            );
        }
    }

    public async Task<ApiResponse<EquipmentDto>> CreateEquipmentAsync(CreateEquipmentDto createDto)
    {
        try
        {
            _logger.LogInformation("Creating new equipment: {EquipmentName}", createDto.EquipmentName);

            // Check if serial number already exists
            var serialExists = await _unitOfWork.Equipment.SerialNumberExistsAsync(createDto.SerialNumber);
            if (serialExists)
            {
                _logger.LogWarning("Serial number {SerialNumber} already exists", createDto.SerialNumber);
                return ApiResponse<EquipmentDto>.ErrorResponse(
                    "Serial number already exists",
                    new List<string> { $"Equipment with serial number '{createDto.SerialNumber}' already exists" }
                );
            }

            // Verify category exists
            var category = await _unitOfWork.Categories.GetByIdAsync(createDto.CategoryID);
            if (category == null)
            {
                _logger.LogWarning("Category with ID {CategoryId} not found", createDto.CategoryID);
                return ApiResponse<EquipmentDto>.ErrorResponse(
                    "Invalid category",
                    new List<string> { $"Category with ID {createDto.CategoryID} does not exist" }
                );
            }

            // Verify location exists
            var location = await _unitOfWork.Locations.GetByIdAsync(createDto.LocationID);
            if (location == null)
            {
                _logger.LogWarning("Location with ID {LocationId} not found", createDto.LocationID);
                return ApiResponse<EquipmentDto>.ErrorResponse(
                    "Invalid location",
                    new List<string> { $"Location with ID {createDto.LocationID} does not exist" }
                );
            }

            var equipment = new Equipment
            {
                EquipmentName = createDto.EquipmentName,
                SerialNumber = createDto.SerialNumber,
                CategoryID = createDto.CategoryID,
                LocationID = createDto.LocationID,
                PurchaseDate = createDto.PurchaseDate,
                Status = Enum.Parse<EquipmentStatus>(createDto.Status),
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow
            };

            var createdEquipment = await _unitOfWork.Equipment.AddAsync(equipment);
            await _unitOfWork.SaveChangesAsync();

            // Reload with navigation properties
            var equipmentWithRelations = await _unitOfWork.Equipment.GetByIdAsync(createdEquipment.EquipmentID);
            var equipmentDto = MapToDto(equipmentWithRelations!);

            _logger.LogInformation("Successfully created equipment with ID: {EquipmentId}", createdEquipment.EquipmentID);

            return ApiResponse<EquipmentDto>.SuccessResponse(equipmentDto, "Equipment created successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while creating equipment");
            return ApiResponse<EquipmentDto>.ErrorResponse(
                "An error occurred while creating equipment",
                new List<string> { ex.Message }
            );
        }
    }

    public async Task<ApiResponse<EquipmentDto>> UpdateEquipmentAsync(UpdateEquipmentDto updateDto)
    {
        try
        {
            _logger.LogInformation("Updating equipment with ID: {EquipmentId}", updateDto.EquipmentID);

            var equipment = await _unitOfWork.Equipment.GetByIdAsync(updateDto.EquipmentID);
            if (equipment == null)
            {
                _logger.LogWarning("Equipment with ID {EquipmentId} not found", updateDto.EquipmentID);
                return ApiResponse<EquipmentDto>.ErrorResponse(
                    $"Equipment with ID {updateDto.EquipmentID} not found",
                    new List<string> { "Equipment not found" }
                );
            }

            // Check if serial number is being changed and if it's already in use
            if (equipment.SerialNumber != updateDto.SerialNumber)
            {
                var serialExists = await _unitOfWork.Equipment.SerialNumberExistsAsync(
                    updateDto.SerialNumber, 
                    updateDto.EquipmentID);
                
                if (serialExists)
                {
                    _logger.LogWarning("Serial number {SerialNumber} already exists", updateDto.SerialNumber);
                    return ApiResponse<EquipmentDto>.ErrorResponse(
                        "Serial number already exists",
                        new List<string> { $"Equipment with serial number '{updateDto.SerialNumber}' already exists" }
                    );
                }
            }

            // Verify category exists
            var category = await _unitOfWork.Categories.GetByIdAsync(updateDto.CategoryID);
            if (category == null)
            {
                _logger.LogWarning("Category with ID {CategoryId} not found", updateDto.CategoryID);
                return ApiResponse<EquipmentDto>.ErrorResponse(
                    "Invalid category",
                    new List<string> { $"Category with ID {updateDto.CategoryID} does not exist" }
                );
            }

            // Verify location exists
            var location = await _unitOfWork.Locations.GetByIdAsync(updateDto.LocationID);
            if (location == null)
            {
                _logger.LogWarning("Location with ID {LocationId} not found", updateDto.LocationID);
                return ApiResponse<EquipmentDto>.ErrorResponse(
                    "Invalid location",
                    new List<string> { $"Location with ID {updateDto.LocationID} does not exist" }
                );
            }

            equipment.EquipmentName = updateDto.EquipmentName;
            equipment.SerialNumber = updateDto.SerialNumber;
            equipment.CategoryID = updateDto.CategoryID;
            equipment.LocationID = updateDto.LocationID;
            equipment.PurchaseDate = updateDto.PurchaseDate;
            equipment.Status = Enum.Parse<EquipmentStatus>(updateDto.Status);
            equipment.ModifiedDate = DateTime.UtcNow;

            await _unitOfWork.Equipment.UpdateAsync(equipment);
            await _unitOfWork.SaveChangesAsync();

            // Reload with navigation properties
            var updatedEquipment = await _unitOfWork.Equipment.GetByIdAsync(equipment.EquipmentID);
            var equipmentDto = MapToDto(updatedEquipment!);

            _logger.LogInformation("Successfully updated equipment with ID: {EquipmentId}", updateDto.EquipmentID);

            return ApiResponse<EquipmentDto>.SuccessResponse(equipmentDto, "Equipment updated successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while updating equipment with ID: {EquipmentId}", updateDto.EquipmentID);
            return ApiResponse<EquipmentDto>.ErrorResponse(
                "An error occurred while updating equipment",
                new List<string> { ex.Message }
            );
        }
    }

    public async Task<ApiResponse<bool>> DeleteEquipmentAsync(int id)
    {
        try
        {
            _logger.LogInformation("Deleting equipment with ID: {EquipmentId}", id);

            var equipment = await _unitOfWork.Equipment.GetByIdAsync(id);
            if (equipment == null)
            {
                _logger.LogWarning("Equipment with ID {EquipmentId} not found", id);
                return ApiResponse<bool>.ErrorResponse(
                    $"Equipment with ID {id} not found",
                    new List<string> { "Equipment not found" }
                );
            }

            await _unitOfWork.Equipment.DeleteAsync(equipment);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Successfully deleted equipment with ID: {EquipmentId}", id);

            return ApiResponse<bool>.SuccessResponse(true, "Equipment deleted successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while deleting equipment with ID: {EquipmentId}", id);
            return ApiResponse<bool>.ErrorResponse(
                "An error occurred while deleting equipment",
                new List<string> { ex.Message }
            );
        }
    }

    public async Task<ApiResponse<IEnumerable<EquipmentDto>>> GetEquipmentByCategoryAsync(int categoryId)
    {
        try
        {
            _logger.LogInformation("Fetching equipment for category ID: {CategoryId}", categoryId);

            var category = await _unitOfWork.Categories.GetByIdAsync(categoryId);
            if (category == null)
            {
                _logger.LogWarning("Category with ID {CategoryId} not found", categoryId);
                return ApiResponse<IEnumerable<EquipmentDto>>.ErrorResponse(
                    $"Category with ID {categoryId} not found",
                    new List<string> { "Category not found" }
                );
            }

            var equipmentList = await _unitOfWork.Equipment.GetByCategoryAsync(categoryId);
            var equipmentDtos = equipmentList.Select(MapToDto).ToList();

            _logger.LogInformation("Found {Count} equipment items for category ID: {CategoryId}", 
                equipmentDtos.Count, categoryId);

            return ApiResponse<IEnumerable<EquipmentDto>>.SuccessResponse(equipmentDtos);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching equipment for category ID: {CategoryId}", categoryId);
            return ApiResponse<IEnumerable<EquipmentDto>>.ErrorResponse(
                "An error occurred while fetching equipment by category",
                new List<string> { ex.Message }
            );
        }
    }

    private static EquipmentDto MapToDto(Equipment equipment)
    {
        return new EquipmentDto
        {
            EquipmentID = equipment.EquipmentID,
            EquipmentName = equipment.EquipmentName,
            SerialNumber = equipment.SerialNumber,
            CategoryID = equipment.CategoryID,
            CategoryName = equipment.Category?.CategoryName ?? string.Empty,
            LocationID = equipment.LocationID,
            LocationName = equipment.Location?.LocationName ?? string.Empty,
            Building = equipment.Location?.Building ?? string.Empty,
            Floor = equipment.Location?.Floor ?? string.Empty,
            PurchaseDate = equipment.PurchaseDate,
            Status = equipment.Status.ToString()
        };
    }
}
