namespace TwitterClone.Responses;

public class TweetResponse
{
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public string AuthorName { get; set; }

    public string AuthorProfilePicture { get; set; }

    public string Content { get; set; }

    public int LikeCount { get; set; }

    public int ReplyCount { get; set; }

    public TweetResponse[] ParentTweets { get; set; }

    public TweetResponse[] Replies { get; set; }
}
