import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { Content, CreateContentRequest, UpdateContentRequest } from '../models/content.model';

@Injectable({ providedIn: 'root' })
export class ContentService {
  private http = inject(HttpClient);
  private apiUrl = `${environment.apiUrl}/contents`;

  getAll() {
    return this.http.get<Content[]>(this.apiUrl);
  }

  getById(id: string) {
    return this.http.get<Content>(`${this.apiUrl}/${id}`);
  }

  create(request: CreateContentRequest) {
    return this.http.post<Content>(this.apiUrl, request);
  }

  update(id: string, request: UpdateContentRequest) {
    return this.http.put<Content>(`${this.apiUrl}/${id}`, request);
  }

  delete(id: string) {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
