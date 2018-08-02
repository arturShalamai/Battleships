import { GameService } from './../services/Game/game.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '../../../node_modules/@angular/router';

@Component({
  selector: 'app-join-game',
  templateUrl: './join-game.component.html',
  styleUrls: ['./join-game.component.css']
})
export class JoinGameComponent implements OnInit {

  gameId : string;

  constructor(private gameSvc:GameService,
              private router:Router) { }

  ngOnInit() {
  }

  joinGame(){
    debugger;
    this.gameSvc.joinGame(this.gameId).subscribe(res => this.router.navigate(['/game-dashboard']));
  }

}
