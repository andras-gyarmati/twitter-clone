import {Component} from '@angular/core';
import {PageRoutes} from "../../../_constants/page-routes";
import {UserService} from "../../../_services/user.service";

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent {
  protected readonly PageRoutes = PageRoutes;

  constructor(public userService: UserService) {
  }

  isFollowed: boolean = false;

  myProfile() {
    // TODO: ellenőrizni hogy én vagyok-e
    return true;
  }

  followed() {
    // TODO: ellenőrizni hogy követem-e
    this.isFollowed = true;
  }

  unFollowed() {
    // TODO: ellenőrizni hogy követem-e
    this.isFollowed = false;
  }
}
