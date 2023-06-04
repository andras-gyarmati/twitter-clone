import {Injectable} from '@angular/core';
import {HttpClient, HttpErrorResponse} from "@angular/common/http";
import {lastValueFrom} from "rxjs";
import {environment} from "../../environments/environment";
import {Tweet} from "../models/tweet";
import {CreateTweetRequest} from "../models/createTweetRequest";
import {UserService} from "./user.service";

@Injectable({
  providedIn: 'root'
})
export class TweetService {
  constructor(private httpClient: HttpClient, private userService: UserService) {
  }

  private getPath() {
    return 'tweets';
  }

  async get(): Promise<Tweet[]> {
    try {
      return await lastValueFrom(this.httpClient.get<Tweet[]>(`${environment.apiUrl}/${this.getPath()}/`, {}));
    } catch (e) {
      if (e instanceof HttpErrorResponse && e?.error?.status === 401) {
        console.log("invalid credentials");
        this.userService.remove();
      }
    }
    return [];
  }

  async post(tweet: CreateTweetRequest): Promise<any> {
    return await lastValueFrom(this.httpClient.post<Tweet>(`${environment.apiUrl}/${this.getPath()}/`, tweet));
  }
}
