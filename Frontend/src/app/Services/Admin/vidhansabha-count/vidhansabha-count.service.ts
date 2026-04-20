import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpParams } from '@angular/common/http';
import { BaseApiService } from '../../common/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class VidhanSabhaCountService extends BaseApiService {
  private entity = 'vidhansabhacount';

  getAllByUserId(userId: string): Observable<any> {
    const params = new HttpParams().set('userId', userId);
    return this.http.get<any>(`${this.apiUrl}/${this.entity}/getAll`, { params });
  }

  getDistrictCountAllByUserId(userId: string): Observable<any> {
    const params = new HttpParams().set('userId', userId);
    return this.http.get<any>(`${this.apiUrl}/${this.entity}/districtwise/getAll`, { params });
  }

  createVidhanSabhaCount(data: any): Observable<any> {
    return this.create(`${this.entity}/districtwise`, data);
  }

  updateVidhanSabhaCount(data: any): Observable<any> {
    return this.update(`${this.entity}/districtwise`, data);
  }
}
