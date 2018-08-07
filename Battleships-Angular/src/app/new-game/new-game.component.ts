import { GameService } from "./../services/Game/game.service";
import { Component, OnInit } from "@angular/core";
import { Router } from "../../../node_modules/@angular/router";

@Component({
  selector: "app-new-game",
  templateUrl: "./new-game.component.html",
  styleUrls: ["./new-game.component.css"]
})
export class NewGameComponent implements OnInit {
  constructor(private gameSvc: GameService, private router: Router) {}

  ngOnInit() {}

  createGame() {
    this.gameSvc.createGame().subscribe(res => {
      // debugger;
      console.log(`Game created with id : ${res.id}`);
      this.router.navigate([`/game/${res.id}`]);
    });
  }
}
