import { HttpClient } from "@angular/common/http";
import { OnInit } from "@angular/core";
import { Injectable } from "@angular/core";
import {
  HubConnection,
  HubConnectionBuilder
} from "../../../../node_modules/@aspnet/signalr";
import { delay } from "../../../../node_modules/rxjs/operators";
import { resolve } from "url";

@Injectable({
  providedIn: "root"
})
export class SignalRService {
  public gamesConnection: HubConnection | undefined;

  constructor(private client: HttpClient) {
    let hubConnBuilder = new HubConnectionBuilder();
    this.gamesConnection = hubConnBuilder
      .withUrl("https://localhost:44310/hubs/game", {
        accessTokenFactory: () => localStorage.getItem("access_token")
      })
      .build();

    // this.gamesConnection.

    this.gamesConnection
      .start()
      .then(() => {
        this.gamesConnection.on("onPlayerJoined", res => {
          console.log(Date.now().toLocaleString(), "Joined player : ", res);
        });

        console.log("Connection Success");
        setTimeout(resolve, 10000);
        // delay(5000);
      })
      .then(() => {
        this.client
          .get(
            "https://localhost:44310/api/values/84616006-81e5-4f1d-9506-00d2ceabc4e1",
            {
              headers: {
                Authorization: `Bearer ${localStorage.getItem("access_token")}`
              }
            }
          )
          .subscribe(res => console.log(`[Games Service] : ${res}`));
      })
      .catch(err => console.log("Hub Connection error", err));

    // this.gamesConnection.on("hited", args => console.log(`Hited ${args}`));
  }

  subscribe(name: string) {}
}
