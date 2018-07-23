import { OnInit } from "@angular/core";
import { Injectable } from "@angular/core";
import { HubConnection } from "@aspnet/signalr";
import * as signalR from "@aspnet/signalr";

@Injectable()
export class SignalRGameService implements OnInit {
  hubConnection: HubConnection | undefined;
  nick = "";
  message = "";
  messages: string[] = [];

  constructor() {
    debugger;
    var cb = new signalR.HubConnectionBuilder();
    this.hubConnection = cb
      .withUrl("https://localhost:44310/hubs/game")
      .build();
    this.hubConnection
      .start()
      .then(x => console.log("[Game Hub] : conn Started successfull"))
      .catch(err => console.log(`[Game Hub] : SignalR error ${err}`));

      this.hubConnection.on("StartGame", (id:string) => console.log(`Recieved id : ${id}`));
  }

  ngOnInit(): void {}
}
