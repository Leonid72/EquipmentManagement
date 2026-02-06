using EquipmentManagement.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EquipmentManagement.Application.Interfaces
{
    public interface ICategoryService
    {
        Task<ApiResponse<IEnumerable<CategoryDto>>> GetAllCategoryAsync();
    }
}
