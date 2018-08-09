import { debug } from 'util';
import { GameService } from "./../services/Game/game.service";
import { Component, OnInit, Input } from "@angular/core";

@Component({
  selector: "app-game-surrender",
  templateUrl: "./game-surrender.component.html",
  styleUrls: ["./game-surrender.component.css"]
})
export class GameSurrenderComponent implements OnInit {
  @Input()gameId: string;

  constructor(private gameSvc: GameService) {}

  ngOnInit() {}

  surrender() {
    debugger;
    this.gameSvc.surrender(this.gameId).subscribe(suc => {
      console.log("Surrendered");
    });
  }
}
