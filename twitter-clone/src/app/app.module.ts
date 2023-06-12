import {NgModule} from '@angular/core';
import {BrowserModule} from '@angular/platform-browser';
import {AppRoutingModule} from './app-routing.module';
import {AppComponent} from './app.component';
import {LoginComponent} from './components/login/login.component';
import {RegisterComponent} from './components/register/register.component';
import {LobbyComponent} from './components/lobby/lobby.component';
import {en_US, NZ_I18N} from 'ng-zorro-antd/i18n';
import {registerLocaleData} from '@angular/common';
import en from '@angular/common/locales/en';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {HttpClientModule} from '@angular/common/http';
import {BrowserAnimationsModule} from '@angular/platform-browser/animations';
import {NzFormModule} from "ng-zorro-antd/form";
import {NzInputModule} from "ng-zorro-antd/input";
import {NzButtonModule} from "ng-zorro-antd/button";
import {NzCheckboxModule} from "ng-zorro-antd/checkbox";
import {NzSelectModule} from "ng-zorro-antd/select";
import {LayoutComponent} from './layout/layout.component';
import {NzLayoutModule} from "ng-zorro-antd/layout";
import {NzBreadCrumbModule} from "ng-zorro-antd/breadcrumb";
import {NzMenuModule} from "ng-zorro-antd/menu";
import {NzTypographyModule} from "ng-zorro-antd/typography";
import {TweetComponent} from './components/tweet/tweet.component';
import {NzCommentModule} from "ng-zorro-antd/comment";
import {NzAvatarModule} from "ng-zorro-antd/avatar";
import {NzToolTipModule} from "ng-zorro-antd/tooltip";
import {NzIconModule} from "ng-zorro-antd/icon";
import {TweetListComponent} from './components/tweet-list/tweet-list.component';
import {NzListModule} from "ng-zorro-antd/list";
import {NzMessageModule} from 'ng-zorro-antd/message';
import {NzDividerModule} from "ng-zorro-antd/divider";
import {NzCardModule} from "ng-zorro-antd/card";
import {EditProfileComponent} from './components/profile/edit-profile/edit-profile.component';
import {NzDatePickerModule} from "ng-zorro-antd/date-picker";
import {NzTagModule} from "ng-zorro-antd/tag";
import {NewTweetComponent} from './components/new-tweet/new-tweet.component';
import {NzResizableModule} from "ng-zorro-antd/resizable";
import {ProfileComponent} from './components/profile/profile/profile.component';
import {NzDescriptionsModule} from "ng-zorro-antd/descriptions";
import {NzUploadModule} from "ng-zorro-antd/upload";
import {FeedComponent} from './components/feed/feed.component';
import {NzModalModule} from "ng-zorro-antd/modal";

registerLocaleData(en);

@NgModule({
  declarations: [
    AppComponent,
    LoginComponent,
    RegisterComponent,
    LobbyComponent,
    LayoutComponent,
    TweetComponent,
    TweetListComponent,
    EditProfileComponent,
    NewTweetComponent,
    ProfileComponent,
    FeedComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    BrowserAnimationsModule,
    NzFormModule,
    NzInputModule,
    ReactiveFormsModule,
    NzButtonModule,
    NzCheckboxModule,
    NzSelectModule,
    NzLayoutModule,
    NzBreadCrumbModule,
    NzMenuModule,
    NzTypographyModule,
    NzCommentModule,
    NzAvatarModule,
    NzToolTipModule,
    NzIconModule,
    NzListModule,
    NzMessageModule,
    NzDividerModule,
    NzCardModule,
    NzDatePickerModule,
    NzTagModule,
    NzResizableModule,
    NzDescriptionsModule,
    NzUploadModule,
    NzModalModule
  ],
  providers: [
    {provide: NZ_I18N, useValue: en_US}
  ],
  bootstrap: [AppComponent]
})
export class AppModule {
}
