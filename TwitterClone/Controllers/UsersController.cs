using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(UserResponse))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(string username)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Username == username);
        if (user == null)
        {
            return new NotFoundResult();
        }
        var userResponse = new UserResponse
        {
            Username = user.Username,
            Email = user.Email,
            BirthDate = user.BirthDate,
            Bio = user.Bio,
            ProfilePicture = user.ProfilePicture
        };
        return new OkObjectResult(userResponse);
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
        return new CreatedResult($"/users/{newUser.Username}", newUser.Username);
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
            return new NotFoundResult();
        }
        user.BirthDate = request.BirthDate;
        user.Bio = request.Bio;
        user.ProfilePicture = request.ProfilePicture;
        await _context.SaveChangesAsync();
        return new OkResult();
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
            return new NotFoundResult();
        }
        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return new OkResult();
    }
    
    /// <summary>
    ///     Login user
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    [HttpPost("login")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == loginRequest.UserName);
        if (user == null)
        {
            return new UnauthorizedResult();
        }
        return Ok(new { message = "hello" });
    }
}

public class LoginRequest
{
    public string UserName { get; set; }
    public string Password { get; set; }
}
