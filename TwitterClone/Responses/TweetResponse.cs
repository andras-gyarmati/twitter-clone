namespace TwitterClone.Responses;

public class TweetResponse
{
    public DateOnly CreatedAt { get; set; }

    public string AuthorName { get; set; }

    public string Content { get; set; }
}
