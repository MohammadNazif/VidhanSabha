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
   * Generic GET request with standardized query parameters.
   */
  getWithParams<T>(url: string, queryParams: any = {}): Observable<T> {
    let params = new HttpParams();

    // 1. Handle Mandatory Parameters with Defaults
    const pageNumber = queryParams.PageNumber || queryParams.pageNumber || 1;
    const pageSize = queryParams.PageSize || queryParams.pageSize || 50;
    const isDescending = queryParams.IsDescending ?? queryParams.isDescending ?? true;

    params = params.set('PageNumber', String(pageNumber));
    params = params.set('PageSize', String(pageSize));
    params = params.set('IsDescending', String(isDescending));

    // 2. Handle Standard Optional Parameters (PascalCase)
    const sortBy = queryParams.sortBy || queryParams.SortBy || 'id';
    params = params.set('SortBy', String(sortBy));

    if (queryParams.searchTerm || queryParams.SearchTerm) {
      params = params.set('SearchTerm', String(queryParams.searchTerm || queryParams.SearchTerm));
    }

    // 3. Handle Custom Parameters
    const standardKeys = [
      'pageNumber', 'PageNumber',
      'pageSize', 'PageSize',
      'isDescending', 'IsDescending',
      'searchTerm', 'SearchTerm',
      'sortBy', 'SortBy'
    ];

    Object.keys(queryParams).forEach(key => {
      if (!standardKeys.includes(key) && queryParams[key] !== undefined && queryParams[key] !== null) {
        params = params.set(key, String(queryParams[key]));
      }
    });

    return this.http.get<T>(url, { params });
  }

  getAllByParams<T>(entity: string, queryParams: any = {}): Observable<T> {
    return this.getWithParams<T>(`${this.apiUrl}/${entity}/getAll`, queryParams);
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
