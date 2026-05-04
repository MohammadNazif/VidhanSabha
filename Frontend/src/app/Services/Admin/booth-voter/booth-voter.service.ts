import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../common/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class BoothVoterService extends BaseApiService {
  private entity = 'boothvoter';

  createBoothVoter(data: any): Observable<any> {
    return this.create(this.entity, data);
  }

  getAllBoothVoters(params: any = {}): Observable<any> {
    return this.getAllByParams(this.entity, params);
  }

  updateBoothVoter(data: any): Observable<any> {
    return this.update(this.entity, data);
  }

  deleteBoothVoter(id: number | string): Observable<any> {
    return this.delete(this.entity, id);
  }

  exportToExcel(): Observable<Blob> {
    return this.export(this.entity, 'excel');
  }

  exportToPdf(): Observable<Blob> {
    return this.export(this.entity, 'pdf');
  }

  getCommonData(path: string, userId?: string | null, pageSize: number = 1000): Observable<any> {
    let url = `common/${path}`;
    if (userId) url += `&userId=${userId}`;
    return this.getCustom(url);
  }
}
