import {Component, OnInit} from '@angular/core';
import {PageRoutes} from "../../../_constants/page-routes";
import {UserService} from "../../../_services/user.service";
import {TweetService} from "../../../_services/tweet.service";
import {Tweet} from "../../../models/tweet";

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  protected readonly PageRoutes = PageRoutes;
  public tweets: Tweet[] = [];

  constructor(public userService: UserService, private tweetService: TweetService) {
  }

  async ngOnInit() {
    const user = this.userService.getLoggedInUser();
    if (user) {
      this.tweets = await this.tweetService.getUsersTweets(user.username);
    }
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
