using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwitterClone.Models;

[Table("User")]
public class User
{
    [Key] public int Id { get; set; }
    public string Email { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
    public DateTime BirthDate { get; set; }
    public string Bio { get; set; }
    public string ProfilePicture { get; set; }
    public List<Tweet> Tweets { get; set; }
}
