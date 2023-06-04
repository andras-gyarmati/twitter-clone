import {Injectable} from '@angular/core';
import {StorageKeys} from "../_constants/storage-keys";
import {lastValueFrom} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../environments/environment";
import {LoginRequest} from "../models/loginRequest";
import {PageRoutes} from "../_constants/page-routes";
import {Router} from "@angular/router";
import {User} from "../models/user";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  public isLoggedIn: boolean = false;
  constructor(private httpClient: HttpClient, private router: Router) {
    this.isLoggedIn = this.get() !== null;
  }

  private getPath() {
    return 'users';
  }

  get(): string | null {
    return localStorage.getItem(StorageKeys.session);
  }

  getLoggedInUser(): User | undefined {
    return localStorage.getItem(StorageKeys.loggedInUser) ? JSON.parse(localStorage.getItem(StorageKeys.loggedInUser) as string) : undefined;
  }

  remove(): void {
    localStorage.removeItem(StorageKeys.session);
    localStorage.removeItem(StorageKeys.loggedInUser);
    this.router.navigateByUrl(`/${PageRoutes.login}`);
    this.isLoggedIn = false;
  }

  store(token: string): void {
    localStorage.setItem(StorageKeys.session, token);
    this.isLoggedIn = true;
  }

  async login(loginRequest: LoginRequest): Promise<any> {
    const token = await lastValueFrom(this.httpClient.post<any>(`${environment.apiUrl}/${this.getPath()}/login`, loginRequest));
    this.store(token.token);
    const loggedInUser = await lastValueFrom(this.httpClient.get<User>(`${environment.apiUrl}/${this.getPath()}/${loginRequest.username}`));
    localStorage.setItem(StorageKeys.loggedInUser, JSON.stringify(loggedInUser));
  }
}
