using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterClone.Models;

[Table("UserUser")]
public class UserUser
{
    public int FollowedId { get; set; }

    public User Followed { get; set; }

    public int FollowerId { get; set; }

    public User Follower { get; set; }

    public DateTime FollowDate { get; set; }
}
