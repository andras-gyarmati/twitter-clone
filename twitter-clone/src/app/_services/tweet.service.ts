import {Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {lastValueFrom} from "rxjs";

export class Tweet {
  id!: number;
  createdAt!: Date;
  authorId!: number;
  content!: string;
  isDeleted!: boolean;
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
