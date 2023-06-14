export class User {
  email: string = "";
  username: string = "";
  bio: string = "";
  profilePicture: string | undefined;
  following: string[] = [];
  followerCount: number = 0;
  birthDate: Date = new Date();
}
