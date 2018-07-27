import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-start-game',
  templateUrl: './start-game.component.html',
  styleUrls: ['./start-game.component.css']
})
export class StartGameComponent implements OnInit {

  private images: string[] = ['/assets/images/home/bg1.jpg', '/assets/images/home/bg2.png', '/assets/images/home/bg3.png'];
  public bgImage: string = this.images[0];

  constructor() { }

  ngOnInit() {
    debugger;
    // setInterval(() => {
    //   var randomImage = this.getRandomImage();
    //   console.log(randomImage);
    //   this.bgImage = randomImage;
    // }, 3000);
  }

  private getRandomImage(): string{
    return this.images[Math.floor(Math.random()*this.images.length)];
  }

}
