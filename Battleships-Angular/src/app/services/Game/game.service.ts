import { GameInfoModel } from './../../Models/GameInfoModel';
import { ShotResult } from './ShotResult';
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
    signalRSvc.gamesConnection.on("oponentReady", res =>
      console.log("Oponent Ready")
    );
  }

  getGameInfo(id:string): Observable<GameInfoModel>{
    var token = localStorage.getItem("access_token");
    return this.http.get<GameInfoModel>(
      `https://localhost:44310/api/Game/${id}`,
      { headers: { Authorization: `Bearer ${token}` } }
    );
  }

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
      { headers: { Authorization: `Bearer ${token}`, ContentType:'application/json' } }
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
    return this.http.get<boolean>(
      `https://localhost:44310/api/Game/${gameId}/checkParticipant`,
      {
        headers: { Authorization: `Bearer ${token}` }
      }
    ).toPromise();
  }
}
