import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { ApiResponse, PagedResult } from '../models/api-response.model';
import { Equipment, CreateEquipment, UpdateEquipment, EquipmentSearch } from '../models/equipment.model';

@Injectable({
  providedIn: 'root'
})
export class EquipmentService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/Equipments`;

  getAll(search: EquipmentSearch): Observable<ApiResponse<PagedResult<Equipment>>> {
    let params = new HttpParams()
      .set('pageNumber', search.pageNumber.toString())
      .set('pageSize', search.pageSize.toString())
      .set('sortBy', search.sortBy)
      .set('sortDirection', search.sortDirection);

    if (search.searchTerm) {
      params = params.set('searchTerm', search.searchTerm);
    }
    if (search.categoryID) {
      params = params.set('categoryID', search.categoryID.toString());
    }
    if (search.purchaseDateFrom) {
      params = params.set('purchaseDateFrom', search.purchaseDateFrom.toISOString());
    }
    if (search.purchaseDateTo) {
      params = params.set('purchaseDateTo', search.purchaseDateTo.toISOString());
    }
    if (search.status) {
      params = params.set('status', search.status);
    }

    return this.http.get<ApiResponse<PagedResult<Equipment>>>(this.apiUrl, { params });
  }

  getById(id: number): Observable<ApiResponse<Equipment>> {
    return this.http.get<ApiResponse<Equipment>>(`${this.apiUrl}/${id}`);
  }

  create(equipment: CreateEquipment): Observable<ApiResponse<Equipment>> {
    return this.http.post<ApiResponse<Equipment>>(this.apiUrl, equipment);
  }

  update(equipment: UpdateEquipment): Observable<ApiResponse<Equipment>> {
    return this.http.put<ApiResponse<Equipment>>(`${this.apiUrl}/${equipment.equipmentID}`, equipment);
  }

  delete(id: number): Observable<ApiResponse<boolean>> {
    return this.http.delete<ApiResponse<boolean>>(`${this.apiUrl}/${id}`);
  }
}
