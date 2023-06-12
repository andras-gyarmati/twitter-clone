using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterClone.Models;

[Table("Tweet")]
public class Tweet
{
    [Key]
    public int Id { get; set; }

    public DateTime CreatedAt { get; set; }

    public int AuthorId { get; set; }

    [MaxLength(280)]
    public string Content { get; set; }

    public bool IsDeleted { get; set; }

    public User Author { get; set; }

    public int? InReplyToId { get; set; }

    public Tweet InReplyTo { get; set; }

    public List<Tweet> Replies { get; set; }

    public List<User> LikedByUsers { get; set; }
}
