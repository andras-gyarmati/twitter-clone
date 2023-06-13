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

    public UsersController(ILogger<UsersController> logger, TwitterCloneDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    /// <summary>
    ///   Get user by username
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    [HttpGet("{username}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(UserResponse))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(string username)
    {
        var user = await _context.Users
            .Include(x => x.Following)
            .Include(x => x.Followers)
            .FirstOrDefaultAsync(u => u.Username == username);
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
            ProfilePicture = user.ProfilePicture,
            Following = user.Following.Select(x => x.Followed.Username).ToArray(),
            FollowerCount = user.Followers.Count
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
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, user.Username)
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
        var userToFollow = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (userToFollow == null)
        {
            return NotFound();
        }
        User loggedInUser = null;
        if (User.Identity != null)
        {
            loggedInUser = await _context.Users
                .Include(x => x.Following)
                .FirstOrDefaultAsync(u => u.Username == User.Identity.Name);
        }
        if (loggedInUser == null)
        {
            return Unauthorized();
        }
        if (loggedInUser.Following.Any(x => x.FollowedId == userToFollow.Id))
        {
            return BadRequest("You are already following this user");
        }
        var follow = new UserUser
        {
            Follower = loggedInUser,
            Followed = userToFollow,
            FollowDate = DateTime.UtcNow
        };
        await _context.UserUsers.AddAsync(follow);
        await _context.SaveChangesAsync();
        return Ok();
    }

    /// <summary>
    ///     Unfollow user
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    [HttpPost("unfollow/{username}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    public async Task<IActionResult> Unfollow(string username)
    {
        var userToUnfollow = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        if (userToUnfollow == null)
        {
            return NotFound();
        }
        User loggedInUser = null;
        if (User.Identity != null)
        {
            loggedInUser = await _context.Users
                .Include(x => x.Following)
                .FirstOrDefaultAsync(u => u.Username == User.Identity.Name);
        }
        if (loggedInUser == null)
        {
            return Unauthorized();
        }
        if (!loggedInUser.Following.Any(x => x.FollowedId == userToUnfollow.Id))
        {
            return BadRequest("You are not following this user");
        }
        var follow = loggedInUser.Following.FirstOrDefault(x => x.FollowedId == userToUnfollow.Id);
        loggedInUser.Following.Remove(follow);
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
        var tweetResponses = await TweetsController.GetTweetResponseList(
            _context,
            HttpContext.Request.Headers["Filtering"].FirstOrDefault() ?? "",
            username);
        return Ok(tweetResponses);
    }
}
