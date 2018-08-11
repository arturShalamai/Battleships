import { Component, OnInit } from "@angular/core";
import { OAuthService, JwksValidationHandler } from "angular-oauth2-oidc";
import { GamesService } from "./services/games-service.service";

@Component({
  selector: "app-root",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.css"]
})
export class AppComponent implements OnInit {
  title = "app";
  constructor(
    private gamesService: GamesService,
    private oauthService: OAuthService
  ) {}

  ngOnInit(): void {
    // Config Info = https://www.softwarearchitekt.at/post/2016/07/03/authentication-in-angular-2-with-oauth2-oidc-and-guards-for-the-newest-new-router-english-version.aspx

    this.oauthService.configure({
      clientId: "client-7d2ff6a2-a1ab-4a7c-9967-609376135f50",
      issuer: "https://localhost:44362",
      oidc: true,
      redirectUri: 'http://localhost:4200/signin-platform',
      scope: "openid profile"
    });

    // Discovery Document of your AuthServer as defined by OIDC
    let url = "https://localhost:44362//.well-known//openid-configuration";
    // this.oauthService.tokenValidationHandler = new JwksValidationHandler();

    // Load Discovery Document and then try to login the user
    this.oauthService.loadDiscoveryDocument(url).then(() => {
      // This method just tries to parse the token(s) within the url when
      // the auth-server redirects the user back to the web-app
      // It dosn't send the user the the login page
      this.oauthService.setStorage(localStorage);
      this.oauthService.tryLogin({}).catch(err => console.log(err));
    });
    let claims = this.oauthService.getIdentityClaims();
    if (claims != null) {
      console.log(claims);
    }
  }

  public Login(): void {
    this.oauthService.initImplicitFlow();
  }

  claims: string;

  public getName() {
    let claims = this.oauthService.getIdentityClaims();
    let identityToken = this.oauthService.getIdToken();
  }

  public Logout(): void {
    this.oauthService.logOut();
  }
}


//ClientId client-12068b21-d0f0-41e4-b030-664349a9720d
//ClientSecret secret-470e5043-8a56-4041-af1d-d23231db9690