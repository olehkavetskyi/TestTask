import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from '../../environments/environment';
import { Url } from '../shared/models/Url';
import { RedirectService } from './redirect.service';

@Component({
  selector: 'app-redirect',
  template: '',
})
export class RedirectComponent {
  constructor(private router: Router, private redirectService: RedirectService) {
    const shortUrl = window.location.pathname.split('/').pop();
    this.redirectToFullUrl(shortUrl as string);
  }

  private redirectToFullUrl(shortUrl: string): void {

    this.redirectService.getFullUrl(shortUrl).subscribe({
      next: response => {
        const fullUrl = response.fullUrl;
        window.location.href = fullUrl;
      },
      error: (err) => {
        console.error('Failed to retrieve full URL:', err);
      }
    })
  }
}
