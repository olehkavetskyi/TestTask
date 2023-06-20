import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { UrlsTableComponent } from './urls-table/urls-table.component';
import { RedirectComponent } from './redirect/redirect.component';
import { FormsModule } from '@angular/forms';
import { ToastrModule } from 'ngx-toastr';
import { UrlInfoComponent } from './url-info/url-info.component';

@NgModule({
  declarations: [
    AppComponent,
    UrlsTableComponent,
    RedirectComponent,
    UrlInfoComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    FormsModule,
    ToastrModule.forRoot()
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
