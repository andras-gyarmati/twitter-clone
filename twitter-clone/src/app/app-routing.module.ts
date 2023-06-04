import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {LoginComponent} from "./components/login/login.component";
import {PageRoutes} from "./_constants/page-routes";
import {LobbyComponent} from "./components/lobby/lobby.component";
import {RegisterComponent} from "./components/register/register.component";
import {TweetListComponent} from "./components/tweet-list/tweet-list.component";
import {EditProfileComponent} from "./components/profile/edit-profile/edit-profile.component";
import {ProfileComponent} from "./components/profile/profile/profile.component";


const routes: Routes = [
  {path: PageRoutes.login, component: LoginComponent},
  {path: PageRoutes.register, component: RegisterComponent},
  {path: PageRoutes.lobby, component: LobbyComponent},
  {path: PageRoutes.tweets, component: TweetListComponent},
  {path: PageRoutes.profile, component: ProfileComponent},
  {path: PageRoutes.editProfile, component: EditProfileComponent},
  {path: '', redirectTo: `/${PageRoutes.login}`, pathMatch: 'prefix'},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
