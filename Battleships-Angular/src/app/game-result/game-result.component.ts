import { Component, OnInit, Input } from '@angular/core';
import { Router } from '../../../node_modules/@angular/router';

@Component({
  selector: 'app-game-result',
  templateUrl: './game-result.component.html',
  styleUrls: ['./game-result.component.css']
})
export class GameResultComponent implements OnInit {

  @Input() public victory = false;
  public backgroundColor = this.victory ? '#64dd17' : '#f44336' ;

  constructor(private router:Router) { }

  ngOnInit() {
  }

  returnToMenu(){
    this.router.navigate(['/game']);
  }

  setState(state: string){
    this.victory = state == 'win' ? true : false;
  }

}
