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

  getAllBooths(mandalId?: number, sectorId?: number): Observable<any> {
    let params: any = {};
    if (mandalId) params['mandalId'] = mandalId;
    if (sectorId) params['sectorId'] = sectorId;
    return this.getAllByParams(this.entity, params);
  }

  deleteBooth(id: number | string): Observable<any> {
    return this.delete(this.entity, id);
  }

  updateBooth(data: any): Observable<any> {
    return this.update(this.entity, data);
  }
}
