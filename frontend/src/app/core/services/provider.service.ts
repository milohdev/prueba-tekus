import { Injectable, inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Provider, UpdateProviderRequest } from '../models/provider.model';
import { PagedResult } from '../models/paged-result.model';

@Injectable({ providedIn: 'root' })
export class ProviderService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/providers`;

  getAll(search = '', sortBy = '', page = 1, pageSize = 10) {
    const params = new HttpParams()
      .set('search', search)
      .set('sortBy', sortBy)
      .set('page', page)
      .set('pageSize', pageSize);

    return this.http.get<PagedResult<Provider>>(this.apiUrl, { params });
  }

  getById(id: string) {
    return this.http.get<Provider>(`${this.apiUrl}/${id}`);
  }

  update(id: string, request: UpdateProviderRequest) {
    return this.http.put<Provider>(`${this.apiUrl}/${id}`, request);
  }
}