using EquipmentManagement.Application.DTOs;
using EquipmentManagement.Application.Interfaces;
using EquipmentManagement.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EquipmentManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationsController : ControllerBase
    {
        private readonly ILocationService _locationService;
        private readonly ILogger<LocationsController> _logger;

        public LocationsController(ILocationService locationService, ILogger<LocationsController> logger)
        {
            this._locationService = locationService;
            _logger = logger;
        }


        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<LocationDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ApiResponse<IEnumerable<LocationDto>>>> GetAllLocations()
        {
            var result = await _locationService.GetAllLocationAsync();
            if (!result.Success)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, result);
            }

            return Ok(result);
        }
    }
}
