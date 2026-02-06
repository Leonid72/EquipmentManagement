using System.ComponentModel.DataAnnotations;

namespace EquipmentManagement.Application.DTOs;

public class CategoryDto
{
    public int CategoryID { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string? Description { get; set; }
}

public class LocationDto
{
    public int LocationID { get; set; }
    public string LocationName { get; set; } = string.Empty;
    public string Building { get; set; } = string.Empty;
    public string Floor { get; set; } = string.Empty;
}
