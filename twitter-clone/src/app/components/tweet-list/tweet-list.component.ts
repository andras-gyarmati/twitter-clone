import {Component, Input} from '@angular/core';
import {NzMessageService} from 'ng-zorro-antd/message';
import {UserService} from "../../_services/user.service";
import {Tweet} from "../../models/tweet";

@Component({
  selector: 'app-tweet-list',
  templateUrl: './tweet-list.component.html',
  styleUrls: ['./tweet-list.component.css']
})
export class TweetListComponent {
  @Input() data: Tweet[] | undefined = [];
  @Input() loadMoreTweetsCallback: () => Promise<void> = async () => { };

  constructor(public msg: NzMessageService, public userService: UserService) {
  }

  canCreateNewPost() {
    return this.userService.isLoggedIn;
  }

  async loadMoreTweets() {
    await this.loadMoreTweetsCallback();
  }
}
