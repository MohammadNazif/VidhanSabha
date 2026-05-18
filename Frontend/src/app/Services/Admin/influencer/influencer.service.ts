import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../common/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class InfluencerService extends BaseApiService {
  private entity = 'influencer';

  createInfluencer(data: any): Observable<any> {
    return this.create(this.entity, data);
  }

  getAllInfluencer(params: any = {}): Observable<any> {
    return this.getAllByParams(this.entity, params);
  }

  deleteInfluencer(id: number): Observable<any> {
    return this.delete(this.entity, id);
  }

  updateInfluencer(data: any): Observable<any> {
    return this.update(this.entity, data);
  }

  exportToExcel(): Observable<Blob> {
    return this.export(this.entity, 'excel');
  }

  exportToPdf(): Observable<Blob> {
    return this.export(this.entity, 'pdf');
  }

  getDesignations(): Observable<any> {
    return this.getCustom('common/getadmindesignation');
  }

  getCommonData(path: string, userId?: string | null, pageSize: number = 1000): Observable<any> {
    let url = `common/${path}?PageNumber=1&PageSize=${pageSize}`;
    if (userId) url += `&userId=${userId}`;
    return this.getCustom(url);
  }

  importExcel(file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post(`${this.apiUrl}/${this.entity}/import/excel`, formData);
  }
}
