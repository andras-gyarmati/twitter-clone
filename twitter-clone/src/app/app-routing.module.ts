import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {LoginComponent} from "./login/login.component";
import {PageRoutes} from "./_constants/page-routes";
import {LobbyComponent} from "./lobby/lobby.component";
import {RegisterComponent} from "./register/register.component";
import {TweetListComponent} from "./tweet-list/tweet-list.component";


const routes: Routes = [
  {path: PageRoutes.login, component: LoginComponent},
  {path: PageRoutes.register, component: RegisterComponent},
  {path: PageRoutes.lobby, component: LobbyComponent},
  {path: PageRoutes.tweets, component: TweetListComponent},
  {path: '', redirectTo: `/${PageRoutes.login}`, pathMatch: 'prefix'},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
