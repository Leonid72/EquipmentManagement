namespace EquipmentManagement.Domain.Entities;

public class Equipment
{
    public int EquipmentID { get; set; }
    public string EquipmentName { get; set; } = string.Empty;
    public string SerialNumber { get; set; } = string.Empty;
    public int CategoryID { get; set; }
    public int LocationID { get; set; }
    public DateTime PurchaseDate { get; set; }
    public EquipmentStatus Status { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }

    // Navigation properties
    public virtual Category? Category { get; set; }
    public virtual Location? Location { get; set; }
}

public enum EquipmentStatus
{
    Active,
    InMaintenance,
    OutOfService,
    Retired
}
