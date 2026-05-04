import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from '../common/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class DashboardService extends BaseApiService {
  private entity = '/counts';

  getGlobalCounts(): Observable<any> {
    return this.http.get(`${this.apiUrl}${this.entity}/getAll`);
  }

  getBoothCounts(): Observable<any> {
    return this.http.get(`${this.apiUrl}${this.entity}/booth/getAll`);
  }
}
