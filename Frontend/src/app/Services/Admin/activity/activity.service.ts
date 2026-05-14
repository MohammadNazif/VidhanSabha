import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../common/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class ActivityService extends BaseApiService {
  private entity = 'activity';

  getAllActivities(params: any = {}): Observable<any> {
    return this.getAllByParams(this.entity, params);
  }

  createActivity(data: FormData): Observable<any> {
    return this.http.post(`${this.apiUrl}/${this.entity}/create`, data);
  }

  updateActivity(data: FormData): Observable<any> {
    return this.http.post(`${this.apiUrl}/${this.entity}/update`, data);
  }

  deleteActivity(id: number | string): Observable<any> {
    return this.delete(this.entity, id);
  }
}
