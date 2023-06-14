export class Tweet {
  id: number = 0;
  createdAt: Date = new Date();
  authorName: string = '';
  authorProfilePicture: string | undefined;
  content: string = '';
  likeCount: number = 0;
  replyCount: number = 0;
}
