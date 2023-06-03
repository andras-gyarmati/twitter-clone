namespace TwitterClone.Responses;

public class TweetResponse
{
    public DateTime CreatedAt { get; set; }

    public string AuthorName { get; set; }

    public string AuthorProfilePicture { get; set; }
    
    public string Content { get; set; }
}
