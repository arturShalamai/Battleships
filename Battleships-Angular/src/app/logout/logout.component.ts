import { AuthService } from './../services/Auth/auth.service';
import { Component, OnInit, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.css']
})
export class LogoutComponent implements OnInit {

  @Output() loggedOut = new EventEmitter<any>();

  constructor(private authSvc:AuthService) { }

  ngOnInit() {
  }

logOut(){
  debugger;
  this.authSvc.logout();
  this.loggedOut.emit(null);
}

}
