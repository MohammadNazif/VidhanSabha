import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../common/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class BoothSamitiService extends BaseApiService {
  private entity = 'boothsamiti';

  createBoothSamiti(data: any): Observable<any> {
    return this.create(this.entity, data);
  }

  updateBoothSamiti(data: any): Observable<any> {
    return this.update(this.entity, data);
  }

  getBoothSamiti(params: any): Observable<any> {
    return this.getAll(this.entity, params);
  }

  deleteBoothSamiti(id: number): Observable<any> {
    return this.delete(this.entity, id);
  }

  getDesignations(): Observable<any> {
    return this.http.get(`${this.apiUrl}boothsamiti-designation/getAll`);
  }
}
