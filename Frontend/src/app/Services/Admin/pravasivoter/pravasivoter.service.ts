import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../common/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class PravasivoterService extends BaseApiService {
  private entity = 'pravasivoter';

  getAllPravasivoters(): Observable<any> {
    return this.getAll(this.entity);
  }

  createPravasivoter(data: any): Observable<any> {
    return this.create(this.entity, data);
  }

  updatePravasivoter(data: any): Observable<any> {
    return this.update(this.entity, data);
  }

  deletePravasivoter(id: number): Observable<any> {
    return this.delete(this.entity, id);
  }
}
