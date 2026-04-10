import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PannapramukhService {
  private apiUrl = `${environment.apiUrl}/pannapramukh`;

  constructor(private http: HttpClient) { }

  getAllPannapramukhs(): Observable<any> {
    return this.http.get(`${this.apiUrl}/getall`);
  }

  createPannapramukh(data: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/create`, data);
  }

  updatePannapramukh(data: any): Observable<any> {
    return this.http.put(`${this.apiUrl}/update`, data);
  }

  deletePannapramukh(id: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/delete?id=${id}`);
  }
}
