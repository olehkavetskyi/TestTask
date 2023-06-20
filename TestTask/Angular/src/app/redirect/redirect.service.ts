import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class RedirectService {
  public apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getFullUrl(shortUrl: string): Observable<any> {
    return this.http.get<any>(`${this.apiUrl}api/url/${shortUrl}`);
  }
}
