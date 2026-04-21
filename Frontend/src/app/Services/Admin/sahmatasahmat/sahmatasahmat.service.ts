import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../common/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class SahmatAsahmatService extends BaseApiService {
  private entity = 'sahmatasahmat';

  getAllSahmatAsahmat(params: any = {}): Observable<any> {
    return this.getAllByParams(this.entity, params);
  }

  createSahmatAsahmat(data: any): Observable<any> {
    return this.create(this.entity, data);
  }

  updateSahmatAsahmat(data: any): Observable<any> {
    return this.update(this.entity, data);
  }

  deleteSahmatAsahmat(id: number): Observable<any> {
    return this.delete(this.entity, id);
  }
}
