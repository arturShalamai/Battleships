import { ShipsFieldModel } from './../Models/ShipsFieldModel';
import { GameService } from './../services/Game/game.service';
import { Component, OnInit, Injectable, Input } from '@angular/core';

@Component({
  selector: 'app-field',
  templateUrl: './field.component.html',
  styleUrls: ['./field.component.css']
})
export class FieldComponent implements OnInit {

  userField = ' '.repeat(42);

  enemyField: boolean[][] = [
    [false, false, false, false, false, false],
    [false, false, false, false, false, false],
    [false, false, false, false, false, false],
    [false, false, false, false, false, false],
    [false, false, false, false, false, false],
    [false, false, false, false, false, false],
    [false, false, false, false, false, false]
  ];

  @Input() public gameId:string;

  constructor(private gameSvc:GameService) { }

  numbOfRows : number = 7;
  numbOfCols : number = 6;

  @Input() userMode : boolean;

  ngOnInit() {
  }

  makeAction(index:number){
      if(this.userMode){
        this.placeShip(index);
      }
      else{
        
      }
  }

  fire(index: number) {
    this.gameSvc.fire(this.gameId, index).subscribe(res => {
      debugger;
      console.log(`Successfully fired to ${this.gameId} at ${res}`);
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

}


function replaceAt(s, n, t) {
  return s.substring(0, n) + t + s.substring(n + 1);
}