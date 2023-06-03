namespace TwitterClone.Requests;

public class CreateTweetRequest
{
    public DateOnly CreatedAt { get; set; }

    public string AuthorName { get; set; }

    public string Content { get; set; }
}
