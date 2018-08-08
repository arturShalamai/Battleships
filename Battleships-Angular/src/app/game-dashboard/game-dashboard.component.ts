import { GameInfoModel } from "./../Models/GameInfoModel";
import { HttpClient } from "@angular/common/http";
import { GameService } from "./../services/Game/game.service";
import { GameDashboardService } from "./../services/GameDashboard/game-dashboard.service";
import { Component, OnInit, OnDestroy } from "@angular/core";
// import { HubConnectionBuilder } from "@aspnet/signalr";
import { ActivatedRoute, Router } from "../../../node_modules/@angular/router";
import { ShipsFieldModel } from "../Models/ShipsFieldModel";
import { debug } from "util";

@Component({
  selector: "app-game-dashboard",
  templateUrl: "./game-dashboard.component.html",
  styleUrls: ["./game-dashboard.component.css"]
})
export class GameDashboardComponent implements OnInit, OnDestroy {
  ngOnDestroy(): void {
    debugger;
    this.gameSvc.closeConn();
  }
  gameId: string;

  showMenu: boolean = true;

  gameInfo: GameInfoModel;

  userField = " ".repeat(42);
  enemyFieldString = "█ █   █     █ █████        ██  ████       ";
  enemyField: boolean[][] = [
    [null, null, null, null, null, null],
    [null, null, null, null, null, null],
    [null, null, null, null, null, null],
    [null, null, null, null, null, null],
    [null, null, null, null, null, null],
    [null, null, null, null, null, null],
    [null, null, null, null, null, null]
  ];

  constructor(
    private gameSvc: GameService,
    private route: ActivatedRoute,
    private router: Router,
    private client: HttpClient
  ) {}

  ngOnInit() {
    this.route.params.subscribe(params => {
      debugger;
      let idParam = params["id"];
      if (idParam != undefined) {
        this.gameSvc.checkParticipation(idParam).subscribe(suc => {
          debugger;
          if (suc) {
            this.showMenu = false;
            this.gameId = idParam;
            this.loadCurrGame();
          }
        });
      }
    });
  }

  loadCurrGame() {
    this.gameSvc.getGameInfo(this.gameId).subscribe(res => {
      this.gameInfo = res;
    });
  }

  numbOfRows: number = 7;
  numbOfCols: number = 6;

  fire(index: number) {
    this.gameSvc.fire(this.gameId, index).subscribe(res => {
      debugger;
      var sym = res.result == "Hit" ? "x" : "0";
      this.enemyFieldString = replaceAt(this.enemyFieldString, index, sym);
      console.log(`Successfully fired to ${this.gameId} at ${res.result}`);
    });
  }

  confirmShips() {
    // debugger;
    let shipsModel = new ShipsFieldModel();
    shipsModel.GameId = this.gameId;
    shipsModel.Field = this.userField;
    this.gameSvc.submitShips(shipsModel).subscribe(res => {
      console.log("Ships position accepted");
    });
  }

  placeShip(index: number) {
    this.userField = replaceAt(this.userField, index, "█");
    console.log("Ship placed : ", index);
    console.log(this.userField);
  }

  onGameStarted(gameId: string) {
    this.gameId = gameId;
    this.showMenu = false;
    this.loadCurrGame();
    console.log("Stated game ", gameId);
  }

  onGameJoined(gameId: string) {
    debugger;
    this.showMenu = false;
    this.gameId = gameId;
    this.loadCurrGame();
    console.log("Joined  game ", gameId);
  }
}

function replaceAt(s, n, t) {
  return s.substring(0, n) + t + s.substring(n + 1);
}
