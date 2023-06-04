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
    this.data = await this.tweetService.get();
  }
}
