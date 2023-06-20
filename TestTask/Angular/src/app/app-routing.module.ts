import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { RedirectComponent } from './redirect/redirect.component';
import { UrlsTableComponent } from './urls-table/urls-table.component';
import { UrlInfoComponent } from './url-info/url-info.component';

const routes: Routes = [
  { path: 'urls-table', component: UrlsTableComponent },
  { path: 'info/:id', component: UrlInfoComponent },
  { path: 'r/:shortUrl', component: RedirectComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
