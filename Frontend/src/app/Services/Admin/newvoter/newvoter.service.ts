import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../common/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class NewvoterService extends BaseApiService {
  private entity = 'newvoter';

  getAllNewvoters(): Observable<any> {
    return this.getAll(this.entity);
  }

  createNewvoter(data: any): Observable<any> {
    return this.create(this.entity, data);
  }

  updateNewvoter(data: any): Observable<any> {
    return this.update(this.entity, data);
  }

  deleteNewvoter(id: number): Observable<any> {
    return this.delete(this.entity, id);
  }
}
