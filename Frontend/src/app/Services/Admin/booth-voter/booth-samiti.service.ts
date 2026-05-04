import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../common/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class BoothSamitiService extends BaseApiService {
  private entity = 'boothsamiti';

  createBoothSamiti(data: any): Observable<any> {
    // Send BoothId as a query parameter as expected by the backend
    return this.http.post(`${this.apiUrl}/${this.entity}/create`, {}, {
      params: { BoothId: data.boothId }
    });
  }

  updateBoothSamiti(data: any): Observable<any> {
    return this.update(this.entity, data);
  }

  getBoothSamiti(params: any): Observable<any> {
    return this.getAllByParams(this.entity, params);
  }

  deleteBoothSamiti(id: number): Observable<any> {
    return this.delete(this.entity, id);
  }

  createBoothSamitiMem(data: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/boothsamitiMem/create`, data);
  }

  getAllMembers(id: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/boothsamitiMem/getAllMem?id=${id}`);
  }

  updateBoothSamitiMem(data: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/boothsamitiMem/update`, data);
  }

  deleteBoothSamitiMem(id: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/boothsamitiMem/delete?id=${id}`, {});
  }

  getDesignations(): Observable<any> {
    return this.http.get(`${this.apiUrl}/boothsamiti-designation/getAll`);
  }
}
