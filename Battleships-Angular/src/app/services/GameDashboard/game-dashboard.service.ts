import { SignalRService } from './../SignalR/signal-r.service';
import { Injectable, OnInit } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class GameDashboardService implements OnInit {  
  
  constructor(private signalRSvc:SignalRService) { }

  ngOnInit(): void {
    debugger;
    // this.signalRSvc.gamesConnection.on("", (pos:number) => {console.log(`[Game Dashboard Svc] : hit ${pos};`)});
  }
}
