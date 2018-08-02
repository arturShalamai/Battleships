import { GameDashboardService } from './../services/GameDashboard/game-dashboard.service';
import { Component, OnInit } from '@angular/core';
import { HubConnectionBuilder } from '@aspnet/signalr';

@Component({
  selector: 'app-game-dashboard',
  templateUrl: './game-dashboard.component.html',
  styleUrls: ['./game-dashboard.component.css']
})
export class GameDashboardComponent implements OnInit {

  fieldOne: boolean[][] = [[true, true, false, true, false, true],
                           [true, true, false, true, false, true],
                           [true, true, false, true, false, true],
                           [true, true, false, true, false, true],
                           [true, true, false, true, false, true],
                           [true, true, false, true, false, true],
                           [true, true, false, true, false, true],
                          ];

  fieldTwo: boolean[][] =[[false, false, false, false, false, false],
                          [false, false, false, false, false, false],
                          [false, false, false, false, false, false],
                          [false, false, false, false, false, false],
                          [false, false, false, false, false, false],
                          [false, false, false, false, false, false],
                          [false, false, false, false, false, false],
                         ];

  constructor(private gameSvc:GameDashboardService) { 
  }

  fire(index:number){
    console.log(`index ${index}`)
  }

  ngOnInit() {
  }

}
