using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterClone.Models;

[Table("User")]
public class User
{
    [Key]
    public int Id { get; set; }

    [MaxLength(250)]
    public string Email { get; set; }

    [MaxLength(250)]
    public string Username { get; set; }

    [MaxLength(1024)]
    public string Password { get; set; }

    public DateTime BirthDate { get; set; }

    [MaxLength(1024)]
    public string Bio { get; set; }

    [MaxLength(1024)]
    public string ProfilePicture { get; set; }

    public List<Tweet> Tweets { get; set; }

    public List<UserUser> Following { get; set; }

    public List<UserUser> Followers { get; set; }

    public List<Tweet> LikedTweets { get; set; }
}
