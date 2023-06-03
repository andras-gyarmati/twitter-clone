import {Component} from '@angular/core';
import {PageRoutes} from "../_constants/page-routes";

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: ['./layout.component.css']
})
export class LayoutComponent {
  protected readonly PageRoutes = PageRoutes;
}
