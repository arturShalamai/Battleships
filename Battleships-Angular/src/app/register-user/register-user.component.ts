import { debug } from 'util';
import { PlayerRegisterModel } from './PlayerRegisterModel';
import { Observable } from "rxjs";
import { AuthService } from "./../services/Auth/auth.service";
import { Component, OnInit, Output, EventEmitter } from "@angular/core";
import { OAuthService } from "angular-oauth2-oidc";

@Component({
  selector: "app-register-user",
  templateUrl: "./register-user.component.html",
  styleUrls: ["./register-user.component.css"]
})
export class RegisterUserComponent implements OnInit {
  @Output() public successfullRegister = new EventEmitter<any>();
  @Output() public login = new EventEmitter<any>();

  public registerModel: PlayerRegisterModel;

  constructor(private authSvc: AuthService) {}

  ngOnInit() {
    this.registerModel = new PlayerRegisterModel(
      "Ivan@mail.com",
      "Ivan",
      "Petrenko",
      "Ivan112",
      "qwerty12",
      "qwerty12"
    );
  }

 Register() {
    debugger;
    this.authSvc.register(this.registerModel).subscribe(
      suc => {
        debugger;
        this.successfullRegister.emit(null);
      },
      err => {
        debugger;
        this.registerModel = new PlayerRegisterModel('', '', '', '', '', '');
      }
    );
  }


  public goToLogin(){
    this.login.emit(null);
  }
}
