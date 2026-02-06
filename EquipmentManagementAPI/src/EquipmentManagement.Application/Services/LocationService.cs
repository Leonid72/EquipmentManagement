using EquipmentManagement.Application.DTOs;
using EquipmentManagement.Application.Interfaces;
using EquipmentManagement.Domain.Entities;
using EquipmentManagement.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipmentManagement.Application.Services
{
    public class LocationService : ILocationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<LocationService> _logger;
        public LocationService(IUnitOfWork unitOfWork, ILogger<LocationService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<ApiResponse<IEnumerable<LocationDto>>> GetAllLocationAsync()
        {
            try
            {
                var locations = await _unitOfWork.Locations.GetAllAsync();
                var locationsDto = locations.Select(MapToDto).ToList();
                return ApiResponse<IEnumerable<LocationDto>>.SuccessResponse(locationsDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching locations");
                return ApiResponse<IEnumerable<LocationDto>>.ErrorResponse(
                    "An error occurred while fetching locations",
                    new List<string> { ex.Message }
                );
            }

        }

        private static LocationDto MapToDto(Location locations)
        {
            return new LocationDto
            {
                LocationID = locations.LocationID,
                LocationName = locations.LocationName,
                Building = locations.Building,
                Floor = locations.Floor
            };
        }
    }
}
