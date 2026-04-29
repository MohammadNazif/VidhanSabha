import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { BaseApiService } from '../../common/base-api.service';

@Injectable({
  providedIn: 'root'
})
export class SocialMediaService extends BaseApiService {
  private entity = 'socialmedia';

  getAllSocialMedia(params: any = {}): Observable<any> {
    return this.getAllByParams(this.entity, params);
  }

  createSocialMedia(data: any): Observable<any> {
    return this.create(this.entity, data);
  }

  updateSocialMedia(data: any): Observable<any> {
    return this.update(this.entity, data);
  }

  deleteSocialMedia(id: number): Observable<any> {
    return this.delete(this.entity, id);
  }

  getAllPlatforms(): Observable<any> {
    // Calling the endpoint /api/socialmedia/getAllPlatform
    return this.http.get(`${this.apiUrl}/${this.entity}/getAllPlatform`);
  }
}
