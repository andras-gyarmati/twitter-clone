import {Injectable} from '@angular/core';
import {StorageKeys} from "../_constants/storage-keys";
import {lastValueFrom} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../environments/environment";
import {LoginRequest} from "../models/loginRequest";
import {User} from "../models/user";
import {RouterService} from "./router.service";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  public isLoggedIn: boolean = false;

  constructor(private httpClient: HttpClient, private routerService: RouterService) {
    this.isLoggedIn = this.getToken() !== null;
  }

  private getPath() {
    return 'users';
  }

  getToken(): string | null {
    return localStorage.getItem(StorageKeys.token);
  }

  getLoggedInUser(): User | undefined {
    return localStorage.getItem(StorageKeys.loggedInUser) ? JSON.parse(localStorage.getItem(StorageKeys.loggedInUser) as string) : undefined;
  }

  remove(): void {
    localStorage.removeItem(StorageKeys.token);
    localStorage.removeItem(StorageKeys.loggedInUser);
    this.routerService.routeToLogin();
    this.isLoggedIn = false;
  }

  store(token: string): void {
    localStorage.setItem(StorageKeys.token, token);
    this.isLoggedIn = true;
  }

  async login(loginRequest: LoginRequest): Promise<any> {
    const token = await lastValueFrom(this.httpClient.post<any>(`${environment.apiUrl}/${this.getPath()}/login`, loginRequest));
    this.store(token.token);
    const loggedInUser: User = await lastValueFrom(this.httpClient.get<User>(`${environment.apiUrl}/${this.getPath()}/${loginRequest.username}`, {
      headers: {
        Authorization: `Bearer ${this.getToken()}`
      }
    }));
    localStorage.setItem(StorageKeys.loggedInUser, JSON.stringify(loggedInUser));
  }
}
