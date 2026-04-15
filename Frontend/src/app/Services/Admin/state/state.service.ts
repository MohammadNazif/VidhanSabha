import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../common/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class StateService extends BaseApiService {
  private entity = 'vidhansabhacount';

  createState(data: any): Observable<any> {
    return this.create(this.entity, data);
  }

  getAllStates(): Observable<any> {
    return this.getAll(this.entity);
  }

  deleteState(id: number): Observable<any> {
    return this.delete(this.entity, id);
  }

  updateState(data: any): Observable<any> {
    return this.update(this.entity, data);
  }
}
