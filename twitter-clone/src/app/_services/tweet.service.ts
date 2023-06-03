import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {lastValueFrom} from "rxjs";

export class Tweet {
  createdAt!: Date;
  authorName!: number;
  authorProfilePicture!: string;
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

  private getPath() {
    return 'tweets'
  }
}
