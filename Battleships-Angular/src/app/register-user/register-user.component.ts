import { AuthService } from './../services/Auth/auth.service';
import { UsersService } from "./../services/UsersService/users.service";
import { Component, OnInit, Output, EventEmitter } from "@angular/core";
import { OAuthService } from "angular-oauth2-oidc";
import { PlayerregisterModel } from "./PlayerRegisterModel";

@Component({
  selector: "app-register-user",
  templateUrl: "./register-user.component.html",
  styleUrls: ["./register-user.component.css"]
})
export class RegisterUserComponent implements OnInit {
  @Output() public login = new EventEmitter<any>();
  public registerModel: PlayerregisterModel;

  constructor(private authSvc: AuthService) {}

  ngOnInit() {
    this.registerModel = new PlayerregisterModel(
      "Ivan@mail.com",
      "Ivan",
      "Petrenko",
      "Ivan112",
      "qwerty12",
      "qwerty12"
    );
  }

  goToLogin() {
    this.login.emit(null);
  }

  Register(): void {
    debugger;
    this.authSvc.register(this.registerModel);
  }
}
