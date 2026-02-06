using EquipmentManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EquipmentManagement.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Equipment> Equipment { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<Location> Locations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Equipment configuration
        modelBuilder.Entity<Equipment>(entity =>
        {
            entity.ToTable("Equipment");
            entity.HasKey(e => e.EquipmentID);
            
            entity.Property(e => e.EquipmentName)
                .IsRequired()
                .HasMaxLength(200);
            
            entity.Property(e => e.SerialNumber)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.HasIndex(e => e.SerialNumber)
                .IsUnique()
                .HasDatabaseName("UQ_SerialNumber");
            
            entity.Property(e => e.Status)
                .IsRequired()
                .HasMaxLength(50)
                .HasConversion<string>();
            
            entity.Property(e => e.PurchaseDate)
                .IsRequired()
                .HasColumnType("date");
            
            entity.Property(e => e.CreatedDate)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETUTCDATE()");
            
            entity.Property(e => e.ModifiedDate)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETUTCDATE()");

            // Relationships
            entity.HasOne(e => e.Category)
                .WithMany(c => c.Equipment)
                .HasForeignKey(e => e.CategoryID)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Equipment_Categories");
            
            entity.HasOne(e => e.Location)
                .WithMany(l => l.Equipment)
                .HasForeignKey(e => e.LocationID)
                .OnDelete(DeleteBehavior.Restrict)
                .HasConstraintName("FK_Equipment_Locations");

            // Indexes
            entity.HasIndex(e => e.CategoryID)
                .HasDatabaseName("IX_Equipment_CategoryID");
            
            entity.HasIndex(e => e.LocationID)
                .HasDatabaseName("IX_Equipment_LocationID");
            
            entity.HasIndex(e => e.Status)
                .HasDatabaseName("IX_Equipment_Status");
            
            entity.HasIndex(e => e.PurchaseDate)
                .HasDatabaseName("IX_Equipment_PurchaseDate");
            
            entity.HasIndex(e => e.EquipmentName)
                .HasDatabaseName("IX_Equipment_EquipmentName");
        });

        // Category configuration
        modelBuilder.Entity<Category>(entity =>
        {
            entity.ToTable("Categories");
            entity.HasKey(c => c.CategoryID);
            
            entity.Property(c => c.CategoryName)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.HasIndex(c => c.CategoryName)
                .IsUnique()
                .HasDatabaseName("UQ_CategoryName");
            
            entity.Property(c => c.Description)
                .HasMaxLength(500);
            
            entity.Property(c => c.CreatedDate)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETUTCDATE()");
            
            entity.Property(c => c.ModifiedDate)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETUTCDATE()");
        });

        // Location configuration
        modelBuilder.Entity<Location>(entity =>
        {
            entity.ToTable("Locations");
            entity.HasKey(l => l.LocationID);
            
            entity.Property(l => l.LocationName)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(l => l.Building)
                .IsRequired()
                .HasMaxLength(100);
            
            entity.Property(l => l.Floor)
                .IsRequired()
                .HasMaxLength(50);
            
            entity.HasIndex(l => new { l.LocationName, l.Building, l.Floor })
                .IsUnique()
                .HasDatabaseName("UQ_Location");
            
            entity.Property(l => l.CreatedDate)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETUTCDATE()");
            
            entity.Property(l => l.ModifiedDate)
                .HasColumnType("datetime2")
                .HasDefaultValueSql("GETUTCDATE()");
        });
        // Seed initial data
        //SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasData(
            new Category
            {
                CategoryID = 1,
                CategoryName = "Laptops",
                Description = "Portable computers",
                CreatedDate = new DateTime(2024, 1, 1),
                ModifiedDate = new DateTime(2024, 1, 1)
            },
            new Category
            {
                CategoryID = 2,
                CategoryName = "Monitors",
                Description = "Display equipment",
                CreatedDate = new DateTime(2024, 1, 1),
                ModifiedDate = new DateTime(2024, 1, 1)
            }
        );

        modelBuilder.Entity<Location>().HasData(
            new Location
            {
                LocationID = 1,
                LocationName = "IT Storage",
                Building = "HQ",
                Floor = "B1",
                CreatedDate = new DateTime(2024, 1, 1),
                ModifiedDate = new DateTime(2024, 1, 1)
            },
            new Location
            {
                LocationID = 2,
                LocationName = "Office Area",
                Building = "HQ",
                Floor = "3",
                CreatedDate = new DateTime(2024, 1, 1),
                ModifiedDate = new DateTime(2024, 1, 1)
            }
        );

        modelBuilder.Entity<Equipment>().HasData(
            new Equipment
            {
                EquipmentID = 1,
                EquipmentName = "Dell Latitude 7440",
                SerialNumber = "DL-7440-001",
                Status = EquipmentStatus.Active,
                PurchaseDate = new DateTime(2023, 6, 15),
                CategoryID = 1,
                LocationID = 1,
                CreatedDate = new DateTime(2024, 1, 1),
                ModifiedDate = new DateTime(2024, 1, 1)
            },
            new Equipment
            {
                EquipmentID = 2,
                EquipmentName = "Samsung 27\" Monitor",
                SerialNumber = "SM-27-009",
                Status = EquipmentStatus.Active,
                PurchaseDate = new DateTime(2022, 11, 20),
                CategoryID = 2,
                LocationID = 2,
                CreatedDate = new DateTime(2024, 1, 1),
                ModifiedDate = new DateTime(2024, 1, 1)
            }
        );
    }
}
