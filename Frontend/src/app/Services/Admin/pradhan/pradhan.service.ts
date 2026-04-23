import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../common/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class PradhanService extends BaseApiService {
  private entity = 'pradhan';

  createPradhan(data: any): Observable<any> {
    return this.create(this.entity, data);
  }

  getAllPradhans(params?: any): Observable<any> {
    return this.getAllByParams(this.entity, params);
  }

  deletePradhan(id: number): Observable<any> {
    return this.delete(this.entity, id);
  }

  updatePradhan(data: any): Observable<any> {
    return this.update(this.entity, data);
  }
}
