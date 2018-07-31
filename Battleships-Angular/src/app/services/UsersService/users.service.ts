import { Observable } from "rxjs";
import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { PlayerregisterModel } from "../../register-user/PlayerRegisterModel";
import { ResponseType } from "../../../../node_modules/@angular/http";

@Injectable({
  providedIn: "root"
})
export class UsersService {
  constructor(private http: HttpClient) {}

  login(): void {}

  register(player: PlayerregisterModel): void {
    const headers = new HttpHeaders().set('Content-Type', 'text/plain; charset=utf-8');
    this.http
      .post("https://localhost:44310/api/players", player, {headers: {'Content-Type': 'application/json'}})
      .subscribe(x => console.log(`User Successfull Registered`));
  }
}
