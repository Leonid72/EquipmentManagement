using System.ComponentModel.DataAnnotations;

namespace EquipmentManagement.Application.DTOs;

public class EquipmentDto
{
    public int EquipmentID { get; set; }
    public string EquipmentName { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public int CategoryID { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public int LocationID { get; set; }
    public string LocationName { get; set; } = string.Empty;
    public string Building { get; set; } = string.Empty;
    public string Floor { get; set; } = string.Empty;
    public DateTime PurchaseDate { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class CreateEquipmentDto
{
    [Required(ErrorMessage = "Equipment name is required")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Equipment name must be between 3 and 200 characters")]
    public string EquipmentName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Serial number is required")]
    [StringLength(100, MinimumLength = 5, ErrorMessage = "Serial number must be between 5 and 100 characters")]
    public string SerialNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Category ID is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Category ID must be a positive number")]
    public int CategoryID { get; set; }

    [Required(ErrorMessage = "Location ID is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Location ID must be a positive number")]
    public int LocationID { get; set; }

    [Required(ErrorMessage = "Purchase date is required")]
    [DataType(DataType.Date)]
    public DateTime PurchaseDate { get; set; }

    [Required(ErrorMessage = "Status is required")]
    [RegularExpression("^(Active|InMaintenance|OutOfService|Retired)$", 
        ErrorMessage = "Status must be Active, InMaintenance, OutOfService, or Retired")]
    public string Status { get; set; } = string.Empty;
}

public class UpdateEquipmentDto
{
    [Required(ErrorMessage = "Equipment ID is required")]
    public int EquipmentID { get; set; }

    [Required(ErrorMessage = "Equipment name is required")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Equipment name must be between 3 and 200 characters")]
    public string EquipmentName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Serial number is required")]
    [StringLength(100, MinimumLength = 5, ErrorMessage = "Serial number must be between 5 and 100 characters")]
    public string SerialNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Category ID is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Category ID must be a positive number")]
    public int CategoryID { get; set; }

    [Required(ErrorMessage = "Location ID is required")]
    [Range(1, int.MaxValue, ErrorMessage = "Location ID must be a positive number")]
    public int LocationID { get; set; }

    [Required(ErrorMessage = "Purchase date is required")]
    [DataType(DataType.Date)]
    public DateTime PurchaseDate { get; set; }

    [Required(ErrorMessage = "Status is required")]
    [RegularExpression("^(Active|InMaintenance|OutOfService|Retired)$", 
        ErrorMessage = "Status must be Active, InMaintenance, OutOfService, or Retired")]
    public string Status { get; set; } = string.Empty;
}

public class EquipmentSearchDto
{
    public string? SearchTerm { get; set; }
    public int? CategoryID { get; set; }
    public DateTime? PurchaseDateFrom { get; set; }
    public DateTime? PurchaseDateTo { get; set; }
    public string? Status { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string SortBy { get; set; } = "EquipmentName";
    public string SortDirection { get; set; } = "ASC";
}

public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = new List<T>();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
}
