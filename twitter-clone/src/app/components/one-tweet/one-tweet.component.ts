import {Component, Input} from '@angular/core';
import {UserService} from "../../_services/user.service";
import {TweetService} from "../../_services/tweet.service";
import {Tweet} from "../../models/tweet";
import {ActivatedRoute} from "@angular/router";

@Component({
  selector: 'app-one-tweet',
  templateUrl: './one-tweet.component.html',
  styleUrls: ['./one-tweet.component.less']
})
export class OneTweetComponent {
  public mainTweet!: Tweet;
  public mainTweetId!: number;
  public parentTweets: Tweet[] = [];
  public replies: Tweet[] = [];

  constructor(public userService: UserService, public tweetService: TweetService, private route: ActivatedRoute) {
    this.route.params.subscribe(params => {
      this.mainTweetId = params['id'];
    });
  }

  async ngOnInit() {
    const response: any = await this.tweetService.getById(this.mainTweetId);
    console.log(response)
    if (response){
      this.mainTweet = response;
      this.parentTweets = response.parentTweets
      this.replies = response.replies
    }
  }

  loadReply = async (id: number) => {
    try {
      const response = await this.tweetService.getById(id);
      if (response) {
        // this.data = [response].concat(this.data);
      }
    } catch (e) {
      console.log(e);
    }
  };

  deleteTweet = async (id: number) => {
    // this.data = this.data.filter(e => e.id !== id);
  }
}
