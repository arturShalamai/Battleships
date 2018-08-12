import { AuthService } from "./../services/Auth/auth.service";
import { Component, OnInit } from "@angular/core";
import { Router, ActivatedRoute, Params } from "@angular/router";

@Component({
  selector: "app-signin-platform",
  templateUrl: "./signin-platform.component.html",
  styleUrls: ["./signin-platform.component.css"]
})
export class SigninPlatformComponent implements OnInit {
  constructor(
    private authSvc: AuthService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    let access_token = '';

    let fragment = this.route.fragment.subscribe(fragment => {
      access_token =fragment.substring(fragment.indexOf('access_token='), fragment.indexOf('&token_type')).slice(13); 
        console.log(access_token);
    });

    // this.route.data.subscribe(params => {
    //   console.log(params);
    //   access_token = params["access_token"];
    // });

    if (access_token != null) {
      this.authSvc.signinPlatform(access_token).subscribe(suc => {
        localStorage.setItem("access_token", access_token);
      });
    }
    this.router.navigate(["/login"]);
  }
}




// access_token=eyJhbGciOiJSUzI1NiIsImtpZCI6IjFmNjgxNzk3OGIxY2MzMmI5MjE4YjEyYzU0NTEwNWY1IiwidHlwIjoiSldUIn0.eyJuYmYiOjE1MzQwMTk4MzEsImV4cCI6MTUzNDAyMzQzMSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNjIiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo0NDM2Mi9yZXNvdXJjZXMiLCJjbGllbnRfaWQiOiJjbGllbnQtN2QyZmY2YTItYTFhYi00YTdjLTk5NjctNjA5Mzc2MTM1ZjUwIiwic3ViIjoiZmRkOTBlYWQtMGQ4Yy00NThjLWEyZWQtY2YwYzg5ODFlYjliIiwiYXV0aF90aW1lIjoxNTM0MDEzNjg0LCJpZHAiOiJsb2NhbCIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkRldmVsb3BlciIsInNjb3BlIjpbIm9wZW5pZCIsInByb2ZpbGUiXSwiYW1yIjpbInB3ZCJdfQ.p1tRBjIBgVFuHQPMBONBe_Q8H3m4TIq8XMEy4X7qQ6VS6RNbWFQUew5K4xLmKkgPjuI_zgSsIdMApmnsMSc0VTHfDms0grEP4ruJhc2wLX48bH0OySUnqo4pEYi6TwCwvM1RVQzuQjVdHf6l-PD0_n8iLNkHmMpFjaxT6PpGBSZ8xO_yppBUoz478FXN0oiBMCMSZKdtyDZOc1HU1zpZHseihay1LU8u8bDwRxwI76LmpB-iy-0oLbho_9in9dRzwWlVht6Xa19-KP06ikPqUQWQJUXR1gYKWfGM2FaNKBtFzgKBELDmiHllq3fqnz1_nh0k1YgHtIbbKbcGtkwiCA
// access_token=eyJhbGciOiJSUzI1NiIsImtpZCI6IjFmNjgxNzk3OGIxY2MzMmI5MjE4YjEyYzU0NTEwNWY1IiwidHlwIjoiSldUIn0.eyJuYmYiOjE1MzQwMjE5NjksImV4cCI6MTUzNDAyNTU2OSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzNjIiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo0NDM2Mi9yZXNvdXJjZXMiLCJjbGllbnRfaWQiOiJjbGllbnQtN2QyZmY2YTItYTFhYi00YTdjLTk5NjctNjA5Mzc2MTM1ZjUwIiwic3ViIjoiZmRkOTBlYWQtMGQ4Yy00NThjLWEyZWQtY2YwYzg5ODFlYjliIiwiYXV0aF90aW1lIjoxNTM0MDIxOTU2LCJpZHAiOiJsb2NhbCIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6IkRldmVsb3BlciIsInNjb3BlIjpbIm9wZW5pZCIsInByb2ZpbGUiXSwiYW1yIjpbInB3ZCJdfQ.i7BTPgcXJXSvnCAIH95y52VF92JxYBMLcXxJ5jMV-Qy4g00DHNx6okGkIQ9Vgsfbyi_wEA0KRM_waL3xE14nYJCP4dRnZLlbR0vQHY-hd_w5fXSxUOaNIlKZaaxVhGyi-N6P9cZSplu1L-qhoBiwUvSI88xo2Kd39kxDDVKu3lT5x7Spv36ChFiWhxvg7R8Mhn_f_qiVQCHlyjdgvCuj52-DHMcuB3P1mEw2htFmzFE1PkLZQ_U6rKBwvgKgg4QEmNL5qNj89iLQxWM95htoukKdh_b4ZmxF-cs26U4cbR4FQSaSw0HFGuqCR20MpCUK2Ee8Lt71NZwF70teuv2Q3w