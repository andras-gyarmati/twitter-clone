import {Injectable} from '@angular/core';
import {StorageKeys} from "../_constants/storage-keys";
import {lastValueFrom} from "rxjs";
import {HttpClient} from "@angular/common/http";
import {environment} from "../../environments/environment";
import {LoginRequest} from "../models/loginRequest";
import {PageRoutes} from "../_constants/page-routes";
import {Router} from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class UserService {
  constructor(private httpClient: HttpClient, private router: Router) {
  }

  private getPath() {
    return 'tweets';
  }

  get(): string | null {
    return localStorage.getItem(StorageKeys.session);
  }

  remove(): void {
    localStorage.removeItem(StorageKeys.session);
    this.router.navigateByUrl(`/${PageRoutes.login}`);
  }

  store(token: string): void {
    localStorage.setItem(StorageKeys.session, token);
  }

  async login(loginRequest: LoginRequest): Promise<any> {
    const token = await lastValueFrom(this.httpClient.post<any>(`${environment.apiUrl}/users/login`, loginRequest));
    this.store(token.message);
  }
}
