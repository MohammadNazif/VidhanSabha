import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../common/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class CasteVoterService extends BaseApiService {
  private entity = 'castevoter';

  createCasteVoter(data: any): Observable<any> {
    return this.create(this.entity, data);
  }

  getCasteVotersByBoothVoterId(boothVoterId: number | string): Observable<any> {
    return this.getAllByParams(this.entity, { Id: boothVoterId, PageNumber: 1, PageSize: 100, IsDescending: true });
  }

  updateCasteVoter(data: any): Observable<any> {
    return this.update(this.entity, data);
  }

  deleteCasteVoter(id: number | string): Observable<any> {
    return this.delete(this.entity, id);
  }
}
