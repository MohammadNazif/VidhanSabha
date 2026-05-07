import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../../environments/environment';
import { BaseApiService } from '../../common/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class PannapramukhService extends BaseApiService {
  private entity = 'pannapramukh';

  getAllPannapramukhs(params: any = {}): Observable<any> {
    return this.getAllByParams(this.entity, params);
  }

  createPannapramukh(data: any): Observable<any> {
    return this.create(this.entity, data);
  }

  updatePannapramukh(data: any): Observable<any> {
    return this.update(this.entity, data);
  }

  deletePannapramukh(id: number): Observable<any> {
    return this.delete(this.entity, id);
  }

  exportToExcel(params: any = {}): Observable<Blob> {
    return this.export(this.entity, 'excel', params);
  }

  exportToPdf(params: any = {}): Observable<Blob> {
    return this.export(this.entity, 'pdf', params);
  }

  getCommonData(path: string, userId?: string | null, pageSize: number = 1000): Observable<any> {
    let url = `common/${path}`;
    return this.getCustom(url);
  }
}
