namespace EquipmentManagement.Domain.Entities;

public class Category
{
    public int CategoryID { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }

    // Navigation properties
    public virtual ICollection<Equipment> Equipment { get; set; } = new List<Equipment>();
}
