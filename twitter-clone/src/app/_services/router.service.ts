import {Injectable} from '@angular/core';
import {Router} from "@angular/router";
import {PageRoutes} from "../_constants/page-routes";

@Injectable({
  providedIn: 'root'
})
export class RouterService {
  constructor(private router: Router) {
  }

  routeToTweets(): void {
    this.router.navigateByUrl(`/${PageRoutes.tweets}`);
  }

  routeToLogin(): void {
    this.router.navigateByUrl(`/${PageRoutes.login}`);
  }
}
