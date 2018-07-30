import { GameDashboardService } from './../services/GameDashboard/game-dashboard.service';
import { Component, OnInit } from '@angular/core';
import { HubConnectionBuilder } from '@aspnet/signalr';

@Component({
  selector: 'app-game-dashboard',
  templateUrl: './game-dashboard.component.html',
  styleUrls: ['./game-dashboard.component.css']
})
export class GameDashboardComponent implements OnInit {

  fieldOne: boolean[][] = [[null, null, null, null, null, null],
                           [null, null, null, null, null, null],
                           [null, null, null, null, null, null],
                           [null, null, null, null, null, null],
                           [null, null, null, null, null, null],
                           [null, null, null, null, null, null],
                           [null, null, null, null, null, null],
                          ];
                          
  constructor(private gameSvc:GameDashboardService) { 
  }

  ngOnInit() {
  }

}
