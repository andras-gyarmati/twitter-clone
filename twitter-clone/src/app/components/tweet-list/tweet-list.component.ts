import {Component, Input} from '@angular/core';
import {UserService} from "../../_services/user.service";
import {Tweet} from "../../models/tweet";
import {TweetService} from "../../_services/tweet.service";

@Component({
  selector: 'app-tweet-list',
  templateUrl: './tweet-list.component.html',
  styleUrls: ['./tweet-list.component.css']
})
export class TweetListComponent {
  @Input() data: Tweet[] = [];
  @Input() loadMoreTweetsCallback: () => Promise<void> = async () => {
  };

  constructor(public userService: UserService, public tweetService: TweetService) {
  }

  canCreateNewPost() {
    return this.userService.isLoggedIn;
  }

  async loadMoreTweets() {
    await this.loadMoreTweetsCallback();
  }

  loadReply = async (id: number) => {
    try {
      const response = await this.tweetService.getById(id);
      if (response) {
        this.data = [response].concat(this.data);
      }
    } catch (e) {
      console.log(e);
    }
  };

  deleteTweet = async (id: number) => {
    this.data = this.data.filter(e => e.id !== id);
  }
}
