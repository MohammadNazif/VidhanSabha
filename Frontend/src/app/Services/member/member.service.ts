import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from '../common/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class MemberService extends BaseApiService {
  private entity = 'member';

  createMember(data: any): Observable<any> {
    return this.create(this.entity, data);
  }

  getAllMembers(): Observable<any> {
    return this.getAll(this.entity);
  }

  deleteMember(id: number): Observable<any> {
    return this.delete(this.entity, id);
  }

  updateMember(data: any): Observable<any> {
    return this.update(this.entity, data);
  }
}
