import { GameService } from "./../services/Game/game.service";
import { GameDashboardService } from "./../services/GameDashboard/game-dashboard.service";
import { Component, OnInit } from "@angular/core";
import { HubConnectionBuilder } from "@aspnet/signalr";
import { ActivatedRoute, Router } from "../../../node_modules/@angular/router";
import { ShipsFieldModel } from "../Models/ShipsFieldModel";
import { debug } from "util";

@Component({
  selector: "app-game-dashboard",
  templateUrl: "./game-dashboard.component.html",
  styleUrls: ["./game-dashboard.component.css"]
})
export class GameDashboardComponent implements OnInit {
  gameId: string;

  showMenu: boolean = true;

  userField = '█ ██  █   █ █  ████       █ █ █ █     █ █ ';
  enemyFieldString = ' '.repeat(42);
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
    private router: Router
  ) {
    this.route.params.subscribe(param => {
      this.checkForParticipation();
    });
  }

  ngOnInit() {
    
  }

  numbOfRows : number = 7;
  numbOfCols : number = 6;

  private checkForParticipation() {
    debugger;
    var gameId = this.route.snapshot.params["id"];
    this.gameSvc.checkParticipation(gameId).then(res => {
      debugger;
      if (gameId != undefined && this.gameSvc.checkParticipation(gameId)) {
        this.gameId = gameId;
        this.showMenu = false;
        console.log("Game id : ", gameId);
      }
    });
  }

  fire(index: number) {
    this.gameSvc.fire(this.gameId, index).subscribe(res => {
      debugger;
      var sym = res.result== 'Hit' ? 'x' : '0';
      this.enemyFieldString = replaceAt(this.enemyFieldString, index, sym);
      console.log(`Successfully fired to ${this.gameId} at ${res.result}`);
    });
  }

  confirmShips(){
    debugger;
    let shipsModel = new ShipsFieldModel();
    shipsModel.GameId = this.gameId;
    shipsModel.Field = this.userField;
    this.gameSvc.submitShips(shipsModel).subscribe(res => console.log('Ships position accepted'));
  }

  placeShip(index:number){
    this.userField = replaceAt(this.userField, index, '█');
    console.log('Ship placed : ', index);
    console.log(this.userField);
  }

  srtGameId(game: string) {
    this.gameId = game;
    console.log("Game Id was chnged to ", game);
  }
}

function replaceAt(s, n, t) {
  return s.substring(0, n) + t + s.substring(n + 1);
}