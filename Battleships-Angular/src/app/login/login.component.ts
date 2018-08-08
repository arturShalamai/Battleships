import { OAuthService } from "angular-oauth2-oidc";
import { AuthService } from "./../services/Auth/auth.service";
import { Component, OnInit, Output, EventEmitter } from "@angular/core";
import { Router } from "../../../node_modules/@angular/router";

@Component({
  selector: "app-login",
  templateUrl: "./login.component.html",
  styleUrls: ["./login.component.css"]
})
export class LoginComponent implements OnInit {
  public isSignUp: boolean;
  public email: string;
  public password: string;

  @Output() public register = new EventEmitter<any>();
  @Output() public loginSuccess = new EventEmitter<any>();

  constructor(
    private oauthSvc: OAuthService,
    private authSvc: AuthService,
    private router: Router
  ) {}

  ngOnInit() {}

  login(): void {
    debugger;
    this.authSvc.login(this.email, this.password).subscribe(
      suc => {
        debugger;
        localStorage.setItem('access_token', suc.token);
        this.loginSuccess.emit(null);
      },
      err => {
        debugger;
        this.email = '';
        this.password = '';
      }
    );
  }

  platformLogin() {
    debugger;
    this.oauthSvc.initImplicitFlow();
  }

  signUp(): void {
    this.register.emit(null);
  }
}
