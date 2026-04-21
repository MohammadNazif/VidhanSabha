import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpParams } from '@angular/common/http';
import { BaseApiService } from '../../common/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class StateService extends BaseApiService {
  private entity = 'vidhansabhacount';

  createState(data: any): Observable<any> {
    return this.create(this.entity, data);
  }

  getAllStates(params: any = {}): Observable<any> {
    const userId = localStorage.getItem('userId');
    if (userId) {
      params['userId'] = userId;
    }
    return this.getAllByParams(this.entity, params);
  }

  deleteState(id: number): Observable<any> {
    return this.delete(this.entity, id);
  }

  updateState(data: any): Observable<any> {
    return this.update(this.entity, data);
  }
}
