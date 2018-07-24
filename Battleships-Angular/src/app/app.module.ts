import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {HttpClientModule} from '@angular/common/http';

import { AppComponent } from './app.component';
import { StartGameComponent } from './start-game/start-game.component';
import {OAuthModule} from 'angular-oauth2-oidc';
import { HttpModule } from '../../node_modules/@angular/http';

@NgModule({
  declarations: [
    AppComponent,
    StartGameComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    HttpModule,
    OAuthModule.forRoot()
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
