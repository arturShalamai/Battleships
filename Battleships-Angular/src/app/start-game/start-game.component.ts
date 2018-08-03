import { AuthService } from "./../services/Auth/auth.service";
import { Component, OnInit, AfterViewInit } from "@angular/core";
import { Observable } from "rxjs";
import { Router, ActivatedRoute } from "../../../node_modules/@angular/router";

@Component({
  selector: "app-start-game",
  templateUrl: "./start-game.component.html",
  styleUrls: ["./start-game.component.css"]
})
export class StartGameComponent implements OnInit, AfterViewInit {
  private images: string[] = [
    "/assets/images/home/bg1.jpg",
    "/assets/images/home/bg2.png",
    "/assets/images/home/bg3.png"
  ];
  public bgImage: string = this.images[0];

  public isSignUp: boolean;

  constructor(
    private router: Router,
    private authSvc: AuthService,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    debugger;
    let access_token = this.route.snapshot.queryParams["access_token"];
    if(access_token != undefined) { localStorage.setItem("access_token", access_token); }
    // setInterval(() => {
    //   var randomImage = this.getRandomImage();
    //   console.log(randomImage);
    //   this.bgImage = randomImage;
    // }, 3000);
  }
  //http://localhost:4200/
  //#id_token=eyJhbGciOiJSUzI1NiIsImtpZCI6IjFmNjgxNzk3OGIxY2MzMmI5MjE4YjEyYzU0NTEwNWY1IiwidHlwIjoiSldUIn0.eyJuYmYiOjE1MzMzMTI5NDEsImV4cCI6MTUzMzMxMzI0MSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNjIiLCJhdWQiOiIyNmZhZDdlOS05OTVjLTRiNmItOWQxNi1jYzJjYTkzZDE5Y2YiLCJub25jZSI6IjNlb3p0MTd5c25BVG5UU1poOEF3MFlIZUttYTNrVHJvbUNJSWFVMnUiLCJpYXQiOjE1MzMzMTI5NDEsImF0X2hhc2giOiIyLTgtRTFJRWRVZmNyZVpqUEkwMHZRIiwic2lkIjoiMzE0ZTc0ZjA4NzUwYWE4OWM4MzczODVlNDA0MGJhZTIiLCJzdWIiOiJmZGQ5MGVhZC0wZDhjLTQ1OGMtYTJlZC1jZjBjODk4MWViOWIiLCJhdXRoX3RpbWUiOjE1MzMzMDI0MDYsImlkcCI6ImxvY2FsIiwiYW1yIjpbInB3ZCJdfQ.SehPKWH5dTYsGvlSQ657P5mLuO0MUk0Gm0FyJgFb37VP5h4F-a7K-ARFblwOymNFicdm7IoD0PZQaOR1a-Oz7QdgOj3fWuLiRLYSFbofDukBvtmAtgIRZKSnz2pne8orUx2kpI1dDdBiKO7jCYXDILjV3Lt3MS9IYrAaKgKXbDPn_EfKrbgjiv00eKgSuzzOvKY1fHMo2mQ24EIrNvdIoUooLPK6NWpi6mpJFnM7UN4xLdwIoNGJEjSkj1UicfIIchmfI7a8ULOa4gvk8khRHkPy1rzOzO7KjdGdRacpyMRMOju9dyWR4K5aAbkELk3FVS9SQuWtLTAXhfXu1D6IaQ
  //&access_token=eyJhbGciOiJSUzI1NiIsImtpZCI6IjFmNjgxNzk3OGIxY2MzMmI5MjE4YjEyYzU0NTEwNWY1IiwidHlwIjoiSldUIn0.eyJuYmYiOjE1MzMzMTI5NDEsImV4cCI6MTUzMzMxNjU0MSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNjIiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo0NDM2Mi9yZXNvdXJjZXMiLCJjbGllbnRfaWQiOiIyNmZhZDdlOS05OTVjLTRiNmItOWQxNi1jYzJjYTkzZDE5Y2YiLCJzdWIiOiJmZGQ5MGVhZC0wZDhjLTQ1OGMtYTJlZC1jZjBjODk4MWViOWIiLCJhdXRoX3RpbWUiOjE1MzMzMDI0MDYsImlkcCI6ImxvY2FsIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvZ2l2ZW5uYW1lIjoiQXJ0dXJAbWFpbC5jb20iLCJzY29wZSI6WyJvcGVuaWQiLCJwcm9maWxlIl0sImFtciI6WyJwd2QiXX0.VGCbqJZTQKD5nRJVRFeh0TGo0EPLEo7FBOIMH-VnV7trO6eo0swAYz3EFpkmTmE0Cq_qKJQAXVGzeRQ_2g9VbVupto9LOjtUWrSJQESrbZTEqh0HRmAaSfve4ACMwzujsIZtCRwnQ65T2Q_ZcWvisO6_ANxKfE-zp-aJ-BE-W4E_9J-cH79o9DKSE9tNywsuYiR2SQOF335hzUjPDO51fSXlEDdRCTJzCJ_Zpg8grKAuSTSyMWIHf0NyfuQguB52LQTShTCEz7V8DzwE6eL1Of4IjJ9C9sBasSauUTP3M8L7mFa_OWQxVEio34UBf1guYOhMjxDEwdFSUy5MJZbfOg
  //&token_type=Bearer
  //&expires_in=3600
  //&scope=openid%20profile
  //&state=3eozt17ysnATnTSZh8Aw0YHeKma3kTromCIIaU2u
  //&session_state=wgY1CUVAG-aCcZHvoPCudSrNcOFW3gWzNpEpuVHwtTA.f8a06be505dfed4f7f01f71f2f02b563

  ngAfterViewInit(): void {
    debugger;
    this.authSvc.validateUser().subscribe(res => {
      this.router.navigate(["/game/1"]);
    });
  }

  register(): void {
    this.isSignUp = true;
  }

  login() {
    this.isSignUp = false;
  }
  redirectToDashboard() {
    this.router.navigate(["/game/1"]);
  }

  private getRandomImage(): string {
    return this.images[Math.floor(Math.random() * this.images.length)];
  }
}
