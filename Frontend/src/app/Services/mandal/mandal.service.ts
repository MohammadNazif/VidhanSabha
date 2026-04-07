import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from '../common/base-api.service';

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
  getAllMandals(): Observable<any> {
    return this.getAll(this.entity);
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
}
