import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SignalRService {

  gameConnection : HubConnection | undefined;

  constructor() { }
}
