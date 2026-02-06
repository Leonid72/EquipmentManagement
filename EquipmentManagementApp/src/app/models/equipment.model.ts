export interface Equipment {
  equipmentID: number;
  equipmentName: string;
  serialNumber: string;
  categoryID: number;
  categoryName: string;
  locationID: number;
  locationName: string;
  building: string;
  floor: string;
  purchaseDate: Date;
  status: EquipmentStatus;
}

export interface CreateEquipment {
  equipmentName: string;
  serialNumber: string;
  categoryID: number;
  locationID: number;
  purchaseDate: Date;
  status: EquipmentStatus;
}

export interface UpdateEquipment {
  equipmentID: number;
  equipmentName: string;
  serialNumber: string;
  categoryID: number;
  locationID: number;
  purchaseDate: Date;
  status: EquipmentStatus;
}

export interface EquipmentSearch {
  searchTerm?: string;
  categoryID?: number;
  purchaseDateFrom?: Date;
  purchaseDateTo?: Date;
  status?: string;
  pageNumber: number;
  pageSize: number;
  sortBy: string;
  sortDirection: 'ASC' | 'DESC';
}

export type EquipmentStatus = 'Active' | 'InMaintenance' | 'OutOfService' | 'Retired';

export const EQUIPMENT_STATUSES: EquipmentStatus[] = ['Active', 'InMaintenance', 'OutOfService', 'Retired'];
