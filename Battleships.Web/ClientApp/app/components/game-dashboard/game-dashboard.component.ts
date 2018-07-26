import { Component } from "@angular/core";
import { OnInit } from "../../../../node_modules/@angular/core";
import { ActivatedRoute } from "../../../../node_modules/@angular/router";

@Component({
  selector: "game-dashboard",
  templateUrl: "./game-dashboard.component.html"
})
export class GameDashboardComponent implements OnInit {
  constructor(private route: ActivatedRoute) {}

  gameId: number | undefined;

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      this.gameId = +params["id"];
    });
  }
}
