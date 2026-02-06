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
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CategoryService> _logger;
        public CategoryService(IUnitOfWork unitOfWork, ILogger<CategoryService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<ApiResponse<IEnumerable<CategoryDto>>> GetAllCategoryAsync()
        {
            try
            {
                var categories = await _unitOfWork.Categories.GetAllAsync();
                var categoriesDto = categories.Select(MapToDto).ToList();
                return ApiResponse<IEnumerable<CategoryDto>>.SuccessResponse(categoriesDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching categories");
                return ApiResponse<IEnumerable<CategoryDto>>.ErrorResponse(
                    "An error occurred while fetching categories",
                    new List<string> { ex.Message }
                );
            }
            
        }

        private static CategoryDto MapToDto(Category equipment)
        {
            return new CategoryDto
            {
                 CategoryID = equipment.CategoryID,
                 CategoryName = equipment.CategoryName,
                 Description = equipment.Description
            };
        }
    }
}
