import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map, Observable } from 'rxjs';
import { User } from '../shared/models/User';
import { environment } from '../../environments/environment';
import { Url } from '../shared/models/Url';

@Injectable({
  providedIn: 'root'
})
export class UrlsTableService {
  apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getCurrentUser(): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}api/user/current-user`);
  }

  getShortUrls(): Observable<Url[]> {
    return this.http.get<Url[]>(`${this.apiUrl}api/url`).pipe(
      map(urls => {
        return urls.map(url => {
          url.shortUrl = `${this.apiUrl}r/${url.shortUrl}`;
          return url;
        });
      })
    );
  }

  removeUrl(urlId: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}api/url/${urlId}`);
  }

  removeAll(): Observable<any> {
    return this.http.delete(`${this.apiUrl}api/url`);
  }

  addUrl(fullUrl: string): Observable<any> { 
    return this.http.post<any>(`${this.apiUrl}api/url/`, { fullUrl });
  }
}
