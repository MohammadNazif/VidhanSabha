import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../common/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class DistrictService extends BaseApiService {
  private entity = 'district';

  createDistrict(data: any): Observable<any> {
    return this.create(this.entity, data);
  }

  getAllDistricts(params: any = {}): Observable<any> {
    return this.getAllByParams(this.entity, params);
  }

  getDistrictsByStateId(stateId: number | string): Observable<any> {
    return this.getAllByParams(this.entity, { stateId });
  }

  deleteDistrict(id: number): Observable<any> {
    return this.delete(this.entity, id);
  }

  updateDistrict(data: any): Observable<any> {
    return this.update(this.entity, data);
  }
}
