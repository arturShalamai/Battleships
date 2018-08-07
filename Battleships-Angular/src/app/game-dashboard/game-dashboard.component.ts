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

  userField = " ".repeat(42);
  enemyFieldString = '█ █   █     █ █████        ██  ████       ';
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
  ) {
    // this.route.params.subscribe(param => {
    //   this.checkForParticipation();
    // });
  }

  ngOnInit() {
    var gameId = this.route.snapshot.params["id"];
    if (gameId != undefined) {
      this.gameId = gameId;
      // checkForParticipation(this.gameId);
    }
    // this.gameSvc.getGameInfo(gameId).subscribe(res => {
    //   debugger;
    //   this.enemyFieldString = res.enemyField;
    //   this.userField = res.playerField;
    //   console.log(res);
    // });
  }

  getGame() {
    debugger;
    let token = localStorage.getItem("access_token");
    this.client
      .get(
        "https://localhost:44310/api/values/84616006-81e5-4f1d-9506-00d2ceabc4e1",
        {
          headers: {
            Authorization: `Bearer ${token}`
          },
          responseType: "text"
        }
      )
      .subscribe(res => {});
  }

  numbOfRows: number = 7;
  numbOfCols: number = 6;

  private checkForParticipation(gameId : string) {
    this.gameSvc.checkParticipation(gameId).then(res => {
      if (res == true) {
        this.gameId = gameId;
        this.showMenu = false;
        console.log("Game id : ", gameId);
      }
    });
  }

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

  srtGameId(game: string) {
    this.gameId = game;
    this.router.navigate([`/game/${this.gameId}`]);
    console.log("Game Id was chnged to ", game);
  }

  gameJoined(gameId: string){
    this.showMenu = false;
    this.gameId = gameId;
    // this.checkForParticipation();
  }

}

function replaceAt(s, n, t) {
  return s.substring(0, n) + t + s.substring(n + 1);
}
