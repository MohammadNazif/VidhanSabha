import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../common/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class VidhanSabhaPrabhariService extends BaseApiService {
  private entity = 'stateprabhari/vidhansabhaPrabhari';

  createPrabhari(data: any): Observable<any> {
    return this.create('stateprabhari/vidhansabha', data);
  }

  getAllPrabharis(stateId?: number | string | null): Observable<any> {
    const params = stateId ? { stateId } : {};
    return this.getAllByParams(this.entity, params);
  }

  deletePrabhari(id: number): Observable<any> {
    return this.delete(this.entity, id);
  }

  updatePrabhari(data: any): Observable<any> {
    return this.update('stateprabhari', data);
  }
}
