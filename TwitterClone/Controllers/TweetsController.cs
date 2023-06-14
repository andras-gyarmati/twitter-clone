using Microsoft.AspNetCore.Authorization;
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
    private readonly IUserContext _userContext;

    public TweetsController(ILogger<TweetsController> logger, TwitterCloneDbContext context, IUserContext userContext)
    {
        _logger = logger;
        _context = context;
        _userContext = userContext;
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
        var tweetResponses = await GetTweetResponseList(_context, HttpContext.Request.Headers["Filtering"].FirstOrDefault() ?? "");
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
            .Include(x => x.InReplyTo)
            .Include(x => x.Replies)
            .FirstOrDefaultAsync(t => t.Id == id);
        if (tweet == null)
        {
            return NotFound();
        }
        var tweetResponse = new TweetResponse
        {
            Id = tweet.Id,
            CreatedAt = tweet.CreatedAt,
            AuthorName = tweet.Author.Username,
            AuthorProfilePicture = tweet.Author.ProfilePicture,
            Content = tweet.IsDeleted ? "Deleted tweet" : tweet.Content,
            LikeCount = GetLikeCount(_context, tweet.Id),
            ReplyCount = GetReplyCount(_context, tweet),
            ParentTweets = GetParentTweets(_context, tweet),
            Replies = GetReplies(_context, tweet)
        };
        return Ok(tweetResponse);
    }

    private static TweetResponse[] GetReplies(TwitterCloneDbContext context, Tweet tweet)
    {
        if (tweet.Replies == null)
        {
            return Array.Empty<TweetResponse>();
        }
        return tweet.Replies.Where(x => !x.IsDeleted).Select(x => ToTweetResponse(context, x)).ToArray();
    }

    private static TweetResponse[] GetParentTweets(TwitterCloneDbContext context, Tweet tweet)
    {
        var parentTweets = new List<Tweet>();
        var parentTweet = tweet.InReplyTo;
        while (parentTweet != null)
        {
            parentTweets.Add(parentTweet);
            parentTweet = parentTweet.InReplyTo;
        }
        return parentTweets.OrderBy(x => x.CreatedAt).Select(x => ToTweetResponse(context, x)).ToArray();
    }

    /// <summary>
    ///     Create tweet
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    public async Task<IActionResult> Create(CreateTweetRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var author = await _context.Users.FirstOrDefaultAsync(x => x.Username == _userContext.UserName);
        if (author == null)
        {
            return Unauthorized();
        }
        var tweet = new Tweet
        {
            CreatedAt = DateTime.UtcNow,
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
    [Authorize]
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

    /// <summary>
    ///     Reply to tweet
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("{id:int}/reply")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    public async Task<IActionResult> ReplyTo(int id, CreateTweetRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var author = await _context.Users.FirstOrDefaultAsync(x => x.Username == _userContext.UserName);
        if (author == null)
        {
            return Unauthorized();
        }
        var tweet = new Tweet
        {
            CreatedAt = DateTime.UtcNow,
            AuthorId = author.Id,
            Content = request.Content,
            IsDeleted = false,
            InReplyToId = id
        };
        _context.Tweets.Add(tweet);
        await _context.SaveChangesAsync();
        return Ok(tweet.Id);
    }

    /// <summary>
    ///     Like tweet
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPost("{id:int}/like")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(int))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(int))]
    public async Task<IActionResult> LikeTweet(int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        var user = await _context.Users
            .Include(x => x.LikedTweets)
            .FirstOrDefaultAsync(x => x.Username == _userContext.UserName);
        if (user == null)
        {
            return Unauthorized();
        }
        var tweet = await _context.Tweets.FirstOrDefaultAsync(t => t.Id == id);
        if (tweet == null)
        {
            return NotFound();
        }
        if (user.LikedTweets.Any(t => t.Id == id))
        {
            return BadRequest("Tweet already liked");
        }
        user.LikedTweets.Add(tweet);
        await _context.SaveChangesAsync();
        return Ok();
    }

    public static async Task<List<TweetResponse>> GetTweetResponseList(TwitterCloneDbContext context, string filter, string username = null)
    {
        var from = DateTime.MinValue;
        try
        {
            from = DateTime.Parse(filter);
        }
        catch (Exception)
        {
            // ignored
        }
        var isFilteringForAllUsers = string.IsNullOrEmpty(username);
        var tweets = await context.Tweets
            .Include(x => x.Author)
            .Where(x => !x.IsDeleted && x.CreatedAt < from && (isFilteringForAllUsers || x.Author.Username == username))
            .OrderByDescending(x => x.CreatedAt)
            .Take(5)
            .ToListAsync();
        var tweetResponses = ToTweetResponseList(context, tweets);
        return tweetResponses;
    }

    private static List<TweetResponse> ToTweetResponseList(TwitterCloneDbContext context, List<Tweet> tweets)
    {
        var tweetResponses = tweets.Select(x => ToTweetResponse(context, x)).ToList();
        return tweetResponses;
    }

    private static TweetResponse ToTweetResponse(TwitterCloneDbContext context, Tweet tweet)
    {
        return new TweetResponse
        {
            Id = tweet.Id,
            CreatedAt = tweet.CreatedAt,
            AuthorName = tweet.Author.Username,
            AuthorProfilePicture = tweet.Author.ProfilePicture,
            Content = tweet.IsDeleted ? "Deleted tweet" : tweet.Content,
            LikeCount = GetLikeCount(context, tweet.Id),
            ReplyCount = GetReplyCount(context, tweet)
        };
    }

    private static int GetLikeCount(TwitterCloneDbContext context, int tweetId)
    {
        return context.Tweets
            .Where(t => t.Id == tweetId && !t.IsDeleted)
            .Take(1)
            .Select(x => x.LikedByUsers.Count)
            .FirstOrDefault();
    }

    private static int GetReplyCount(TwitterCloneDbContext context, Tweet tweet)
    {
        return context.Tweets.Count(t => t.InReplyToId == tweet.Id && !t.IsDeleted);
    }
}
