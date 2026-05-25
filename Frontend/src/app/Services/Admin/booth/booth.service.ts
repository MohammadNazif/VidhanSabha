import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../common/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class BoothService extends BaseApiService {
  private entity = 'booth';

  createBooth(data: any): Observable<any> {
    return this.create(this.entity, data);
  }

  getAllBooths(params: any = {}): Observable<any> {
    return this.getAllByParams(this.entity, params);
  }

  deleteBooth(id: number | string): Observable<any> {
    return this.delete(this.entity, id);
  }

  updateBooth(data: any): Observable<any> {
    return this.update(this.entity, data);
  }

  getBoothIncharge(): Observable<any> {
    return this.http.get(`${this.apiUrl}/common/getboothincharge?pageSize=1000`);
  }

  getAllBoothReports(params: any = {}): Observable<any> {
    return this.getWithParams(`${this.apiUrl}/${this.entity}/getAllBoothReports`, params);
  }

  exportBoothReport(format: 'excel' | 'pdf', params: any = {}): Observable<Blob> {
    return this.export('boothreport', format, params);
  }

  exportToExcel(): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/${this.entity}/export/excel`, { responseType: 'blob' });
  }

  exportToPdf(): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/${this.entity}/export/pdf`, { responseType: 'blob' });
  }
}
