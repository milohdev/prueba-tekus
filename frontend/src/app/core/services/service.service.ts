import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { CreateServiceRequest, Service, UpdateServiceRequest } from '../models/service.model';
import { PagedResult } from '../models/paged-result.model';

@Injectable({ providedIn: 'root' })
export class ServiceService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/services`;

  getAll(
    providerId = '',
    search = '',
    sortBy = '',
    descending = false,
    page = 1,
    pageSize = 10
  ) {
    const params = new HttpParams()
      .set('search', search)
      .set('providerId', providerId)
      .set('sortBy', sortBy)
      .set('descending', descending)
      .set('page', page)
      .set('pageSize', pageSize);

    return this.http.get<PagedResult<Service>>(this.apiUrl, { params });
  }

  getById(id: string) {
    return this.http.get<Service>(`${this.apiUrl}/${id}`);
  }

  create(request: CreateServiceRequest) {
    return this.http.post<Service>(this.apiUrl, request);
  }

  update(id: string, request: UpdateServiceRequest) {
    return this.http.put<Service>(`${this.apiUrl}/${id}`, request);
  }

  delete(id: string) {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}