import {Injectable} from '@angular/core';
import {StorageKeys} from "../_constants/storage-keys";
import {delay, lastValueFrom, Observable, of} from "rxjs";
import {Tweet} from "./tweet.service";
import {HttpClient} from "@angular/common/http";

export class LoginRequest {
  userName!: string;
  password!: string;
}

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private httpClient: HttpClient) {
  }

  get(): string | null {
    return localStorage.getItem(StorageKeys.session);
  }

  remove(): void {
    localStorage.removeItem(StorageKeys.session);
  }

  store(token: string): void {
    localStorage.setItem(StorageKeys.session, token);
  }

  async login(loginRequest: LoginRequest): Promise<any> {
    // return of({token: 'my_token'}).pipe(delay(1000))
    const token = await lastValueFrom(this.httpClient.post<any>(`http://localhost:5017/users/login`, loginRequest));
    console.log(token.message);
    this.store(token.message);
  }
}
