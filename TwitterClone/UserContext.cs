using System.Security.Claims;

namespace TwitterClone;

public interface IUserContext
{
    ClaimsPrincipal User { get; }

    bool IsAuthenticated { get; }

    string Token { get; }

    string UserName { get; }
}

public class UserContext : IUserContext
{
    private readonly IHttpContextAccessor _context;

    public UserContext(IHttpContextAccessor context)
    {
        _context = context;
    }

    public ClaimsPrincipal User => _context?.HttpContext?.User;

    public bool IsAuthenticated => User?.Identity?.IsAuthenticated ?? false;

    public string Token => _context.HttpContext.Request.Headers.FirstOrDefault(x => x.Key == "Authorization").Value;

    public string UserName => User?.Identity?.Name;
}
