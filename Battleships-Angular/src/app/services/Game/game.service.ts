import { CreateGameResponse } from './CreateGameResponse';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class GameService {
  constructor(private http:HttpClient) { }

  createGame(): Observable<CreateGameResponse>{
    var token = localStorage.getItem("access_token");
    return this.http.post<CreateGameResponse>('https://localhost:44310/api/Game/create', {},{headers:{'Authorization': `Bearer ${token}`}});
  }

  joinGame(gameId):Observable<any> {
    var token = localStorage.getItem("access_token");
    return this.http.post(`https://localhost:44310/api/Game/join/${gameId}`, {},{headers:{'Authorization': `Bearer ${token}`}});
  }

}
