import { AuthService } from './../services/Auth/auth.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.css']
})
export class LogoutComponent implements OnInit {

  constructor(private authSvc:AuthService) { }

  ngOnInit() {
    this.authSvc.logout();
  }

}
