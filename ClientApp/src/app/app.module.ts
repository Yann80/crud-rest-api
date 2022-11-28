import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { BrowserAnimationsModule } from "@angular/platform-browser/animations";
import { NgxSpinnerModule } from "ngx-spinner";

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';

import { AuthorComponent } from './author/author.component';
import { BlogComponent } from './blog/blog.component';
import { MicroblogService } from './services/microblog.service';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    AuthorComponent,
    BlogComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    ReactiveFormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'auteur', component: AuthorComponent },
      { path: 'list-billet', component: BlogComponent },
      { path: 'list-billet/:id', component: BlogComponent }
    ]),
    NgxSpinnerModule,
    BrowserAnimationsModule
  ],
  providers: [MicroblogService],
  bootstrap: [AppComponent]
})
export class AppModule { }
