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
import { OAuthService } from "../../../node_modules/angular-oauth2-oidc";

@Injectable({
  providedIn: "root"
})
export class AuthGuardService implements CanLoad, CanActivate {
  constructor(private oauthSvc: OAuthService, private route:Route) {}

  canActivate(): boolean {
    debugger;
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
