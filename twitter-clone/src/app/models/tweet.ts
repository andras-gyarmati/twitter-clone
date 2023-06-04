export class Tweet {
  id!: number;
  createdAt!: Date;
  authorName!: string;
  authorProfilePicture!: string;
  content!: string;
  likeCount: number = 0;
  replyCount: number = 0;
}
