import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, map, of } from 'rxjs';
import { DropdownOption } from './generic-form.types';

import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class FormDataService {
  constructor(private http: HttpClient) { }

  /**
   * Fetches dropdown options from a given URL and maps them using a mapper function.
   * Automatically prefixes relative URLs with the environment's base API URL.
   * @param url The API endpoint to fetch data from.
   * @param mapper A function to transform the API response into an array of DropdownOptions.
   * @param formValues Optional current form values for context-aware mapping.
   */
  getOptionsFromApi(url: string, mapper?: (data: any, formValues?: any) => DropdownOption[], formValues?: any): Observable<DropdownOption[]> {
    if (!url) return of([]);

    // Prefix relative URLs with base API URL
    const finalUrl = url.startsWith('http') ? url : `${environment.apiUrl}${url.startsWith('/') ? '' : '/'}${url}`;

    const headers = { 'X-Skip-Loader': 'true' };
    return this.http.get<any>(finalUrl, { headers }).pipe(
      map(response => {
        if (mapper) {
          return mapper(response, formValues);
        }
        // Default mapper for common response formats (e.g., arrays)
        if (Array.isArray(response)) {
          return response.map(item => ({
            value: item.id || item.value || item,
            label: item.name || item.label || item
          }));
        }
        return [];
      })
    );
  }
}
