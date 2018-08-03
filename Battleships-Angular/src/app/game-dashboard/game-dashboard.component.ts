import { GameService } from "./../services/Game/game.service";
import { GameDashboardService } from "./../services/GameDashboard/game-dashboard.service";
import { Component, OnInit } from "@angular/core";
import { HubConnectionBuilder } from "@aspnet/signalr";
import { ActivatedRoute } from "../../../node_modules/@angular/router";

@Component({
  selector: "app-game-dashboard",
  templateUrl: "./game-dashboard.component.html",
  styleUrls: ["./game-dashboard.component.css"]
})
export class GameDashboardComponent implements OnInit {
  gameId: string;

  showMenu: boolean = true;

  fieldOne: boolean[][] = [
    [true, true, false, true, false, true],
    [true, true, false, true, false, true],
    [true, true, false, true, false, true],
    [true, true, false, true, false, true],
    [true, true, false, true, false, true],
    [true, true, false, true, false, true],
    [true, true, false, true, false, true]
  ];

  fieldTwo: boolean[][] = [
    [false, false, false, false, false, false],
    [false, false, false, false, false, false],
    [false, false, false, false, false, false],
    [false, false, false, false, false, false],
    [false, false, false, false, false, false],
    [false, false, false, false, false, false],
    [false, false, false, false, false, false]
  ];

  constructor(private gameSvc: GameService, private route: ActivatedRoute) {}

  ngOnInit() {
    debugger;
    var gameId = this.route.snapshot.params["id"];
    this.gameSvc.checkParticipation(gameId).then(res => {
      debugger;
      if (gameId != undefined && this.gameSvc.checkParticipation(gameId)) {
        this.gameId == gameId;
        this.showMenu = false;
        console.log("Game id : ", gameId);
      }
    });
  }

  fire(index: number) {
    this.gameSvc.fire(this.gameId, index).subscribe(res => {
      debugger;
      console.log(`Successfully fired to ${this.gameId} at ${res}`);
    });
  }

  srtGameId(game: string) {
    this.gameId = game;
    console.log("Game Id was chnged to ", game);
  }
}
