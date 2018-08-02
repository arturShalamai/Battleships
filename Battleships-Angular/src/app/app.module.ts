import { GameDashboardComponent } from './game-dashboard/game-dashboard.component';
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
import { RegisterUserComponent } from './register-user/register-user.component';
import { FormsModule } from '../../node_modules/@angular/forms';
import { LoginComponent } from './login/login.component';
import { NewGameComponent } from './new-game/new-game.component';

@NgModule({
  declarations: [
    AppComponent,
    StartGameComponent,
    RegisterUserComponent,
    LoginComponent,
    NewGameComponent,
    GameDashboardComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    HttpModule,
    FormsModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatProgressBarModule,
    MatCardModule,
    RouterModule.forRoot([
      {path : '*', component: StartGameComponent},
      {path : '', component: StartGameComponent},
      {path : 'game-dashboard', component: GameDashboardComponent},
    ]),
    OAuthModule.forRoot()
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
