import { EventEmitter, Output } from "@angular/core";
import { GameService } from "./../services/Game/game.service";
import { Component, OnInit } from "@angular/core";
import { Router } from "../../../node_modules/@angular/router";

@Component({
  selector: "app-join-game",
  templateUrl: "./join-game.component.html",
  styleUrls: ["./join-game.component.css"]
})
export class JoinGameComponent implements OnInit {
  gameId: string;

  @Output() gameJoined = new EventEmitter<string>();

  constructor(private gameSvc: GameService, private router: Router) {}

  ngOnInit() {}

  joinGame() {
    this.gameSvc.joinGame(this.gameId).subscribe(res => {
      debugger;
      this.gameJoined.emit(this.gameId);
    });
  }
}
