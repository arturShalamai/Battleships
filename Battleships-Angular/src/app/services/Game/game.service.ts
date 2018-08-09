import { GameInfoModel } from "./../../Models/GameInfoModel";
import { ShotResult } from "./ShotResult";
import { ShipsFieldModel } from "./../../Models/ShipsFieldModel";
import { SignalRService } from "./../SignalR/signal-r.service";
import { CreateGameResponse } from "./CreateGameResponse";
import { HttpClient } from "@angular/common/http";
import { Observable, Subscribable } from "rxjs";
import { Injectable } from "@angular/core";
import { HubConnection } from "../../../../node_modules/@aspnet/signalr";

@Injectable({
  providedIn: "root"
})
export class GameService {
  public gamesConnection: HubConnection | undefined;

  constructor(private http: HttpClient, private signalRSvc: SignalRService) {
    this.gamesConnection = signalRSvc.gamesConnection;
  }

  closeConn() {
    this.signalRSvc.gamesConnection.stop();
  }

  getGameInfo(gameId: string): Observable<GameInfoModel> {
    let token = localStorage.getItem("access_token");
    return this.http.get<GameInfoModel>(
      `https://localhost:44310/api/Game/${gameId}`,
      {
        headers: { Authorization: `Bearer ${token}` }
      }
    );
  }

  createGame(): Observable<string> {
    var token = localStorage.getItem("access_token");
    return this.http.post<string>(
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

  checkParticipation(gameId: string): Observable<boolean> {
    let token = localStorage.getItem("access_token");
    return this.http.get<boolean>(
      `https://localhost:44310/api/Game/${gameId}/checkParticipant`,
      {
        headers: { Authorization: `Bearer ${token}` }
      }
    );
  }

  surrender(gameId: string): Observable<any> {
    let token = localStorage.getItem("access_token");
    return this.http.post<any>(
      `https://localhost:44310/api/Game/${gameId}/surrender`,
      {},
      {
        headers: { Authorization: `Bearer ${token}` }
      }
    );
  }
}
