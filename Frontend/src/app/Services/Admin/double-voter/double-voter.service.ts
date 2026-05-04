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

  getAllDoubleVoters(params: any = {}): Observable<any> {
    return this.getAllByParams(this.entity, params);
  }

  deleteDoubleVoter(id: number): Observable<any> {
    return this.delete(this.entity, id);
  }

  updateDoubleVoter(data: any): Observable<any> {
    return this.update(this.entity, data);
  }

  exportToExcel(): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/${this.entity}/export/excel`, { responseType: 'blob' });
  }

  exportToPdf(): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/${this.entity}/export/pdf`, { responseType: 'blob' });
  }
}
