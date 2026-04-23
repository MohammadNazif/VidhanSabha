import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../common/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class PrabhavshaliService extends BaseApiService {
  private entity = 'prabhavshali';

  createPrabhavshali(data: any): Observable<any> {
    return this.create(this.entity, data);
  }

  getAllPrabhavshali(params: any = {}): Observable<any> {
    return this.getAllByParams(this.entity, params);
  }

  deletePrabhavshali(id: number): Observable<any> {
    return this.delete(this.entity, id);
  }

  updatePrabhavshali(data: any): Observable<any> {
    return this.update(this.entity, data);
  }
}
