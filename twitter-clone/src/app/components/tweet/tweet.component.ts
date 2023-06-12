import {Component, Input} from '@angular/core';
import {TweetService} from "../../_services/tweet.service";
import {Tweet} from "../../models/tweet";
import {UserService} from "../../_services/user.service";
import {PageRoutes} from "../../_constants/page-routes";

@Component({
  selector: 'app-tweet',
  templateUrl: './tweet.component.html',
  styleUrls: ['./tweet.component.css']
})
export class TweetComponent {
  @Input() data: Tweet = {} as Tweet;

  constructor(private tweetService: TweetService, private userService: UserService) {
  }

  likeTweet() {
    this.data.likeCount++;
  }

  async reply() {
    // todo open modal
    await this.tweetService.reply(this.data.id, {content: "test"});
  }

  protected readonly PageRoutes = PageRoutes;
}
