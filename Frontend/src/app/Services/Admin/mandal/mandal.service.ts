import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../common/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class MandalService extends BaseApiService {
  private entity = 'mandal';

  /**
   * Creates a new Mandal through the API.
   */
  createMandal(mandalData: any): Observable<any> {
    return this.create(this.entity, mandalData);
  }

  /**
   * Fetches all Mandals from the API.
   */
  getAllMandals(params: any = {}): Observable<any> {
    return this.getAllByParams(this.entity, params);
  }

  /**
   * Deletes a Mandal by its ID.
   */
  deleteMandal(id: number): Observable<any> {
    return this.delete(this.entity, id);
  }

  /**
   * Updates an existing Mandal.
   */
  updateMandal(mandalData: any): Observable<any> {
    return this.update(this.entity, mandalData);
  }

  /**
   * Fetches the combined report (Mandals -> Sectors -> Booths).
   */
  getAllCombinedReports(params: any = {}): Observable<any> {
    return this.getWithParams(`${this.apiUrl}/${this.entity}/getAllCombinedReports`, params);
  }

  getAllMandalReport(params: any = {}): Observable<any> {
    return this.getWithParams(`${this.apiUrl}/${this.entity}/getAllMandalReport`, params);
  }

  exportMandalReport(format: 'excel' | 'pdf', params: any = {}): Observable<Blob> {
    return this.export(`mandalreport`, format, params);
  }

  exportCombinedReportExcel(): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/${this.entity}/exportCombined/excel`, { responseType: 'blob' });
  }

  exportCombinedReportPdf(): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/${this.entity}/exportCombined/pdf`, { responseType: 'blob' });
  }

  exportMandalExcel(params: any = {}): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/${this.entity}/export/excel`, {
      params,
      responseType: 'blob'
    });
  }

  exportMandalPdf(params: any = {}): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/${this.entity}/export/pdf`, {
      params,
      responseType: 'blob'
    });
  }

  getSanyojak(mandalId: any): Observable<any> {
    return this.http.get(`${this.apiUrl}/${this.entity}/getsanyojak`, { params: { id: mandalId } });
  }

  exportCombinedMandalExcel(params: any = {}): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/combinedmandalreport/export/excel`, {
      params,
      responseType: 'blob'
    });
  }

  exportCombinedMandalPdf(params: any = {}): Observable<Blob> {
    return this.http.get(`${this.apiUrl}/combinedmandalreport/export/pdf`, {
      params,
      responseType: 'blob'
    });
  }
}
