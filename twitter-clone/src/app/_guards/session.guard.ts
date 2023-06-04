import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import {RouterService} from "src/app/_services/router.service";
import {UserService} from "src/app/_services/user.service";

@Injectable({
  providedIn: 'root'
})
export class SessionGuard implements CanActivate {
  constructor(private routerService: RouterService,
              private userService: UserService) {
  }
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    if (!this.userService.isLoggedIn) {
      this.routerService.routeToLogin();
    }
    return this.userService.isLoggedIn;
  }

}
