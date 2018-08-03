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

  submitShips(fieldModel: ShipsFieldModel) {
    var token = localStorage.getItem("access_token");
    return this.http.post(
      `https://localhost:44310/api/Game/placeships`,
      { fieldModel },
      { headers: { Authorization: `Bearer ${token}` } }
    );
  }

  fire(gameId: string, index: number): Observable<any> {
    debugger;
    var token = localStorage.getItem("access_token");
    return this.http.post<string>(
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
