import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class BaseApiService {
  protected apiUrl = environment.apiUrl;

  constructor(protected http: HttpClient) { }

  /**
   * Generic GET request for a list of entities.
   * Pattern: /[entity]/getAll
   */
  getAll<T>(entity: string): Observable<T> {
    return this.http.get<T>(`${this.apiUrl}/${entity}/getAll`);
  }

  /**
   * Generic POST request to create an entity.
   * Pattern: /[entity]/create
   */
  create<T>(entity: string, data: any): Observable<T> {
    return this.http.post<T>(`${this.apiUrl}/${entity}/create`, data);
  }

  /**
   * Generic POST request to update an entity.
   * Pattern: /[entity]/update
   */
  update<T>(entity: string, data: any): Observable<T> {
    return this.http.post<T>(`${this.apiUrl}/${entity}/update`, data);
  }

  /**
   * Generic POST request to delete an entity using a query parameter ID.
   * Pattern: /[entity]/delete?id=[id]
   */
  delete<T>(entity: string, id: number | string): Observable<T> {
    const params = new HttpParams().set('id', id);
    return this.http.post<T>(`${this.apiUrl}/${entity}/delete`, {}, { params });
  }

  /**
   * Generic GET request for a single entity by ID.
   */
  getById<T>(entity: string, id: number | string): Observable<T> {
    return this.http.get<T>(`${this.apiUrl}/${entity}/getById/${id}`);
  }
}
