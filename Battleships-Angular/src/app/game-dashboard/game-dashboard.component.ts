import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-game-dashboard',
  templateUrl: './game-dashboard.component.html',
  styleUrls: ['./game-dashboard.component.css']
})
export class GameDashboardComponent implements OnInit {

  fieldOne: boolean[][] = [[true, false, false, false, true, null],
                           [true, false, false, false, true, null],
                           [true, false, false, false, true, null],
                           [true, false, false, false, true, null],
                           [true, false, false, false, true, null],
                           [true, false, false, false, true, null],
                           [true, false, false, false, true, null],
                          ];
                          
  constructor() { }

  ngOnInit() {
  }

}
