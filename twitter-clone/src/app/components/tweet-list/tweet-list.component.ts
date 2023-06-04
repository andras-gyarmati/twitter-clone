import {Component, Input, OnInit} from '@angular/core';
import {NzMessageService} from 'ng-zorro-antd/message';
import {TweetService} from "../../_services/tweet.service";
import {UserService} from "../../_services/user.service";
import {Tweet} from "../../models/tweet";

@Component({
  selector: 'app-tweet-list',
  templateUrl: './tweet-list.component.html',
  styleUrls:['./tweet-list.component.css']
})
export class TweetListComponent implements OnInit {
  data: Tweet[] | undefined = [];
  @Input() userName: string = '';


  constructor(public msg: NzMessageService, private tweetService: TweetService, private userService: UserService) {
  }

  async ngOnInit(): Promise<void> {
    this.data = await this.tweetService.get();
  }

  canCreateNewPost() {
    return !!this.userService.get() && !this.userName;
  }
}
