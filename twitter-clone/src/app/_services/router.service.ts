import {Injectable} from '@angular/core';
import {Router} from "@angular/router";
import {PageRoutes} from "../_constants/page-routes";

@Injectable({
  providedIn: 'root'
})
export class RouterService {

  constructor(private router: Router) {
  }

  routeToLobby(): void {
    this.router.navigateByUrl(`/${PageRoutes.lobby}`);
  }
}
