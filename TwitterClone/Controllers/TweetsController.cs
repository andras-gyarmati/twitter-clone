using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TwitterClone.Models;
using TwitterClone.Requests;
using TwitterClone.Responses;

namespace TwitterClone.Controllers;

[ApiController]
[Route("[controller]")]
public class TweetsController : ControllerBase
{
    private readonly ILogger<TweetsController> _logger;
    private readonly TwitterCloneDbContext _context;

    public TweetsController(ILogger<TweetsController> logger, TwitterCloneDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    /// <summary>
    ///     Get all tweets
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(List<TweetResponse>))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TweetResponse>))]
    public async Task<IActionResult> Get()
    {
        var tweets = await _context.Tweets
            .Include(x => x.Author)
            .Where(t => !t.IsDeleted)
            .Take(10) // todo sort and use date
            .ToListAsync();
        var tweetResponses = tweets.Select(t => new TweetResponse
        {
            CreatedAt = t.CreatedAt,
            AuthorName = t.Author.Username,
            Content = t.IsDeleted ? "Deleted tweet" : t.Content
        }).ToList();
        return Ok(tweetResponses);
    }

    /// <summary>
    ///     Get tweet by id
    /// </summary>
    /// <returns></returns>
    [HttpGet("{id:int}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(TweetResponse))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TweetResponse))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(int id)
    {
        var tweet = await _context.Tweets
            .Include(x => x.Author)
            .FirstOrDefaultAsync(t => t.Id == id);
        if (tweet == null)
        {
            return NotFound();
        }
        var tweetResponse = new TweetResponse
        {
            CreatedAt = tweet.CreatedAt,
            AuthorName = tweet.Author.Username,
            Content = tweet.IsDeleted ? "Deleted tweet" : tweet.Content
        };
        return Ok(tweetResponse);
    }

    /// <summary>
    ///     Create tweet
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    public async Task<IActionResult> Create(CreateTweetRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var author = await _context.Users.FirstOrDefaultAsync(x => x.Username == request.AuthorName);
        if (author == null)
        {
            return new BadRequestObjectResult("Author not found");
        }
        var tweet = new Tweet
        {
            CreatedAt = request.CreatedAt,
            AuthorId = author.Id,
            Content = request.Content,
            IsDeleted = false
        };
        _context.Tweets.Add(tweet);
        await _context.SaveChangesAsync();
        return Ok(tweet.Id);
    }

    /// <summary>
    ///     Delete tweet
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Delete(int id)
    {
        var existingTweet = await _context.Tweets.FindAsync(id);
        if (existingTweet == null)
        {
            return null;
        }
        existingTweet.IsDeleted = true;
        _context.Tweets.Update(existingTweet);
        await _context.SaveChangesAsync();
        return Ok();
    }
}
