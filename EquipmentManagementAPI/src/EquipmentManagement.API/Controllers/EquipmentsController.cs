using EquipmentManagement.Application.DTOs;
using EquipmentManagement.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace EquipmentManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class EquipmentsController : ControllerBase
{
    private readonly IEquipmentService _equipmentService;
    private readonly ILogger<EquipmentsController> _logger;

    public EquipmentsController(IEquipmentService equipmentService, ILogger<EquipmentsController> logger)
    {
        _equipmentService = equipmentService;
        _logger = logger;
    }

    /// <summary>
    /// Get all equipment with pagination, filtering, and sorting
    /// </summary>
    /// <param name="searchDto">Search and pagination parameters</param>
    /// <returns>Paginated list of equipment</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<PagedResult<EquipmentDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<PagedResult<EquipmentDto>>>> GetAllEquipment([FromQuery] EquipmentSearchDto searchDto)
    {
        _logger.LogInformation("GET api/equipment - Fetching equipment with filters");

        if (searchDto.PageNumber < 1)
        {
            return BadRequest(ApiResponse<PagedResult<EquipmentDto>>.ErrorResponse(
                "Page number must be greater than 0",
                new List<string> { "Invalid page number" }));
        }

        if (searchDto.PageSize < 1 || searchDto.PageSize > 100)
        {
            return BadRequest(ApiResponse<PagedResult<EquipmentDto>>.ErrorResponse(
                "Page size must be between 1 and 100",
                new List<string> { "Invalid page size" }));
        }

        var result = await _equipmentService.GetAllEquipmentAsync(searchDto);

        if (!result.Success)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Get equipment by ID
    /// </summary>
    /// <param name="id">Equipment ID</param>
    /// <returns>Equipment details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<EquipmentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<EquipmentDto>>> GetEquipmentById(int id)
    {
        _logger.LogInformation("GET api/equipment/{Id} - Fetching equipment by ID", id);

        if (id <= 0)
        {
            return BadRequest(ApiResponse<EquipmentDto>.ErrorResponse(
                "Invalid equipment ID",
                new List<string> { "Equipment ID must be greater than 0" }));
        }

        var result = await _equipmentService.GetEquipmentByIdAsync(id);

        if (!result.Success)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Search equipment by name, purchase date, and category
    /// </summary>
    /// <param name="searchDto">Search criteria</param>
    /// <returns>List of matching equipment</returns>
    [HttpGet("search")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<EquipmentDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<IEnumerable<EquipmentDto>>>> SearchEquipment([FromQuery] EquipmentSearchDto searchDto)
    {
        _logger.LogInformation("GET api/equipment/search - Searching equipment");

        if (searchDto.PurchaseDateFrom.HasValue && searchDto.PurchaseDateTo.HasValue &&
            searchDto.PurchaseDateFrom > searchDto.PurchaseDateTo)
        {
            return BadRequest(ApiResponse<IEnumerable<EquipmentDto>>.ErrorResponse(
                "Purchase date 'from' cannot be after 'to'",
                new List<string> { "Invalid date range" }));
        }

        var result = await _equipmentService.SearchEquipmentAsync(searchDto);

        if (!result.Success)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Create new equipment
    /// </summary>
    /// <param name="createDto">Equipment creation data</param>
    /// <returns>Created equipment</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<EquipmentDto>), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<EquipmentDto>>> CreateEquipment([FromBody] CreateEquipmentDto createDto)
    {
        _logger.LogInformation("POST api/equipment - Creating new equipment");

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return BadRequest(ApiResponse<EquipmentDto>.ErrorResponse(
                "Validation failed",
                errors));
        }

        if (createDto.PurchaseDate > DateTime.Now)
        {
            return BadRequest(ApiResponse<EquipmentDto>.ErrorResponse(
                "Purchase date cannot be in the future",
                new List<string> { "Invalid purchase date" }));
        }

        var result = await _equipmentService.CreateEquipmentAsync(createDto);

        if (!result.Success)
        {
            return BadRequest(result);
        }

        return CreatedAtAction(
            nameof(GetEquipmentById),
            new { id = result.Data!.EquipmentID },
            result);
    }

    /// <summary>
    /// Update existing equipment
    /// </summary>
    /// <param name="id">Equipment ID</param>
    /// <param name="updateDto">Equipment update data</param>
    /// <returns>Updated equipment</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(ApiResponse<EquipmentDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<EquipmentDto>>> UpdateEquipment(int id, [FromBody] UpdateEquipmentDto updateDto)
    {
        _logger.LogInformation("PUT api/equipment/{Id} - Updating equipment", id);

        if (id != updateDto.EquipmentID)
        {
            return BadRequest(ApiResponse<EquipmentDto>.ErrorResponse(
                "Equipment ID mismatch",
                new List<string> { "ID in URL does not match ID in request body" }));
        }

        if (!ModelState.IsValid)
        {
            var errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return BadRequest(ApiResponse<EquipmentDto>.ErrorResponse(
                "Validation failed",
                errors));
        }

        if (updateDto.PurchaseDate > DateTime.Now)
        {
            return BadRequest(ApiResponse<EquipmentDto>.ErrorResponse(
                "Purchase date cannot be in the future",
                new List<string> { "Invalid purchase date" }));
        }

        var result = await _equipmentService.UpdateEquipmentAsync(updateDto);

        if (!result.Success)
        {
            if (result.Errors.Any(e => e.Contains("not found")))
            {
                return NotFound(result);
            }
            return BadRequest(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Delete equipment
    /// </summary>
    /// <param name="id">Equipment ID</param>
    /// <returns>Success status</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse<bool>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<bool>>> DeleteEquipment(int id)
    {
        _logger.LogInformation("DELETE api/equipment/{Id} - Deleting equipment", id);

        if (id <= 0)
        {
            return BadRequest(ApiResponse<bool>.ErrorResponse(
                "Invalid equipment ID",
                new List<string> { "Equipment ID must be greater than 0" }));
        }

        var result = await _equipmentService.DeleteEquipmentAsync(id);

        if (!result.Success)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Get equipment by category ID
    /// </summary>
    /// <param name="categoryId">Category ID</param>
    /// <returns>List of equipment in the category</returns>
    [HttpGet("category/{categoryId}")]
    [ProducesResponseType(typeof(ApiResponse<IEnumerable<EquipmentDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<IEnumerable<EquipmentDto>>>> GetEquipmentByCategory(int categoryId)
    {
        _logger.LogInformation("GET api/equipment/category/{CategoryId} - Fetching equipment by category", categoryId);

        if (categoryId <= 0)
        {
            return BadRequest(ApiResponse<IEnumerable<EquipmentDto>>.ErrorResponse(
                "Invalid category ID",
                new List<string> { "Category ID must be greater than 0" }));
        }

        var result = await _equipmentService.GetEquipmentByCategoryAsync(categoryId);

        if (!result.Success)
        {
            return NotFound(result);
        }

        return Ok(result);
    }
}
