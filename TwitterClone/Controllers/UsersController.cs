using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TwitterClone.Extensions;
using TwitterClone.Models;
using TwitterClone.Requests;
using TwitterClone.Responses;

namespace TwitterClone.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly ILogger<UsersController> _logger;
    private readonly TwitterCloneDbContext _context;

    public static User LoggedInUser { get; set; }

    public UsersController(ILogger<UsersController> logger, TwitterCloneDbContext context)
    {
        _logger = logger;
        _context = context;
        LoggedInUser = new User();
    }

    /// <summary>
    ///   Get user by username
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    [HttpGet("{username}")]
    // [Authorize]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(UserResponse))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null)
        {
            return NotFound();
        }
        var userResponse = new UserResponse
        {
            Username = user.Username,
            Email = user.Email,
            BirthDate = user.BirthDate,
            Bio = user.Bio,
            ProfilePicture = user.ProfilePicture
        };
        return Ok(userResponse);
    }

    /// <summary>
    ///     Create user
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(string))]
    public async Task<IActionResult> Create([FromBody] CreateUserRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
        if (user != null)
        {
            return BadRequest("Username already exists");
        }
        var hashedPassword = request.Password.GetHash();
        var newUser = new User
        {
            Username = request.Username,
            Email = request.Email,
            Password = hashedPassword
        };
        await _context.Users.AddAsync(newUser);
        await _context.SaveChangesAsync();
        return Created($"/users/{newUser.Username}", newUser.Username);
    }

    /// <summary>
    ///     Update user
    /// </summary>
    /// <param name="username"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPatch("{username}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    public async Task<IActionResult> Update([FromRoute] string username, [FromBody] UpdateUserRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null)
        {
            return NotFound();
        }
        user.BirthDate = request.BirthDate;
        user.Bio = request.Bio;
        user.ProfilePicture = request.ProfilePicture;
        await _context.SaveChangesAsync();
        return Ok();
    }

    /// <summary>
    ///     Delete user
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    [HttpDelete("{username}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    public async Task<IActionResult> Delete([FromRoute] string username)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null)
        {
            return NotFound();
        }
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return Ok();
    }

    /// <summary>
    ///     Login user
    /// </summary>
    /// <param name="loginRequest"></param>
    /// <returns></returns>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginRequest.UserName);
        if (user == null || user.Password != loginRequest.Password.GetHash())
        {
            return Unauthorized();
        }
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes("use-the-generated-key-here"); // TODO: replace with your secret key
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Username.ToString())
            }),
            Expires = DateTime.UtcNow.AddDays(7), // Set the expiration as per your requirements
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return Ok(new { token = tokenHandler.WriteToken(token) });
    }

    /// <summary>
    ///     Follow user
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    [HttpPost("follow/{username}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    public async Task<IActionResult> Follow(string username)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (user == null)
        {
            return NotFound();
        }
        if (User.Identity != null)
        {
            var loggedInUsername = User.Identity.Name; // This gets the username from the JWT
            LoggedInUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == loggedInUsername);
        }
        if (LoggedInUser == null)
        {
            // return Unauthorized
            return Unauthorized();
        }
        var follow = new UserUser
        {
            Follower = LoggedInUser,
            Followed = user,
            FollowDate = DateTime.UtcNow
        };
        await _context.UserUsers.AddAsync(follow);
        await _context.SaveChangesAsync();
        return Ok();
    }

    /// <summary>
    ///     Get all tweets from user
    /// </summary>
    /// <returns></returns>
    [HttpGet("{username}/tweets")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<TweetResponse>))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TweetResponse>))]
    public async Task<IActionResult> GetTweets(string username)
    {
        var tweets = await _context.Tweets
            .Include(x => x.Author)
            .Where(t => !t.IsDeleted && t.Author.Username == username)
            .Take(10) // todo sort and use date
            .ToListAsync();
        var tweetResponses = tweets.Select(t => new TweetResponse
        {
            Id = t.Id,
            CreatedAt = t.CreatedAt,
            AuthorName = t.Author.Username,
            AuthorProfilePicture = t.Author.ProfilePicture,
            Content = t.IsDeleted ? "Deleted tweet" : t.Content
        }).ToList();
        return Ok(tweetResponses);
    }
}
