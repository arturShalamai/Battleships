import { MatButtonModule, MatProgressBarModule, MatFormFieldModule, MatInputModule, MatCardModule } from '@angular/material';
import { AuthGuardService } from './services/auth-guard.service';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {HttpClientModule} from '@angular/common/http';

import { AppComponent } from './app.component';
import { StartGameComponent } from './start-game/start-game.component';
import {OAuthModule} from 'angular-oauth2-oidc';
import { HttpModule } from '@angular/http';
import { Routes, RouterModule } from '@angular/router';

@NgModule({
  declarations: [
    AppComponent,
    StartGameComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    HttpModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatProgressBarModule,
    MatCardModule,
    RouterModule.forRoot([
      {path : 'start-game', component: StartGameComponent, canActivate: [AuthGuardService]},
      {path : '*', component: StartGameComponent}
    ]),
    OAuthModule.forRoot()
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
