import {NgModule} from '@angular/core';
import {RouterModule, Routes} from '@angular/router';
import {LoginComponent} from "./components/login/login.component";
import {PageRoutes} from "./_constants/page-routes";
import {RegisterComponent} from "./components/register/register.component";
import {EditProfileComponent} from "./components/profile/edit-profile/edit-profile.component";
import {ProfileComponent} from "./components/profile/profile/profile.component";
import {SessionGuard} from "./_guards/session.guard";
import {PublicGuard} from "./_guards/public.guard";
import {FeedComponent} from "./components/feed/feed.component";

const routes: Routes = [
  {path: PageRoutes.login, component: LoginComponent, canActivate: [PublicGuard]},
  {path: PageRoutes.register, component: RegisterComponent, canActivate: [PublicGuard]},
  {path: PageRoutes.tweets, component: FeedComponent},
  {path: PageRoutes.profile, component: ProfileComponent, canActivate: [SessionGuard]},
  {path: PageRoutes.editProfile, component: EditProfileComponent, canActivate: [SessionGuard]},
  {path: '', redirectTo: `/${PageRoutes.login}`, pathMatch: 'prefix'},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {
}
