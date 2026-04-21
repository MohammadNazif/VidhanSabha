import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../common/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class BlockService extends BaseApiService {
  private entity = 'block';

  createBlock(data: any): Observable<any> {
    return this.create(this.entity, data);
  }

  getAllBlocks(): Observable<any> {
    return this.getAll(this.entity);
  }

  deleteBlock(id: number): Observable<any> {
    return this.delete(this.entity, id);
  }

  updateBlock(data: any): Observable<any> {
    return this.update(this.entity, data);
  }
}
