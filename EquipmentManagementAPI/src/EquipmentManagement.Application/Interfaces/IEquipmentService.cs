using EquipmentManagement.Application.DTOs;

namespace EquipmentManagement.Application.Interfaces;

public interface IEquipmentService
{
    Task<ApiResponse<PagedResult<EquipmentDto>>> GetAllEquipmentAsync(EquipmentSearchDto searchDto);
    Task<ApiResponse<EquipmentDto>> GetEquipmentByIdAsync(int id);
    Task<ApiResponse<IEnumerable<EquipmentDto>>> SearchEquipmentAsync(EquipmentSearchDto searchDto);
    Task<ApiResponse<EquipmentDto>> CreateEquipmentAsync(CreateEquipmentDto createDto);
    Task<ApiResponse<EquipmentDto>> UpdateEquipmentAsync(UpdateEquipmentDto updateDto);
    Task<ApiResponse<bool>> DeleteEquipmentAsync(int id);
    Task<ApiResponse<IEnumerable<EquipmentDto>>> GetEquipmentByCategoryAsync(int categoryId);
}
