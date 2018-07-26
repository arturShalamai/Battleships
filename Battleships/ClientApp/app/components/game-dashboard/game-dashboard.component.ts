import { Component, OnInit } from "@angular/core";
import { ActivatedRoute } from "../../../node_modules/@angular/router";

@Component({
  selector: "game-dashboard",
  templateUrl: "./game-dashboard.component.html"
})
export class GameDashboardComponent implements OnInit {
  constructor(private route: ActivatedRoute) {}

  userPass: number;

  ngOnInit(): void {
    this.userPass = +this.route.snapshot.paramMap.get("id");
    console.log(`Id : ${this.userPass}`);
  }
}
