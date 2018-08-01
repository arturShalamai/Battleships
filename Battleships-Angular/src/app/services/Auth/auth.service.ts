import { Observable, of } from "rxjs";
import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";

@Injectable({
  providedIn: "root"
})
export class AuthService {
  constructor(private http: HttpClient) {}

  login(email: string, password: string): Observable<string> {
    this.http
      .post<TokenResponse>(`https://localhost:44310/api/players/token?Email=${email}&password=${password}`, {})
      .subscribe(res => {
        console.log(res);
        localStorage.setItem("access_token", res.token);
        return of(res);
      });
    return of("none");
  }
}

class TokenResponse{
  token: string;
}