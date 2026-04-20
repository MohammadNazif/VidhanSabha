import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../common/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class VidhanSabhaService extends BaseApiService {
  private entity = 'stateprabhari/vidhansabha';

  createVidhanSabha(data: any): Observable<any> {
    return this.create(this.entity, data);
  }

  getVidhanSabhasByStateId(stateId: any | string): Observable<any> {
    return this.getAllByParams(this.entity, { stateId });
  }

  getVidhanSabhasByDistrictId(districtId: number | string): Observable<any> {
    return this.getAllByParams(this.entity, { districtId });
  }

  deleteVidhanSabha(id: number): Observable<any> {
    return this.delete(this.entity, id);
  }

  updateVidhanSabha(data: any): Observable<any> {
    return this.update(this.entity, data);
  }
}
