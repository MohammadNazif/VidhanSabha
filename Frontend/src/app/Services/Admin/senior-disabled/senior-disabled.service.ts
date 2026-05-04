import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../common/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class SeniorDisabledService extends BaseApiService {
  private entity = 'seniordisabled';

  createSeniorDisabled(data: any): Observable<any> {
    return this.create(this.entity, data);
  }

  getAllSeniorDisabled(params?: any): Observable<any> {
    return this.getAllByParams(this.entity, params);
  }

  deleteSeniorDisabled(id: number): Observable<any> {
    return this.delete(this.entity, id);
  }

  updateSeniorDisabled(data: any): Observable<any> {
    return this.update(this.entity, data);
  }

  exportToExcel(): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/${this.entity}/export/excel`, { responseType: 'blob' });
  }

  exportToPdf(): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/${this.entity}/export/pdf`, { responseType: 'blob' });
  }

  exportSpecial(entityName: string, format: 'excel' | 'pdf'): Observable<Blob> {
    return this.export(entityName, format);
  }
}
