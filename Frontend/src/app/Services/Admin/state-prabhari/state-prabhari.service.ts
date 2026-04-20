import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpParams } from '@angular/common/http';
import { BaseApiService } from '../../common/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class StatePrabhariService extends BaseApiService {
  private entity = 'stateprabhari';

  createPrabhari(data: any): Observable<any> {
    return this.create(this.entity, data);
  }

  getAllPrabharis(): Observable<any> {
    return this.getAll(this.entity);
  }

  deletePrabhari(id: number, userId: string, data: any): Observable<any> {
    const params = new HttpParams().set('id', id).set('userId', userId);
    return this.http.post<any>(`${this.apiUrl}/${this.entity}/delete`, data, { params });
  }

  updatePrabhari(data: any): Observable<any> {
    return this.update(this.entity, data);
  }
}
