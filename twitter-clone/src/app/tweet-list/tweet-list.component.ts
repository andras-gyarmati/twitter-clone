import {Component, OnInit} from '@angular/core';

import {NzMessageService} from 'ng-zorro-antd/message';
import {Tweet, TweetService} from "../_services/tweet.service";

@Component({
  selector: 'app-tweet-list',
  templateUrl: './tweet-list.component.html'
})
export class TweetListComponent implements OnInit {
  data: Tweet[] | undefined = [];


  constructor(public msg: NzMessageService, private tweetService: TweetService) {
  }

  async ngOnInit(): Promise<void> {
    this.data = await this.tweetService.get();
  }
}
