import { TurnResult } from "./TurnResult";
import { SignalRService } from "./../services/SignalR/signal-r.service";
import { GameInfoModel } from "./../Models/GameInfoModel";
import { HttpClient } from "@angular/common/http";
import { GameService } from "./../services/Game/game.service";
import { GameDashboardService } from "./../services/GameDashboard/game-dashboard.service";
import { Component, OnInit, OnDestroy } from "@angular/core";
import { ActivatedRoute, Router } from "../../../node_modules/@angular/router";
import { ShipsFieldModel } from "../Models/ShipsFieldModel";
import { debug } from "util";

@Component({
  selector: "app-game-dashboard",
  templateUrl: "./game-dashboard.component.html",
  styleUrls: ["./game-dashboard.component.css"]
})
export class GameDashboardComponent implements OnInit, OnDestroy {
  gameId: string;
  gameInfo: GameInfoModel;
  showMenu: boolean = true;

  urlVisible = true;

  shipsLeft = 16;

  newGameId = "";

  userField = " ".repeat(42);
  secondUserFieldString = " ".repeat(42);
  usersReady = 0;
  numbOfRows = Array(7).fill(1);
  numbOfCols = Array(6).fill(1);

  userId: string;

  gameEnd = false;
  victory = false;

  secondPlayerStatus = "#eee";

  constructor(
    private gameSvc: GameService,
    private signalRSvc: SignalRService,
    private route: ActivatedRoute,
    private router: Router,
    private client: HttpClient
  ) {}

  ngOnInit() {
    this.route.params.subscribe(params => {
      debugger;
      let idParam = params["id"];
      this.userId = this.gameSvc.getUserId();
      if (idParam != undefined) {
        this.gameSvc.checkParticipation(idParam).subscribe(suc => {
          if (suc) {
            this.showMenu = false;
            this.gameId = idParam;
            this.loadCurrGame();
          }
        });
      }
    });

    this.subscribeToSignalREvents();
  }

  subscribeToSignalREvents() {
    this.signalRSvc.gamesConnection.on("onGameCrated", res => {
      this.newGameId = res;
    });

    this.signalRSvc.gamesConnection.on("onHit", res => {
      let turnRes = res as TurnResult;
      console.log("Hited : ", res);
      this.onHit(turnRes);
    });

    this.signalRSvc.gamesConnection.on("onGameEnd", res => {
      debugger;
      this.gameEnd = true;
      this.victory = res.winner == this.userId ? true : false;
      console.log("Game End", res);
    });

    this.signalRSvc.gamesConnection.on("onPlayerJoined", res => {
      debugger;
      this.onPlayerJoined();
    });

    this.signalRSvc.gamesConnection.on("onPlayerReady", res => {
      this.usersReady++;
      console.log(Date.now().toLocaleString(), "Second player ready.");
    });
  }

  loadCurrGame() {
    this.gameSvc.getGameInfo(this.gameId).subscribe(res => {
      this.gameInfo = res;
    });
  }

  fire(index: number) {
    if (this.secondUserFieldString[index] == ' ') {
      this.gameSvc.fire(this.gameId, index).subscribe(res => {
        debugger;
        var sym = res.result == "Hit" ? "x" : "0";
        this.secondUserFieldString = replaceAt(
          this.secondUserFieldString,
          index,
          sym
        );
        console.log(`Successfully fired to ${this.gameId} at ${res.result}`);
      });
    }
  }

  confirmShips() {
    let shipsModel = new ShipsFieldModel();
    shipsModel.GameId = this.gameId;
    shipsModel.Field = this.userField;
    this.gameSvc.submitShips(shipsModel).subscribe(res => {
      console.log("Ships position accepted");
      this.usersReady++;
    });
  }

  placeShip(index: number) {
    if (this.shipsLeft > 0 && this.userField[index] != "█") {
      this.userField = replaceAt(this.userField, index, "█");
      console.log("Ship placed : ", index);
      console.log(this.userField);
      this.shipsLeft--;
    }
  }

  onGameStarted(gameId: string) {
    this.shipsLeft = 16;
    this.gameId = gameId;
    this.showMenu = false;
    this.loadCurrGame();
    console.log("Stated game ", gameId);
  }

  onGameJoined(gameId: string) {
    debugger;
    this.showMenu = false;
    this.secondPlayerStatus = "#64dd17";
    this.gameId = gameId;
    this.loadCurrGame();
    this.urlVisible = false;
    console.log("Joined  game ", gameId);
  }

  onLoggedOut() {
    debugger;
    this.router.navigate(["/login"]);
  }

  onPlayerJoined() {
    this.secondPlayerStatus = "#64dd17";
  }

  onHit(turnRes: TurnResult) {
    debugger;
    if (turnRes.result == "Hit") {
      this.shipsLeft--;
      this.userField = replaceAt(this.userField, turnRes.position, "x");
    } else {
      this.userField = replaceAt(this.userField, turnRes.position, "0");
    }
  }

  ngOnDestroy(): void {
    debugger;
    this.gameSvc.closeConn();
  }
}

function replaceAt(s, n, t) {
  return s.substring(0, n) + t + s.substring(n + 1);
}
