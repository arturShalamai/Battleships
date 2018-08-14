import "rxjs-compat";
import { debug } from "util";
import { AuthService } from "./../services/Auth/auth.service";
import { Component, OnInit, AfterViewInit } from "@angular/core";
import { Observable } from "rxjs";
import { Router, ActivatedRoute } from "../../../node_modules/@angular/router";

@Component({
  selector: "app-start-game",
  templateUrl: "./start-game.component.html",
  styleUrls: ["./start-game.component.css"]
})
export class StartGameComponent implements OnInit {
  private images: string[] = [
    "/assets/images/home/bg1.jpg",
    "/assets/images/home/bg2.jpg",
    "/assets/images/home/bg3.jpg"
  ];
  public bgImage: string = this.images[0];

  public isSignUp: boolean;

  constructor(
    private router: Router,
    private authSvc: AuthService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    let access_token = localStorage.getItem('access_token');
    
    setInterval(() => { this.bgImage = this.getRandomImage();}, 15000);

    this.authSvc.validateUser().subscribe(res => { this.redirectToDashboard(); })
  }

  showRegister(): void {
    this.isSignUp = true;
  }

  showLogin() {
    this.isSignUp = false;
  }

  public redirectToDashboard() {
    this.router.navigate(["/game"]);
  }

  private getRandomImage(): string {
    return this.images[Math.floor(Math.random() * this.images.length)];
  }
}
