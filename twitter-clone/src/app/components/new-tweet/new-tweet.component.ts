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
  @Input() loadTweetCallback: (id: number) => Promise<void> = async () => {};
  submitting = false;
  inputValue = '';

  constructor(private tweetService: TweetService) {
  }

  async handleSubmit() {
    this.submitting = true;
    const content = this.inputValue;
    this.inputValue = '';
    try {
      const response = await this.tweetService.post({content: content});
      console.log(response);
      if (response) {
        await this.loadTweetCallback(response);
      }
    } catch (e) {
      console.log(e);
    }
    this.submitting = false;
  }
}
