import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../common/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class DoubleVoterService extends BaseApiService {
  private entity = 'doublevoter';

  createDoubleVoter(data: any): Observable<any> {
    return this.create(this.entity, data);
  }

  getAllDoubleVoters(): Observable<any> {
    return this.getAll(this.entity);
  }

  deleteDoubleVoter(id: number): Observable<any> {
    return this.delete(this.entity, id);
  }

  updateDoubleVoter(data: any): Observable<any> {
    return this.update(this.entity, data);
  }
}
