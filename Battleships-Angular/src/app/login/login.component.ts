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

  submitSuccess() {
    this.loginSuccess.emit(null);
  }

  login(): void {
    this.authSvc.login(this.email, this.password).subscribe(res =>{
      this.submitSuccess();
    });
  }

  platformLogin() {
    debugger;
    this.oauthSvc.initImplicitFlow();
  }

  signUp(): void {
    this.register.emit(null);
  }
}
