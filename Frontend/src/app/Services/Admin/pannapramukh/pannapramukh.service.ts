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

  getAllPannapramukhs(): Observable<any> {
    return this.getAll(this.entity);
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
}
