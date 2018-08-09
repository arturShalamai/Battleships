import { debug } from "util";
import { PlayerRegisterModel } from "./../../register-user/PlayerRegisterModel";
import { OAuthService } from "angular-oauth2-oidc";
import { Observable, of } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";

import * as jwt_decode from "jwt-decode";

@Injectable({
  providedIn: "root"
})
export class AuthService {
  constructor(private http: HttpClient, private oAuthSvc: OAuthService) {}

  login(email: string, password: string): Observable<TokenResponse> {
    return this.http.post<TokenResponse>(
      `https://localhost:44310/api/players/token?Email=${email}&password=${password}`,
      {}
    );
  }

  register(playerReg: PlayerRegisterModel): Observable<any> {
    return this.http.post<any>(
      "https://localhost:44310/api/players/register",
      playerReg
    );
  }

  logout() {
    let token = localStorage.getItem("access_token");
    if (token == undefined) {
      return;
    }
    if (this.oAuthSvc.hasValidAccessToken()) {
      this.oAuthSvc.logOut();
    } else {
      localStorage.setItem("access_token", "");
    }
  }

  validateUser(): Observable<any> {
    let token = localStorage.getItem("access_token");
    var res = false;
    return this.http.post(
      "https://localhost:44310/api/players/validateToken",
      {},
      { headers: { Authorization: `Bearer ${token}` } }
    );
  }

  getTokenInfo(): any{
    return jwt_decode(localStorage.getItem("access_token"));
  }


}

class TokenResponse {
  token: string;
}
