import {Component, OnInit} from '@angular/core';
import {PageRoutes} from "../../../_constants/page-routes";
import {UserService} from "../../../_services/user.service";
import {TweetService} from "../../../_services/tweet.service";
import {Tweet} from "../../../models/tweet";
import {User} from "../../../models/user";
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit {
  protected readonly PageRoutes = PageRoutes;
  public isFollowed: boolean | undefined = false;
  public tweets: Tweet[] = [];
  public user!: User;
  private username: string = "";

  constructor(public userService: UserService, private tweetService: TweetService, private route: ActivatedRoute,) {
    this.route.params.subscribe(params => {
      this.username = params['id'];
    });
  }

  async ngOnInit() {
    // const user = this.userService.getLoggedInUser();
    const user = await this.userService.getUser(this.username);
    if (user) {
      this.user = user;
      this.tweets = await this.tweetService.getUsersTweets(user.username, new Date());
      if (this.isMyProfile()) {
        this.userService.storeUserData(user);
      }
    }
    this.isFollowed = this.userService.getLoggedInUser()?.following?.some(e => e === this.username);
    console.log(this.userService.getLoggedInUser())
  }

  loadMoreTweets = async () => {
    console.log(this.tweets[this.tweets.length - 1].createdAt);
    const response = await this.tweetService.getUsersTweets(this.user.username, new Date(this.tweets[this.tweets.length - 1].createdAt));
    this.tweets = this.tweets.concat(response);
  };

  isMyProfile() {
    return this.user?.username === this.userService.getLoggedInUser()?.username;
  }

  async unfollowClicked() {
    try {
      const response = await this.userService.unfollow(this.username);
      this.isFollowed = false;
      let loggedInUser = this.userService.getLoggedInUser();
      if (loggedInUser) {
        loggedInUser.following = loggedInUser.following.filter(e => e !== this.username);
        this.userService.storeUserData(loggedInUser);
        this.user.followerCount--;
      }
    } catch (e) {
      console.log(e);
    }
  }

  async followClicked() {
    try {
      const response = await this.userService.follow(this.username);
      this.isFollowed = true;
      let loggedInUser = this.userService.getLoggedInUser();
      if (loggedInUser) {
        loggedInUser.following = loggedInUser.following.concat(this.username);
        this.userService.storeUserData(loggedInUser);
        this.user.followerCount++;
      }
    } catch (e) {
      console.log(e);
    }
  }
}
