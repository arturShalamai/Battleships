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
      .post("https://localhost:44310/api/player/login", {
        Email: email,
        Password: password
      })
      .subscribe(res => {
        console.log(res);
        return of(res);
      });
    return of("none");
  }
}
