import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {delay, Observable, of} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class ClientService {

  constructor(private httpClient: HttpClient) {
  }

  login(): Observable<{ token: string }> {
    return of({token: 'my_token'}).pipe(delay(1000))
  }
}
