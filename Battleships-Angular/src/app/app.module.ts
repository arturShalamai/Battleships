import { AuthGuardService } from './services/auth-guard.service';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {HttpClientModule} from '@angular/common/http';

import { AppComponent } from './app.component';
import { StartGameComponent } from './start-game/start-game.component';
import {OAuthModule} from 'angular-oauth2-oidc';
import { HttpModule } from '../../node_modules/@angular/http';
import { Routes, RouterModule } from '../../node_modules/@angular/router';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import { MaterialModule } from './material/material.module';

@NgModule({
  declarations: [
    AppComponent,
    StartGameComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    HttpModule,
    MaterialModule,
    RouterModule.forRoot([
      {path : 'start-game', component: StartGameComponent, canActivate: [AuthGuardService]}
    ]),
    BrowserAnimationsModule,
    OAuthModule.forRoot()
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
