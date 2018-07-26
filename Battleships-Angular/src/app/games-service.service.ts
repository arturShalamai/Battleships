import { OAuthService } from 'angular-oauth2-oidc';
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '../../node_modules/@angular/common/http';


@Injectable({
  providedIn: 'root'
})
export class GamesService {

  constructor(private client : HttpClient, private oauthSvc : OAuthService) {
    debugger;
    let headers = new HttpHeaders().set("Authorization", `Bearer ${oauthSvc.getAccessToken()}`);
    console.log(oauthSvc.getAccessToken());
    // headers.set("TestHeader", `Hello ${oauthSvc.getIdToken()}`);
    client.get('https://localhost:44310/api/values', { headers: {"Authorization": `Bearer ${oauthSvc.getAccessToken()}`}}).subscribe(res => console.log(`[Games Service] : ${res}`));
   }

getValues() : void{
}

}
