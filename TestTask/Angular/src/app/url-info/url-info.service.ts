import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Url } from '../shared/models/Url';

@Injectable({
  providedIn: 'root'
})
export class UrlInfoService {
  apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getUrl(id: number) : Observable<Url>{
    return this.http.get<Url>(`${this.apiUrl}api/url/by-id/${id}`);
  }
}
