import { GamesService } from './games-service.service';
import { Component, OnInit } from '@angular/core';
import { OAuthService } from '../../node_modules/angular-oauth2-oidc';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  
  title = 'app';
  constructor(private gamesService : GamesService, private oauthService:OAuthService){}

  ngOnInit(): void {
    debugger;
    // Issuer
    this.oauthService.issuer = 'https://localhost:44362';
    // URL of the SPA to redirect the user to after login
    // this.oauthService.redirectUri = window.location.origin + "/index.html";
    this.oauthService.redirectUri = 'http://localhost:4200';
    this.oauthService.responseType = 'id_token token';
    this.oauthService.postLogoutRedirectUri = 'http://localhost:4200';
    // The SPA's id. The SPA is registerd with this id at the auth-server
    this.oauthService.clientId = 'ng';
    // set the scope for the permissions the client should request
    // The first three are defined by OIDC. The 4th is a usecase-specific one
    this.oauthService.scope = "openid profile";
    // set to true, to receive also an id_token via OpenId Connect (OIDC) in addition to the
    // OAuth2-based access_token
    this.oauthService.oidc = true; // ID_Token

    // Use setStorage to use sessionStorage or another implementation of the TS-type Storage
    // instead of localStorage
    this.oauthService.setStorage(sessionStorage);

    // Discovery Document of your AuthServer as defined by OIDC
    let url = 'https://localhost:44362//.well-known//openid-configuration';
    // Load Discovery Document and then try to login the user
    this.oauthService.loadDiscoveryDocument(url).then(() => {

      // This method just tries to parse the token(s) within the url when
      // the auth-server redirects the user back to the web-app
      // It dosn't send the user the the login page
      this.oauthService.tryLogin({});
      });
      this.oauthService.initImplicitFlow();

      // this.oauthService.responseType = "id_token code";
      // this.oauthService.initHybridFlow();
    }
  }


  // https://localhost:44362/connect/authorize?
  // response_type=id_token%20token&
  // client_id=93cc5e6f-432b-432f-b906-466e4b47ff30&
  // state=YE5Ic0yKWhSRevLKQg0lcOlBVmxW7ND0uPLMCrbK&
  // redirect_uri=http%3A%2F%2Flocalhost%3A4200&
  // scope=openid%20profile&nonce=YE5Ic0yKWhSRevLKQg0lcOlBVmxW7ND0uPLMCrbK