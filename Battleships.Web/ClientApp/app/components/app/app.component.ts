import { SignalRGameService } from "./../signalr-game-service/signalrGameService";
import { Component } from "@angular/core";

@Component({
  selector: "app",
  templateUrl: "./app.component.html",
  styleUrls: ["./app.component.css"]
})
export class AppComponent {
  constructor(private hubSvc: SignalRGameService) {}
  // private _hub : HubContext;
}
