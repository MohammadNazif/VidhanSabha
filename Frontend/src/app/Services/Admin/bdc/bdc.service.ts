import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../common/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class BdcService extends BaseApiService {
  private entity = 'bdc';

  createBdc(data: any): Observable<any> {
    return this.create(this.entity, data);
  }

  getAllBdcs(params?: any): Observable<any> {
    return this.getAllByParams(this.entity, params);
  }

  deleteBdc(id: number): Observable<any> {
    return this.delete(this.entity, id);
  }

  updateBdc(data: any): Observable<any> {
    return this.update(this.entity, data);
  }
}
