import { OAuthService } from 'angular-oauth2-oidc';
import { AuthService } from './../services/Auth/auth.service';
import { Component, OnInit } from "@angular/core";

@Component({
  selector: "app-login",
  templateUrl: "./login.component.html",
  styleUrls: ["./login.component.css"]
})
export class LoginComponent implements OnInit {
  public isSignUp: boolean;
  public email:string;
  public password : string;

  constructor(private authSvc:AuthService) {}

  ngOnInit() {
  }

  login() : void{
    this.authSvc.login(this.email, this.password).subscribe(x => {console.log("User Logined")} );
  }


  signUp(): void {
    this.isSignUp = true;
  }
}
