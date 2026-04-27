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

  // Add other methods if needed
}
