import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../common/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class MandalSamitiService extends BaseApiService {
  private entity = 'mandalsamiti';

  createMandalSamiti(data: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/${this.entity}/create`, {}, {
      params: {
        MandalId: data.mandalId,
        id: data.sanyojakId
      }
    });
  }

  getMandalSamiti(params: any): Observable<any> {
    return this.getWithParams(`${this.apiUrl}/${this.entity}/getAll`, params);
  }

  deleteMandalSamiti(id: number): Observable<any> {
    return this.delete(this.entity, id);
  }

  createMandalSamitiMem(data: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/mandalsamiti/members/create`, data);
  }

  getAllMembers(mandalId: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/mandalsamiti/members/getAll?mandalId=${mandalId}`);
  }

  updateMandalSamitiMem(data: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/mandalsamiti/member/update`, data);
  }

  deleteMandalSamitiMem(id: number, mandalId: number): Observable<any> {
    return this.http.post(`${this.apiUrl}/mandalsamiti/member/delete?id=${id}&MandalId=${mandalId}`, {});
  }
}
