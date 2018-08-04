import { PlayerregisterModel } from "./../../register-user/PlayerRegisterModel";
import { OAuthService } from "angular-oauth2-oidc";
import { Observable, of } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";

import * as jwt_decode from "jwt-decode";

@Injectable({
  providedIn: "root"
})
export class AuthService {
  constructor(private http: HttpClient) {}

  login(email: string, password: string): Observable<string> {
    this.http
      .post<TokenResponse>(
        `https://localhost:44310/api/players/token?Email=${email}&password=${password}`,
        {}
      )
      .subscribe(res => {
        console.log(res);
        localStorage.setItem("token", res.token);
        return of(res);
      });
    return of("none");
  }

  register(playerReg: PlayerregisterModel): Observable<any> {
    return this.http.post(
      "https://localhost:44310/api/players/register",
      playerReg
    );
  }

  logout(){
    let token = localStorage.getItem("access_token");
    if(token == undefined) { return; }
    let tokenInfo = jwt_decode(token);
    console.log(tokenInfo);
  }


  validateUser(): Observable<any> {
    let token = localStorage.getItem("access_token");
    var res = false;
    return this.http
      .post(
        'https://localhost:44310/api/players/validateToken',
        {},
        { headers: { Authorization: `Bearer ${token}` } }
      );
  }
}

class TokenResponse {
  token: string;
}
