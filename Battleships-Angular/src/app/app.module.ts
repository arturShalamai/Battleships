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
import { JoinGameComponent } from './join-game/join-game.component';
import { FieldComponent } from './field/field.component';
import { LogoutComponent } from './logout/logout.component';
import { GameResultComponent } from './game-result/game-result.component';
import { GameSurrenderComponent } from './game-surrender/game-surrender.component';
import { SigninPlatformComponent } from './signin-platform/signin-platform.component';

@NgModule({
  declarations: [
    AppComponent,
    StartGameComponent,
    RegisterUserComponent,
    LoginComponent,
    NewGameComponent,
    JoinGameComponent,
    GameDashboardComponent,
    FieldComponent,
    LogoutComponent,
    GameResultComponent,
    GameSurrenderComponent,
    SigninPlatformComponent
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
      {path : 'login', component: StartGameComponent},
      {path : 'signin-platform', component: SigninPlatformComponent},
      {path : 'game', component: GameDashboardComponent, canActivate: [AuthGuardService], },
      {path : 'game/:id', component: GameDashboardComponent, canActivate: [AuthGuardService]},
      {path : 'game-res', component: GameResultComponent, canActivate: [AuthGuardService]},
    ], {onSameUrlNavigation: `reload`}),
    OAuthModule.forRoot()
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
