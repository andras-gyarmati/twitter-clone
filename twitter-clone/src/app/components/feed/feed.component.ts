import {Component, OnInit} from '@angular/core';
import {TweetService} from "../../_services/tweet.service";
import {Tweet} from "../../models/tweet";

@Component({
  selector: 'app-feed',
  templateUrl: './feed.component.html',
  styleUrls: ['./feed.component.less']
})
export class FeedComponent implements OnInit {
  public data!: Tweet[];

  constructor(public tweetService: TweetService) {
  }

  async ngOnInit() {
    this.data = await this.tweetService.get(new Date());
  }

  loadMoreTweets = async () => {
    console.log(this.data[this.data.length - 1].createdAt);
    const response = await this.tweetService.get(new Date(this.data[this.data.length - 1].createdAt));
    this.data = this.data.concat(response);
  }
}
