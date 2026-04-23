import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../common/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class DistrictPrabhariService extends BaseApiService {
  private entity = 'districtprabhari';

  createPrabhari(data: any): Observable<any> {
    return this.create(this.entity, data);
  }

  getAllPrabharis(params: any = {}): Observable<any> {
    return this.getAllByParams(this.entity, params);
  }

  deletePrabhari(id: number): Observable<any> {
    return this.delete(this.entity, id);
  }

  updatePrabhari(data: any): Observable<any> {
    return this.update(this.entity, data);
  }
}
