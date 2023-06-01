import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-tweet',
  templateUrl: './tweet.component.html',
  styleUrls: ['./tweet.component.css']
})
export class TweetComponent {
  @Input() avatarUrl: string = '';
  @Input() username: string = '';
  @Input() postTime: string = '';
  @Input() content: string = '';
  @Input() likes: number = 0;
  @Input() retweets: number = 0;

  likeTweet() {
    this.likes++;
  }

  retweet() {
    this.retweets++;
  }
}

