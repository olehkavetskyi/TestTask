import { Component } from '@angular/core';
import { Url } from '../shared/models/Url';
import { User, Roles } from '../shared/models/User';
import { UrlsTableService } from './urls-table.service';
import { ToastrService } from 'ngx-toastr';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-urls-table',
  templateUrl: './urls-table.component.html',
  styleUrls: ['./urls-table.component.scss']
})
export class UrlsTableComponent {
  currentUser: User | undefined;
  urls: Url[] = null!;
  fullUrl: string = null!;
  apiUrl  = environment.apiUrl;

  constructor(private urlsTableService: UrlsTableService, private toastr: ToastrService) { }

  getCurrentUser() {
    
    this.urlsTableService.getCurrentUser()
      .subscribe({
        next: response => {
          this.currentUser = response
        }
      })
  }

  ngOnInit() {
    this.getCurrentUser()
    this.getUrls();
  }

  addUrl() {
    this.urlsTableService.addUrl(this.fullUrl).subscribe(
      {
        next: success => {
          this.getUrls();
        },
        error: err => this.toastr.error("This url is already registred!")
      }
    )
  }

  isAdmin() {
    return Roles.Admin == this.currentUser?.role;
  }

  remove(id: number) {
    this.urlsTableService.removeUrl(id).subscribe({
      next: () => this.urls = this.urls.filter(e => e.id != id)
    });
  }

  removeAll() {
    this.urlsTableService.removeAll().subscribe({
      next: () => this.urls.length = 0
    });
  }

  ableToRemove(url: Url) {
    if (this.isAdmin())
      return true;
    if (this.currentUser?.id == url.createdByUserId)
      return true;
    return false;

  }

  getUrls() {
    this.urlsTableService.getShortUrls()
      .subscribe({
        next: response => this.urls = response,
        error: err => console.log(err)
    })
  }
}
