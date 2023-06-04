import { Component } from '@angular/core';
import {PageRoutes} from "../../../_constants/page-routes";

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html'
})
export class ProfileComponent {
  protected readonly PageRoutes = PageRoutes;
}
