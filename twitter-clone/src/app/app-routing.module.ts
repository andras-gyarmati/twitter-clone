import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {LoginComponent} from "./login/login.component";
import {PageRoutes} from "./_constants/page-routes";
import {LobbyComponent} from "./lobby/lobby.component";
import {RegistrationComponent} from "./registration/registration.component";

const routes: Routes = [
  {path: '', redirectTo: `/${PageRoutes.login}`, pathMatch: 'prefix'},
  {path: PageRoutes.login, component: LoginComponent},
  {path: PageRoutes.registration, component: RegistrationComponent},
  {path: PageRoutes.lobby, component: LobbyComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
