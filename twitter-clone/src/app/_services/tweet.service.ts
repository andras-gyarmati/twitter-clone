import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {lastValueFrom} from "rxjs";

export class Tweet {
  createdAt!: Date;
  authorName!: string;
  authorProfilePicture!: string;
  content!: string;
}

export class CreateTweetRequest {
  authorName!: string; // todo remove and use logged in user
  content!: string;
}

@Injectable({
  providedIn: 'root'
})
export class TweetService {

  constructor(private httpClient: HttpClient) {
  }

  async get(): Promise<Tweet[] | undefined> {
    return await lastValueFrom(this.httpClient.get<Tweet[]>(`http://localhost:5017/${this.getPath()}/`, {}));
  }

  async post(tweet: CreateTweetRequest): Promise<any> {
    return await lastValueFrom(this.httpClient.post<Tweet>(`http://localhost:5017/${this.getPath()}/`, tweet));
  }

  private getPath() {
    return 'tweets'
  }
}
