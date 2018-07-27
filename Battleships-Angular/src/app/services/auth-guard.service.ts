import { Injectable } from "@angular/core";
import {
  CanLoad,
  CanActivate,
  Route,
  Router,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  RouterModule
} from "@angular/router";
import { OAuthService } from "angular-oauth2-oidc";

@Injectable({
  providedIn: "root"
})
export class AuthGuardService implements CanLoad, CanActivate {
  constructor(private oauthSvc: OAuthService) {}

  canActivate(): boolean {
    if(this.oauthSvc.hasValidAccessToken()){
      return true;
    }
    // this.route.redirectTo('/home');
    return false;
  }

  canLoad(): boolean {
    debugger;
    debugger;
    if(this.oauthSvc.hasValidAccessToken()){
      return true;
    }
    return false;
  }
}
