import {Component} from '@angular/core';
import {PageRoutes} from "../_constants/page-routes";
import {RouterService} from "../_services/router.service"
import {UserService} from "../_services/user.service";

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.css']
})
export class LayoutComponent {
  protected readonly PageRoutes = PageRoutes;
  protected readonly RouterService = RouterService;

  constructor(public userService: UserService, private routerService: RouterService) {
  }

  logOut() {
    this.userService.remove();
    this.routerService.routeToLogin();
  }
}
