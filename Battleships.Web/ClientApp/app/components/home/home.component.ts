import { SignalRGameService } from './../signalr-game-service/signalrGameService';
import { Component } from '@angular/core';

@Component({
    selector: 'home',
    templateUrl: './home.component.html'
})
export class HomeComponent {
    constructor(private hubSvc: SignalRGameService){
        debugger;
    }
}
