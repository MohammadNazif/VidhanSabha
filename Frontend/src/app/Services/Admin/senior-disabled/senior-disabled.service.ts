import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../common/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class SeniorDisabledService extends BaseApiService {
  private entity = 'seniordisabled';

  createSeniorDisabled(data: any): Observable<any> {
    return this.create(this.entity, data);
  }

  getAllSeniorDisabled(params?: any): Observable<any> {
    return this.getAllByParams(this.entity, params);
  }

  deleteSeniorDisabled(id: number): Observable<any> {
    return this.delete(this.entity, id);
  }

  updateSeniorDisabled(data: any): Observable<any> {
    return this.update(this.entity, data);
  }
}
