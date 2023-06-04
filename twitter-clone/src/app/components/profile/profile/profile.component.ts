import { Component } from '@angular/core';
import {PageRoutes} from "../../../_constants/page-routes";

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html'
})
export class ProfileComponent {
  protected readonly PageRoutes = PageRoutes;

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
