-- Equipment Management Database Schema
-- Author: Equipment Management System
-- Date: 2026-02-05

USE master;
GO

-- Drop database if exists
IF EXISTS (SELECT name FROM sys.databases WHERE name = 'EquipmentManagementDB')
BEGIN
    ALTER DATABASE EquipmentManagementDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE EquipmentManagementDB;
END
GO

-- Create database
CREATE DATABASE EquipmentManagementDB;
GO

USE EquipmentManagementDB;
GO

-- Create Categories table
CREATE TABLE Categories (
    CategoryID INT IDENTITY(1,1) PRIMARY KEY,
    CategoryName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500) NULL,
    CreatedDate DATETIME2 DEFAULT GETUTCDATE(),
    ModifiedDate DATETIME2 DEFAULT GETUTCDATE(),
    CONSTRAINT UQ_CategoryName UNIQUE (CategoryName)
);
GO

-- Create Locations table
CREATE TABLE Locations (
    LocationID INT IDENTITY(1,1) PRIMARY KEY,
    LocationName NVARCHAR(100) NOT NULL,
    Building NVARCHAR(100) NOT NULL,
    Floor NVARCHAR(50) NOT NULL,
    CreatedDate DATETIME2 DEFAULT GETUTCDATE(),
    ModifiedDate DATETIME2 DEFAULT GETUTCDATE(),
    CONSTRAINT UQ_Location UNIQUE (LocationName, Building, Floor)
);
GO

-- Create Equipment table
CREATE TABLE Equipment (
    EquipmentID INT IDENTITY(1,1) PRIMARY KEY,
    EquipmentName NVARCHAR(200) NOT NULL,
    SerialNumber NVARCHAR(100) NOT NULL,
    CategoryID INT NOT NULL,
    LocationID INT NOT NULL,
    PurchaseDate DATE NOT NULL,
    Status NVARCHAR(50) NOT NULL,
    CreatedDate DATETIME2 DEFAULT GETUTCDATE(),
    ModifiedDate DATETIME2 DEFAULT GETUTCDATE(),
    CONSTRAINT UQ_SerialNumber UNIQUE (SerialNumber),
    CONSTRAINT FK_Equipment_Categories FOREIGN KEY (CategoryID) 
        REFERENCES Categories(CategoryID),
    CONSTRAINT FK_Equipment_Locations FOREIGN KEY (LocationID) 
        REFERENCES Locations(LocationID),
    CONSTRAINT CHK_Status CHECK (Status IN ('Active', 'InMaintenance', 'OutOfService', 'Retired'))
);
GO

-- Create indexes for better query performance
CREATE INDEX IX_Equipment_CategoryID ON Equipment(CategoryID);
CREATE INDEX IX_Equipment_LocationID ON Equipment(LocationID);
CREATE INDEX IX_Equipment_Status ON Equipment(Status);
CREATE INDEX IX_Equipment_PurchaseDate ON Equipment(PurchaseDate);
CREATE INDEX IX_Equipment_EquipmentName ON Equipment(EquipmentName);
GO

-- Insert sample data for Categories
INSERT INTO Categories (CategoryName, Description) VALUES
('Computers', 'Desktop computers, laptops, and tablets'),
('Network Equipment', 'Routers, switches, and network infrastructure'),
('Printers', 'Laser and inkjet printers'),
('Mobile Devices', 'Smartphones and tablets'),
('Furniture', 'Office desks, chairs, and cabinets'),
('Audio/Visual', 'Projectors, monitors, and audio equipment');
GO

-- Insert sample data for Locations
INSERT INTO Locations (LocationName, Building, Floor) VALUES
('IT Department', 'Main Building', '3rd Floor'),
('HR Department', 'Main Building', '2nd Floor'),
('Finance Department', 'East Wing', '1st Floor'),
('Conference Room A', 'Main Building', '1st Floor'),
('Conference Room B', 'East Wing', '2nd Floor'),
('Storage Room', 'West Wing', 'Basement');
GO

-- Insert sample data for Equipment
INSERT INTO Equipment (EquipmentName, SerialNumber, CategoryID, LocationID, PurchaseDate, Status) VALUES
('Dell Latitude 5520', 'DL-2023-001', 1, 1, '2023-01-15', 'Active'),
('HP LaserJet Pro', 'HP-2023-002', 3, 2, '2023-02-20', 'Active'),
('Cisco Switch 2960', 'CS-2022-003', 2, 1, '2022-11-10', 'Active'),
('iPhone 14 Pro', 'IP-2024-004', 4, 2, '2024-03-05', 'Active'),
('Samsung Monitor 27"', 'SM-2023-005', 6, 1, '2023-05-18', 'Active'),
('Lenovo ThinkPad X1', 'LT-2021-006', 1, 3, '2021-08-22', 'InMaintenance'),
('BenQ Projector', 'BQ-2022-007', 6, 4, '2022-09-14', 'Active'),
('Office Desk - Executive', 'OD-2020-008', 5, 2, '2020-03-10', 'Active'),
('Canon Printer MF445dw', 'CP-2023-009', 3, 3, '2023-07-25', 'OutOfService'),
('Dell OptiPlex 7090', 'DO-2024-010', 1, 1, '2024-01-12', 'Active');
GO

-- Create view for Equipment with related data
CREATE VIEW vw_EquipmentDetails AS
SELECT 
    e.EquipmentID,
    e.EquipmentName,
    e.SerialNumber,
    e.PurchaseDate,
    e.Status,
    c.CategoryID,
    c.CategoryName,
    c.Description AS CategoryDescription,
    l.LocationID,
    l.LocationName,
    l.Building,
    l.Floor,
    e.CreatedDate,
    e.ModifiedDate
FROM Equipment e
INNER JOIN Categories c ON e.CategoryID = c.CategoryID
INNER JOIN Locations l ON e.LocationID = l.LocationID;
GO

-- Create stored procedure for getting equipment with pagination
CREATE PROCEDURE sp_GetEquipmentPaged
    @PageNumber INT = 1,
    @PageSize INT = 10,
    @SearchTerm NVARCHAR(200) = NULL,
    @CategoryID INT = NULL,
    @Status NVARCHAR(50) = NULL,
    @SortBy NVARCHAR(50) = 'EquipmentName',
    @SortDirection NVARCHAR(4) = 'ASC'
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @Offset INT = (@PageNumber - 1) * @PageSize;
    
    WITH FilteredEquipment AS (
        SELECT *
        FROM vw_EquipmentDetails
        WHERE 
            (@SearchTerm IS NULL OR 
             EquipmentName LIKE '%' + @SearchTerm + '%' OR
             SerialNumber LIKE '%' + @SearchTerm + '%')
            AND (@CategoryID IS NULL OR CategoryID = @CategoryID)
            AND (@Status IS NULL OR Status = @Status)
    )
    SELECT 
        *,
        (SELECT COUNT(*) FROM FilteredEquipment) AS TotalRecords
    FROM FilteredEquipment
    ORDER BY 
        CASE WHEN @SortBy = 'EquipmentName' AND @SortDirection = 'ASC' THEN EquipmentName END ASC,
        CASE WHEN @SortBy = 'EquipmentName' AND @SortDirection = 'DESC' THEN EquipmentName END DESC,
        CASE WHEN @SortBy = 'PurchaseDate' AND @SortDirection = 'ASC' THEN PurchaseDate END ASC,
        CASE WHEN @SortBy = 'PurchaseDate' AND @SortDirection = 'DESC' THEN PurchaseDate END DESC,
        CASE WHEN @SortBy = 'Status' AND @SortDirection = 'ASC' THEN Status END ASC,
        CASE WHEN @SortBy = 'Status' AND @SortDirection = 'DESC' THEN Status END DESC
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;
END
GO

PRINT 'Database schema created successfully!';
GO
