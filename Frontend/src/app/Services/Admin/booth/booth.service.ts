import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../common/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class BoothService extends BaseApiService {
  private entity = 'booth';

  createBooth(data: any): Observable<any> {
    return this.create(this.entity, data);
  }

  getAllBooths(params: any = {}): Observable<any> {
    return this.getAllByParams(this.entity, params);
  }

  deleteBooth(id: number | string): Observable<any> {
    return this.delete(this.entity, id);
  }

  updateBooth(data: any): Observable<any> {
    return this.update(this.entity, data);
  }

  getBoothIncharge(): Observable<any> {
    return this.http.get(`${this.apiUrl}/common/getboothincharge?pageSize=1000`);
  }
}
