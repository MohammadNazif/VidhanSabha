import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../common/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class PrabhavshaliService extends BaseApiService {
  private entity = 'prabhavshali';

  createPrabhavshali(data: any): Observable<any> {
    return this.create(this.entity, data);
  }

  getAllPrabhavshali(params: any = {}): Observable<any> {
    return this.getAllByParams(this.entity, params);
  }

  // Specialized method for category-based lists (Doctor, Advocate, etc.)
  getPrabhavshaliByDesignation(params: any = {}): Observable<any> {
    const desgId = params.designationId;
    delete params.designationId; // Remove from params to avoid duplication in query string if needed
    return this.getWithParams(`${this.apiUrl}/${this.entity}/getDesgById?desgId=${desgId}`, params);
  }

  deletePrabhavshali(id: number): Observable<any> {
    return this.delete(this.entity, id);
  }

  updatePrabhavshali(data: any): Observable<any> {
    return this.update(this.entity, data);
  }

  exportToExcel(): Observable<Blob> {
    return this.export(this.entity, 'excel');
  }

  exportToPdf(): Observable<Blob> {
    return this.export(this.entity, 'pdf');
  }

  exportSpecial(entityName: string, format: 'excel' | 'pdf'): Observable<Blob> {
    return this.export(entityName, format);
  }

  getDesignations(): Observable<any> {
    return this.getCustom('common/getadmindesignation');
  }

  getCommonData(path: string, userId?: string | null, pageSize: number = 1000): Observable<any> {
    let url = `common/${path}`;
    if (userId) url += `&userId=${userId}`;
    return this.getCustom(url);
  }
}
