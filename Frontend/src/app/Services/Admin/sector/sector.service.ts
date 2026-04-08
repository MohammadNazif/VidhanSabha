import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../common/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class SectorService extends BaseApiService {
  private entity = 'sector';

  createSector(data: any): Observable<any> {
    return this.create(this.entity, data);
  }

  getAllSectors(): Observable<any> {
    return this.getAll(this.entity);
  }

  deleteSector(id: number): Observable<any> {
    return this.delete(this.entity, id);
  }

  updateSector(data: any): Observable<any> {
    return this.update(this.entity, data);
  }
}
