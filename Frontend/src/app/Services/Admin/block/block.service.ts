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

  getAllBlocks(params: any = {}): Observable<any> {
    return this.getAllByParams(this.entity, params);
  }

  deleteBlock(id: number): Observable<any> {
    return this.delete(this.entity, id);
  }

  updateBlock(data: any): Observable<any> {
    return this.update(this.entity, data);
  }

  exportToExcel(): Observable<Blob> {
    return this.export(this.entity, 'excel');
  }

  exportToPdf(): Observable<Blob> {
    return this.export(this.entity, 'pdf');
  }
}
