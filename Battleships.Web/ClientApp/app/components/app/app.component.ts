import { OAuthService, JwksValidationHandler } from 'angular-oauth2-oidc';
import { SignalRGameService } from "./../signalr-game-service/signalrGameService";
import { Component } from "@angular/core";

@Component({
  selector: "app",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.css"]
})
export class AppComponent {
  constructor(private hubSvc: SignalRGameService, private oauthSvc: OAuthService) {
    debugger;
    this.oauthSvc.redirectUri = window.location.origin;
    this.oauthSvc.clientId = 'Games.Battleships';
    this.oauthSvc.scope = 'openid profile email';
    this.oauthSvc.issuer = 'https://localhost:44362/oauth2/default';
    this.oauthSvc.tokenValidationHandler = new JwksValidationHandler();

    // Load Discovery Document and then try to login the user
    this.oauthSvc.loadDiscoveryDocumentAndTryLogin();
  }
  // private _hub : HubContext;
}
