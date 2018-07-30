import { MatButtonModule, MatProgressBarModule, MatFormFieldModule, MatInputModule, MatCardModule } from '@angular/material';
import {BrowserAnimationsModule} from "@angular/platform-browser/animations";
import { AuthGuardService } from './services/Auth/auth-guard.service';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {HttpClientModule} from '@angular/common/http';

import { AppComponent } from './app.component';
import { StartGameComponent } from './start-game/start-game.component';
import {OAuthModule} from 'angular-oauth2-oidc';
import { HttpModule } from '@angular/http';
import { Routes, RouterModule } from '@angular/router';
import { GameDashboardComponent } from './game-dashboard/game-dashboard.component';
import { CommonModule } from '@angular/common';

import { MaterialModule } from './material/material.module';

@NgModule({
  declarations: [
    AppComponent,
    StartGameComponent,
    GameDashboardComponent
  ],
  imports: [
    CommonModule,
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    HttpModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatProgressBarModule,
    MatCardModule,
    MaterialModule,
    RouterModule.forRoot([
      {path : 'game/play', component: GameDashboardComponent},
      {path : '*', component: StartGameComponent},
      {path : '', component: StartGameComponent},
    ]),
    BrowserAnimationsModule,
    OAuthModule.forRoot()
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
