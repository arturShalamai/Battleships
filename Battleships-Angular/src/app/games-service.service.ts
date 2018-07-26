import { Injectable } from '@angular/core';
import { HttpClient } from '../../node_modules/@angular/common/http';


@Injectable({
  providedIn: 'root'
})
export class GamesService {

  constructor(private client : HttpClient) {
      client.get('https://localhost:44310/api/values').subscribe(res => console.log(`[Games Service] : ${res}`));

   }
}
