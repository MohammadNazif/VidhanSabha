import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MandalService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  /**
   * Creates a new Mandal through the API.
   * @param mandalData The data gathered from the registration form.
   */
  createMandal(mandalData: any): Observable<any> {
    const endpoint = `${this.apiUrl}/mandal/create`;
    return this.http.post<any>(endpoint, mandalData);
  }

  /**
   * Fetches all Mandals from the API.
   */
  getAllMandals(): Observable<any> {
    const endpoint = `${this.apiUrl}/mandal/getAll`;
    return this.http.get<any>(endpoint);
  }
}

