import { Injectable } from "@angular/core";
import {
  CanLoad,
  CanActivate,
  Route,
  Router,
  ActivatedRouteSnapshot,
  RouterStateSnapshot
} from "@angular/router";
import { OAuthService } from "../../../node_modules/angular-oauth2-oidc";

@Injectable({
  providedIn: "root"
})
export class AuthGuardService implements CanLoad, CanActivate {
  constructor(private oauthSvc: OAuthService) {}

  canActivate(): boolean {
    debugger;
    let hasIdToken = this.oauthSvc.hasValidIdToken();
    let hasAccessToken = this.oauthSvc.hasValidAccessToken();

    return hasIdToken && hasAccessToken;
  }

  canLoad(): boolean {
    debugger;
    let hasIdToken = this.oauthSvc.hasValidIdToken();
    let hasAccessToken = this.oauthSvc.hasValidAccessToken();

    return hasIdToken && hasAccessToken;
  }
}
