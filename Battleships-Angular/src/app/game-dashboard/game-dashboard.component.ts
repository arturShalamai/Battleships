import { GameService } from "./../services/Game/game.service";
import { GameDashboardService } from "./../services/GameDashboard/game-dashboard.service";
import { Component, OnInit } from "@angular/core";
import { HubConnectionBuilder } from "@aspnet/signalr";

@Component({
  selector: "app-game-dashboard",
  templateUrl: "./game-dashboard.component.html",
  styleUrls: ["./game-dashboard.component.css"]
})
export class GameDashboardComponent implements OnInit {
  gameId: string;

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

  constructor(private gameSvc: GameService) {}

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

  ngOnInit() {}
}
