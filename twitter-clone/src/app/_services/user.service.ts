import {Injectable} from '@angular/core';
import {StorageKeys} from "../_constants/storage-keys";
import {delay, Observable, of} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor() {
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

  login(): Observable<{ token: string }> {
    return of({token: 'my_token'}).pipe(delay(1000))
  }
}
