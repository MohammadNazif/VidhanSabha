import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../common/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class StateMemberService extends BaseApiService {
  private entity = 'statemembers';

  createMember(data: FormData): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/${this.entity}/create`, data);
  }

  getAllMembers(params: any = {}): Observable<any> {
    return this.getAllByParams(this.entity, params);
  }

  deleteMember(id: number): Observable<any> {
    return this.delete(this.entity, id);
  }

  updateMember(data: FormData): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/${this.entity}/update`, data);
  }

  getMemberById(id: number): Observable<any> {
    return this.getById(this.entity, id);
  }
}
