import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../common/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class VidhanSabhaPrabhariService extends BaseApiService {
  private entity = 'vidhansabhaprabhari';

  createPrabhari(data: any): Observable<any> {
    return this.create(this.entity, data);
  }

  getAllPrabharis(): Observable<any> {
    return this.getAll(this.entity);
  }

  deletePrabhari(id: number): Observable<any> {
    return this.delete(this.entity, id);
  }

  updatePrabhari(data: any): Observable<any> {
    return this.update(this.entity, data);
  }
}
