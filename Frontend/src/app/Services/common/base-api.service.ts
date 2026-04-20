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
   * Generic GET request for a list of entities with standardized query parameters.
   */
  getAllByParams<T>(entity: string, queryParams: any = {}): Observable<T> {
    // 1. Initialize with mandatory defaults
    const params: any = {
      PageNumber: queryParams.pageNumber || queryParams.PageNumber || 1,
      PageSize: queryParams.pageSize || queryParams.PageSize || 50,
      IsDescending: queryParams.isDescending === true || queryParams.isDescending === 'true' || queryParams.IsDescending === true || false
    };

    // 2. Map standard optional parameters to PascalCase if they exist
    const standardMap: Record<string, string> = {
      searchTerm: 'SearchTerm',
      SearchTerm: 'SearchTerm',
      sortBy: 'SortBy',
      SortBy: 'SortBy'
    };

    Object.keys(standardMap).forEach(key => {
      if (queryParams[key] !== undefined && queryParams[key] !== null) {
        params[standardMap[key]] = queryParams[key];
      }
    });

    // 3. Merge every other parameter as-is (filtering by MandalId, SectorId, etc.)
    const handledKeys = ['pageNumber', 'PageNumber', 'pageSize', 'PageSize', 'isDescending', 'IsDescending', ...Object.keys(standardMap)];
    Object.keys(queryParams).forEach(key => {
      if (!handledKeys.includes(key) && queryParams[key] !== undefined && queryParams[key] !== null) {
        params[key] = queryParams[key];
      }
    });

    return this.http.get<T>(`${this.apiUrl}/${entity}/getAll`, { params });
  }

  getAll<T>(entity: string, pageNumber: number = 1, pageSize: number = 50): Observable<T> {
    return this.getAllByParams<T>(entity, { pageNumber, pageSize });
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
