import {Component, OnInit} from '@angular/core';
import {PageRoutes} from "../../../_constants/page-routes";
import {UserService} from "../../../_services/user.service";
import {TweetService} from "../../../_services/tweet.service";
import {Tweet} from "../../../models/tweet";
import {User} from "../../../models/user";

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  protected readonly PageRoutes = PageRoutes;
  public isFollowed: boolean = false;
  public tweets: Tweet[] = [];
  private user!: User;

  constructor(public userService: UserService, private tweetService: TweetService) {
  }

  async ngOnInit() {
    const user = this.userService.getLoggedInUser();
    if (user) {
      this.user = user;
      this.tweets = await this.tweetService.getUsersTweets(user.username, new Date());
    }
  }

  loadMoreTweets = async () => {
    console.log(this.tweets[this.tweets.length - 1].createdAt);
    const response = await this.tweetService.getUsersTweets(this.user.username, new Date(this.tweets[this.tweets.length - 1].createdAt));
    this.tweets = this.tweets.concat(response);
  };

  isMyProfile() {
    // TODO: ellenőrizni hogy én vagyok-e
    return true;
  }

  unfollowClicked() {
    // TODO: ellenőrizni hogy követem-e
    this.isFollowed = true;
  }

  followClicked() {
    // TODO: ellenőrizni hogy követem-e
    this.isFollowed = false;
  }
}
