import {Component, Input} from '@angular/core';
import {TweetService} from "../../_services/tweet.service";

@Component({
  selector: 'app-new-tweet',
  templateUrl: './new-tweet.component.html',
  styleUrls: ['./new-tweet.component.css']
})
export class NewTweetComponent {
  @Input() avatarUrl: string = '';
  @Input() username: string = '';
  submitting = false;
  inputValue = '';

  constructor(private tweetService: TweetService) {
  }

  async handleSubmit() {
    this.submitting = true;
    const content = this.inputValue;
    this.inputValue = '';
    await this.tweetService.post({authorName: this.username, content: content});
    this.submitting = false;
  }
}
