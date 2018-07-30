import { OnInit } from "@angular/core";
import { Injectable } from "@angular/core";
import {
  HubConnection,
  HubConnectionBuilder
} from "../../../../node_modules/@aspnet/signalr";

@Injectable({
  providedIn: "root"
})
export class SignalRService {
  public gamesConnection: HubConnection | undefined;

  constructor() {
    debugger;
    let hubConnBuilder = new HubConnectionBuilder();
    this.gamesConnection = hubConnBuilder
      .withUrl("https://localhost:44310/hub/games")
      .build();

      this.gamesConnection.start()
      .then(x => console.log("Connection Success"))
      .catch(err => console.log("Hub Connection error", err));
  }
}
