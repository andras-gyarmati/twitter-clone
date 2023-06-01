using Microsoft.AspNetCore.Mvc;
using TwitterClone.Models;

namespace TwitterClone.Controllers;

[ApiController]
[Route("[controller]")]
public class TweetsController : ControllerBase
{
    private readonly ILogger<TweetsController> _logger;

    public TweetsController(ILogger<TweetsController> logger)
    {
        _logger = logger;
    }

    private const string Lorem =
        "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.";

    [HttpGet]
    public IEnumerable<Tweet> Get()
    {
        return Enumerable.Range(1, 10).Select(index => new Tweet
            {
                CreatedAt = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                AuthorId = index,
                Content = Lorem[..Random.Shared.Next(Lorem.Length / 2, Lorem.Length)],
                IsDeleted = false,
            })
            .ToArray();
    }
}
