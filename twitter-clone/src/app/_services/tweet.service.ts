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

  async get(from: Date): Promise<Tweet[]> {
    try {
      return await lastValueFrom(this.httpClient.get<Tweet[]>(`${environment.apiUrl}/${this.getPath()}/`, {
        headers: {
          Filtering: from.toJSON()
        }
      }));
    } catch (e) {
      if (e instanceof HttpErrorResponse && e?.error?.status === 401) {
        console.log("invalid credentials");
        this.userService.remove();
      }
    }
    return [];
  }

  async getById(id: number): Promise<Tweet | undefined> {
    try {
      return await lastValueFrom(this.httpClient.get<Tweet | undefined>(`${environment.apiUrl}/${this.getPath()}/${id}`, {}));
    } catch (e) {
      if (e instanceof HttpErrorResponse && e?.error?.status === 401) {
        console.log("invalid credentials");
        this.userService.remove();
      }
    }
    return undefined;
  }

  async getUsersTweets(username: string, from: Date): Promise<Tweet[]> {
    try {
      return await lastValueFrom(this.httpClient.get<Tweet[]>(`${environment.apiUrl}/users/${username}/tweets`, {
        headers: {
          Filtering: from.toJSON()
        }
      }));
    } catch (e) {
      if (e instanceof HttpErrorResponse && e?.error?.status === 401) {
        console.log("invalid credentials");
        this.userService.remove();
      }
    }
    return [];
  }

  async post(tweet: CreateTweetRequest): Promise<any> {
    return await lastValueFrom(this.httpClient.post<Tweet>(`${environment.apiUrl}/${this.getPath()}/`, tweet, {
      headers: {
        Authorization: `Bearer ${this.userService.getToken()}`
      }
    }));
  }

  async reply(replyToTweetId: number, tweet: CreateTweetRequest): Promise<any> {
    return await lastValueFrom(this.httpClient.post<Tweet>(`${environment.apiUrl}/${this.getPath()}/${replyToTweetId}/reply`, tweet, {
      headers: {
        Authorization: `Bearer ${this.userService.getToken()}`
      }
    }));
  }

  async like(id: number): Promise<any> {
    return await lastValueFrom(this.httpClient.post(`${environment.apiUrl}/${this.getPath()}/${id}/like`, {}, {
      headers: {
        Authorization: `Bearer ${this.userService.get()}`
      }
    }));
  }
}
