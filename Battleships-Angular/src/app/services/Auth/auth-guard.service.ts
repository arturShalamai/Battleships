import { of, Observable } from "rxjs";
import { AuthService } from "./auth.service";
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
import "rxjs-compat";

@Injectable({
  providedIn: "root"
})
export class AuthGuardService implements CanActivate {
  constructor(private authSvc: AuthService, private router: Router) {}

  canActivate() {
    // debugger;
    return this.authSvc
      .validateUser()
      .map(() => {
        console.log('Validated;')
        return true;
      })
      .catch(() => {
        console.log('UnValidated;')
        this.router.navigate(['/login']);
        return Observable.of(false);
      });
  }

  // canLoad(): boolean {
  //   debugger;
  //   debugger;

  //   return false;
  // }
}
