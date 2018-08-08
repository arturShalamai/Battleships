import { GameInfoModel } from "./../../Models/GameInfoModel";
import { ShotResult } from "./ShotResult";
import { ShipsFieldModel } from "./../../Models/ShipsFieldModel";
import { SignalRService } from "./../SignalR/signal-r.service";
import { CreateGameResponse } from "./CreateGameResponse";
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { Injectable } from "@angular/core";

@Injectable({
  providedIn: "root"
})
export class GameService {
  constructor(private http: HttpClient, private signalRSvc: SignalRService) {
    // signalRSvc.gamesConnection.on("getGame", res => {
    //   // debugger;
    //   // console.log("Got game with id ", 25);
    // });

    // signalRSvc.gamesConnection.on('onGameCrated', msg => {
    //   console.log(Date.now().toLocaleString(), msg);
    // });

    signalRSvc.gamesConnection.on('onHit', res => {
      console.log("Hited : ", res);
    });

    signalRSvc.gamesConnection.on('onGameEnd', res => {
      console.log("Game End", res);
    });
    
    signalRSvc.gamesConnection.on("onPlayerJoined", res => {
      console.log(`Joined player : ${res.Id} ${res.FirstName}`);
    });

    signalRSvc.gamesConnection.on("onPlayerReady", res => {
      console.log(Date.now().toLocaleString(), "Second player ready.");
    });
    

    // signalRSvc.gamesConnection.invoke('GetConnId').then(res => {console.log('Connection Id ', res)})
  }

  closeConn(){
    this.signalRSvc.gamesConnection.stop();
  }

  // getGameInfo(id: string): Observable<GameInfoModel> {
  //   var token = localStorage.getItem("access_token");
  //   return this.http.get<GameInfoModel>(
  //     `https://localhost:44310/api/Game/${id}`,
  //     { headers: { Authorization: `Bearer ${token}` } }
  //   );
  // }

  createGame(): Observable<CreateGameResponse> {
    var token = localStorage.getItem("access_token");
    return this.http.post<CreateGameResponse>(
      "https://localhost:44310/api/Game/create",
      {},
      { headers: { Authorization: `Bearer ${token}` } }
    );
  }

  joinGame(gameId): Observable<any> {
    var token = localStorage.getItem("access_token");
    return this.http.post(
      `https://localhost:44310/api/Game/join/${gameId}`,
      {},
      { headers: { Authorization: `Bearer ${token}` } }
    );
  }

  submitShips(fieldModel: ShipsFieldModel): Observable<any> {
    var token = localStorage.getItem("access_token");
    return this.http.post(
      `https://localhost:44310/api/Game/placeships`,
      fieldModel,
      {
        headers: {
          Authorization: `Bearer ${token}`,
          ContentType: "application/json"
        }
      }
    );
  }

  fire(gameId: string, index: number): Observable<ShotResult> {
    debugger;
    var token = localStorage.getItem("access_token");
    return this.http.post<ShotResult>(
      `https://localhost:44310/api/Game/${gameId}/fire/${index}`,
      {},
      { headers: { Authorization: `Bearer ${token}` } }
    );
  }

  checkParticipation(gameId: string): Promise<boolean> {
    let token = localStorage.getItem("access_token");
    return this.http
      .get<boolean>(
        `https://localhost:44310/api/Game/${gameId}/checkParticipant`,
        {
          headers: { Authorization: `Bearer ${token}` }
        }
      )
      .toPromise();
  }
}
