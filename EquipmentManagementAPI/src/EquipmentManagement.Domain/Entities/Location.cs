namespace EquipmentManagement.Domain.Entities;

public class Location
{
    public int LocationID { get; set; }
    public string LocationName { get; set; } = string.Empty;
    public string Building { get; set; } = string.Empty;
    public string Floor { get; set; } = string.Empty;
    public DateTime CreatedDate { get; set; }
    public DateTime ModifiedDate { get; set; }

    // Navigation properties
    public virtual ICollection<Equipment> Equipment { get; set; } = new List<Equipment>();
}
