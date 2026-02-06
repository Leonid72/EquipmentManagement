import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { ApiResponse } from '../models/api-response.model';
import { Location } from '../models/location.model';

@Injectable({
  providedIn: 'root'
})
export class LocationService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/locations`;

  getAll(): Observable<ApiResponse<Location[]>> {
    return this.http.get<ApiResponse<Location[]>>(this.apiUrl);
  }

  getById(id: number): Observable<ApiResponse<Location>> {
    return this.http.get<ApiResponse<Location>>(`${this.apiUrl}/${id}`);
  }
}
