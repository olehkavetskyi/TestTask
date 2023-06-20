import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Url } from '../shared/models/Url';
import { UrlInfoService } from './url-info.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-url-info',
  templateUrl: './url-info.component.html',
  styleUrls: ['./url-info.component.scss']
})
export class UrlInfoComponent {
  url: Url | null = null;
  apiUrl = environment.apiUrl;
  constructor(private route: ActivatedRoute, private urlInfoService: UrlInfoService) {}

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      const id = Number(params.get('id'));
      this.getUrl(id);
    });
  }
 
  getUrl(id: number) {
    this.urlInfoService.getUrl(id).subscribe({
      next: (result) => {
        this.url = result,
        this.url.shortUrl = this.apiUrl + "r/" + this.url.shortUrl
      },
      error: err => console.log(err.message)
    });
  }
}

