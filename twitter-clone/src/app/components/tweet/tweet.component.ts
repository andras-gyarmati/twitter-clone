import {Component, Input, TemplateRef} from '@angular/core';
import {TweetService} from "../../_services/tweet.service";
import {Tweet} from "../../models/tweet";
import {UserService} from "../../_services/user.service";
import {NzModalRef, NzModalService} from "ng-zorro-antd/modal";
import {PageRoutes} from "../../_constants/page-routes";

@Component({
  selector: 'app-tweet',
  templateUrl: './tweet.component.html',
  styleUrls: ['./tweet.component.css']
})
export class TweetComponent {
  @Input() data: Tweet = {} as Tweet;
  @Input() loadReplyCallback: (id: number) => Promise<void> = async () => {};
  @Input() deleteTweetCallback: (id: number) => Promise<void> = async () => {};
  public textValue: string = '';

  constructor(private tweetService: TweetService,
              public userService: UserService,
              private modalService: NzModalService) {
  }

  async likeTweet() {
    try {
      const response = await this.tweetService.like(this.data.id);
      console.log(response);
      this.data.likeCount++;
    } catch (e) {
      console.log(e);
    }
  }

  async reply(tplContent: TemplateRef<any>) {
    const modalRef: NzModalRef = this.modalService.create({
      nzTitle: 'Make a reply',
      nzContent: tplContent,
      nzOnOk: async () => {
        console.log('OK button was clicked', this.textValue);
        try {
          const response = await this.tweetService.reply(this.data.id, {content: this.textValue});
          console.log(response);
          if (response) {
            await this.loadReplyCallback(response);
          }
          this.data.replyCount++;
        } catch (e) {
          console.log(e);
        }
        this.textValue = '';
      },
      nzOnCancel: () => {
        modalRef.close();
      }
    });
  }

  protected readonly PageRoutes = PageRoutes;

  async deleteTweet() {
    if (this.data.authorName === this.userService.getLoggedInUser()?.username){
      try {
        await this.tweetService.deleteTweet(this.data.id);
        await this.deleteTweetCallback(this.data.id);
      } catch (e) {
        console.log(e);
      }
    }
  }
}
