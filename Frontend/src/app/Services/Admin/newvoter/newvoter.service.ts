import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../common/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class NewvoterService extends BaseApiService {
  private entity = 'newvoter';

  getAllNewvoters(params: any = {}): Observable<any> {
    return this.getAllByParams(this.entity, params);
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

  exportToExcel(): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/${this.entity}/export/excel`, { responseType: 'blob' });
  }

  exportToPdf(): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/${this.entity}/export/pdf`, { responseType: 'blob' });
  }
}
